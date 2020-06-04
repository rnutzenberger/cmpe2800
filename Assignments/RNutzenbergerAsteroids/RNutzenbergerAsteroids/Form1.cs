using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RNutzenbergerAsteroids
{
    public partial class Form1 : Form
    {
        
        List<Asteroid> asteroidList = new List<Asteroid>();     //list of asteroids
        List<SpaceShip> spaceShips = new List<SpaceShip>();     //list of spaceships
        SpaceShip Player;                                       //current ship used
        int NumOfLives = 3;                                     //number if lives
        int Score = 0;                                          //score
        int SpawnTime = 250;                                    //asteroid spawn time

        bool GameOver = false;                                  //flag for when game is over
        bool Loaded = false;                                    //flag if game just loaded
        bool left = false;                                      //left flag 
        bool right = false;                                     //right flag
        bool up = false;                                        //up flag
        bool space = false;                                     //space flag
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            timer1.Tick += Timer1_Tick;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;

            //add the player ship
            Player = new SpaceShip(new PointF(ClientSize.Width / 2, ClientSize.Height / 2));
            //add the lives
            for(int i = 0, s = 20; i < 3; ++i, s += 25)
            {
                spaceShips.Add(new SpaceShip(new PointF(s, ClientSize.Height - 20)));
            }
            
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //change flags to false if the keys are released
            if (e.KeyCode == Keys.A)
            {
                left = false;
            }
            if (e.KeyCode == Keys.D)
            {
                right = false;
            }
            if (e.KeyCode == Keys.W)
            {
                up = false;
            }
            if(e.KeyCode == Keys.Space)
            {
                space = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            //change flags to true if keys are pressed down
            if (e.KeyCode == Keys.A)
            {
                left = true;
            }
            if (e.KeyCode == Keys.D)
            {
                right = true;
            }
            if (e.KeyCode == Keys.W)
            {
                up = true;
            }
            if(e.KeyCode == Keys.Space)
            {
                space = true;
            }
        }

        

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            //double buffering.....
            using(BufferedGraphicsContext bgc = new BufferedGraphicsContext())
            {
                using(BufferedGraphics bg = bgc.Allocate(CreateGraphics(), ClientRectangle))
                {
                    //clear the window
                    bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    bg.Graphics.Clear(Color.Black);
                    //draw the score and lives in
                    DrawScore(bg.Graphics, Color.White);
                    DrawLives(bg.Graphics, Color.White);

                    //if game over then game is over
                    if (GameOver)
                    {
                        bg.Graphics.DrawString("GAME OVER", new Font(FontFamily.GenericSansSerif, 15), new SolidBrush(Color.Orange), new PointF(ClientSize.Width / 2 -50, ClientSize.Height / 2));
                    }
                    else
                    {


                        //check collisions with the the player and asteroids
                        Region shipRegion = new Region(Player.GetPath());
                        foreach (Asteroid a in asteroidList)
                        {
                            //as long as the player has not recently spawned
                            if (!Player.Spawned)
                            {
                                //get the regions an check if theres an intersection
                                Region astRegion = new Region(a.GetPath());
                                astRegion.Intersect(shipRegion);
                                if (astRegion.GetRegionScans(new Matrix()).Length > 0)
                                {
                                    //if there is then the player is dead, reset it in the window 
                                    Player.Dead = true;
                                    Player = new SpaceShip(new PointF(ClientSize.Width/3, ClientSize.Height / 2));
                                    if (spaceShips.Count > 0)
                                    {
                                        //reduce the lives
                                        spaceShips.RemoveAt(spaceShips.Count - 1);
                                        NumOfLives--;
                                    }
                                    //if no more lives, game over
                                    if (NumOfLives == 0)
                                    {
                                        GameOver = true;
                                    }
                                }
                            }
                        }


                        //check collision between bullets and asteroid
                        foreach (Bullet b in Player.Bullets)
                        {
                            foreach (Asteroid a in asteroidList)
                            {
                                //get regions and intersection
                                Region bulletRegion = new Region(b.GetPath());
                                Region astRegion = new Region(a.GetPath());
                                astRegion.Intersect(bulletRegion);
                                //if there is an intersection the both asteroid and bullet are dead
                                if (astRegion.GetRegionScans(new Matrix()).Length > 0)
                                {
                                    b.Dead = true;
                                    a.Dead = true;

                                    //if the asteroid was large, give appropriate points and make medium asteroids
                                    if (a.AsteroidSize == Asteroid.ESize.Large)
                                    {
                                        Score += 100;
                                        for (int i = 0, dir = 90; i < 2; ++i, dir += 90)
                                        {
                                            CreateAsteroid(a.Location, Asteroid.ESize.Medium, a.Azimuth + dir);
                                        }
                                        break;
                                    }
                                    //if asteroid was medium then make small asteroids
                                    else if (a.AsteroidSize == Asteroid.ESize.Medium)
                                    {
                                        Score += 250;
                                        for (int i = 0, dir = 90; i < 3; ++i, dir += 90)
                                        {
                                            CreateAsteroid(a.Location, Asteroid.ESize.Small, a.Azimuth + dir);
                                        }
                                        break;
                                    }
                                    //if it was small then just add the score
                                    else
                                    {
                                        Score += 400;
                                        break;
                                    }
                                }
                            }
                        }

                        //make new asteroids and reset the timer
                        if (SpawnTime <= 0)
                        {
                            CreateAsteroid(new PointF(BaseShape._RND.Next(ClientSize.Width), BaseShape._RND.Next(ClientSize.Height)), Asteroid.ESize.Large, BaseShape._RND.Next(361));
                            SpawnTime = 150;
                        }
                        //move the player and asteroids around, render, and also remove any asteroids that are dead
                        Player.Tick(left, right, up, space, ClientSize);
                        Player.Render(bg.Graphics, Color.Yellow);

                        asteroidList.RemoveAll(a => a.Dead);
                        asteroidList.ForEach(a => a.Tick(ClientSize));
                        asteroidList.ForEach(a => a.Render(bg.Graphics, Color.Red));



                        //decrement the asteroid spawn time
                        --SpawnTime;
                        
                    }
                    //render
                    bg.Render();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Makes a new asteroid
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="size"></param>
        /// <param name="dir"></param>
        private void CreateAsteroid(PointF loc, Asteroid.ESize size, float dir)
        {
            asteroidList.Add(new Asteroid(loc, size, dir));
        }
        //draws the score
        private void DrawScore(Graphics g, Color c)
        {
            g.DrawString(Score.ToString("D7"), new Font(FontFamily.GenericSansSerif, 15), new SolidBrush(c), new PointF(0, 0));
        }
        //draws the lives
        private void DrawLives(Graphics g, Color c)
        {
            spaceShips.ForEach(x => x.Render(g, c));
        }
    }
}

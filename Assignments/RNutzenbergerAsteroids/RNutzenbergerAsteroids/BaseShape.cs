using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using GDIDrawer;

namespace RNutzenbergerAsteroids
{
    #region BaseShape
    public abstract class BaseShape
    {
        public static Random _RND = new Random();                    //Static Random 
        public float Rotation { get; set; }                   //Current Rotation for each shape
        public float RotationInc { get; set; } = 7.5F;        //Rotation Increment for each shape
        public float Azimuth { get; set; }                    //Azimuth of shape nose
        public float Accel { get; set; } = 0.2F;              //Acceleration of shape
        public float XInc { get; set; }                       //X increment of shape per tick
        public float YInc { get; set; }                       //Y increment of shape per tick
        public float Speed { get; set; }                      //Current speed of shape
        public bool Dead { get; set; }                        //flag for whether the shape has been killed
        public GraphicsPath Model { get; set; }               //Model of shape

        public PointF Location;                                         //Current Location of shape
        public Size GameSize { get; set; }

        /// <summary>
        /// initialize the Location and Rotation
        /// </summary>
        /// <param name="p"></param>
        public BaseShape(PointF p)
        {
            Location = p;
            Rotation = 180;
        }

        /// <summary>
        /// Render the shape
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        public abstract void Render(Graphics g, Color c);

        /// <summary>
        /// Translate and Rotate the shape
        /// </summary>
        /// <returns></returns>
        public virtual GraphicsPath GetPath()
        {
            Matrix m = new Matrix();
            GraphicsPath gp = (GraphicsPath)Model.Clone();
            m.Rotate(Rotation);
            m.Translate(Location.X, Location.Y,MatrixOrder.Append);
            gp.Transform(m);
            return gp;
        }

        
    }
    #endregion

    #region SpaceShip
    class SpaceShip : BaseShape 
    {
        public List<Bullet> Bullets = new List<Bullet>();           //List of Bullets for the ship
        public const int MaxBullets = 8;                            //Max number of bullets on screen
        public bool Alive { get; set; } = true;                     //flag the status of the ship
        public int SpawnTimer = 100;                                //time the ship is immune
        public bool Spawned;                                        //flag the spawn status
        
        public GraphicsPath Ship;                                   //Model of ship

        public SpaceShip(PointF p) : base(p)
        {
            //create the spaceship
            PointF[] points = new PointF[4];
            points[0] = new PointF(0, 15);
            points[1] = new PointF(10, -10);
            points[2] = new PointF(0, -4);
            points[3] = new PointF(-10, -10);
            Ship = new GraphicsPath();
            Ship.AddPolygon(points);

            Spawned = true;
            
        }

        /// <summary>
        /// translates and rotates the model
        /// </summary>
        /// <returns></returns>
        public override GraphicsPath GetPath()
        {
            Matrix mat = new Matrix();
            mat.Rotate(this.Rotation);
            mat.Translate(this.Location.X, this.Location.Y, MatrixOrder.Append);
            GraphicsPath gp = (Ship.Clone() as GraphicsPath);
            gp.Transform(mat);
            return gp;
        }

        /// <summary>
        /// renders the model and bullets
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        public override void Render(Graphics g, Color c)
        {
            g.DrawPath(new Pen(c), GetPath());
            Bullets.ForEach(b => b.Render(g, Color.White));

        }

        /// <summary>
        /// Moves the spaceship and bullets 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="up"></param>
        /// <param name="space"></param>
        /// <param name="s"></param>
        public void Tick(bool left, bool right, bool up,bool space, Size s)
        {
            //set the game size
            GameSize = s;

            //if the ship is finished spawning then let it be hit
            if(SpawnTimer-- <= 0)
            {
                Spawned = false;
            }
            //left and right inputs change rotation
            if (left)
            {
                Rotation -= RotationInc;
            }
            if (right)
            {
                Rotation += RotationInc;
            }
            //set the azimuth to the rotation
            Azimuth = Rotation;
            
            //if up is pushed then calc the distance the ship
            if (up)
            {
                XInc += -(float)Math.Sin(Rotation * Math.PI / 180) * Accel;
                YInc += (float)Math.Cos(Rotation * Math.PI / 180) * Accel;
            }
            //save the speed
            Speed = (float)(Math.Sqrt(Math.Pow(XInc, 2) + Math.Pow(YInc, 2)));

            //if the ship moves out of the window then set i to the other side of the window
            //else just increment the position of the 
            if(Location.X + XInc < 0)
            {
                Location.X = GameSize.Width;
            }
            else if(Location.X + XInc > GameSize.Width)
            {
                Location.X = 0;
            }
            else
            {
                Location.X += XInc;
            }
            if (Location.Y + YInc < 0)
            {
                Location.Y = GameSize.Height;
            }
            else if (Location.Y + YInc > GameSize.Height)
            {
                Location.Y = 0;
            }
            else
            {
                Location.Y += YInc;
            }

            //if space is pushed(shoot) shoot the bullets
            if(space && Bullets.Count < MaxBullets)
            {
                Bullets.Add(new Bullet(Location, Azimuth, Speed));
            }

            //remove all collided bullets and tick the bullets too
            Bullets.RemoveAll(x=> x.Dead);
            Bullets.ForEach(x=> x.Tick(GameSize));

            

        }

        



    }
    #endregion

    #region Bullet
    class Bullet : BaseShape
    {

        public float Size { get; } = 4;     //Bullet Size
        
        //initialize the bullets
        public Bullet(PointF p, float dir, float speed) : base(p)
        {
            Azimuth = dir;
            Speed = speed + 5;
            Model = new GraphicsPath();
            Model.AddEllipse(Size / 2, Size / 2, Size, Size);
            Dead = false;
        }

       
        /// <summary>
        /// move the bullet
        /// </summary>
        /// <param name="s"></param>
        public void Tick(Size s)
        {
            GameSize = s;

            XInc = -(float)Math.Sin(Azimuth * Math.PI / 180) * Speed;
            YInc = (float)Math.Cos(Azimuth * Math.PI / 180) * Speed;

            Location.X += XInc;
            Location.Y += YInc;

            //bullet dies if it hits the borders
            if(Location.X > GameSize.Width || Location.X < 0)
            {
                Dead = true;
            }
            if(Location.Y < 0 || Location.Y > GameSize.Height)
            {
                Dead = true;
            }
        }

        /// <summary>
        /// renders the bullet
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        public override void Render(Graphics g, Color c)
        {
            g.DrawPath(new Pen(c), GetPath());
        }

    }
    #endregion

    #region Asteroid
    class Asteroid : BaseShape
    {
        public enum ESize { Small, Medium, Large};              //enum for size of asteroid
        public ESize AsteroidSize;                              //init enum
        public const int _size = 40;                            //asteroid size

        //make an asteroid
        public Asteroid(PointF p, ESize eSize, float dir): base(p)
        {

            //random rotation and delta values and acceleration 
            RotationInc = (float)_RND.NextDouble() * 5 - 5;
            XInc = (float)_RND.NextDouble() * 5 - 5;
            YInc = (float)_RND.NextDouble() * 5 - 5;
            Accel = (float)_RND.NextDouble() * 0.1f - 2.0f;
            AsteroidSize = eSize;
            Azimuth = dir;

            //amount of vertices between 6 and 13
            int vertices = _RND.Next(6, 14);
            PointF[] points = new PointF[vertices];
            Matrix m = new Matrix();
            double theta = 0.0;

            //make the points
            for(int i = 0; i < vertices; ++i)
            {
                //get the rotation
                theta = (Math.PI * 2 / vertices) * i;
                //create a new point 
                PointF temp = new PointF();
                //x location is cosine of theta * radius - rnd(between 0 and 1) * radius * varience
                //y is same but using sin
                temp.X = (float)(Math.Cos(theta) * (_size - _RND.NextDouble() * _size * 0.7f));
                temp.Y = (float)(Math.Sin(theta) * (_size - _RND.NextDouble() * _size * 0.7f));

                //add the point
                points[i] = temp;

            }

            //add the shape to the model
            Model = new GraphicsPath();
            Model.AddPolygon(points);

            //scale the matrix depending what size the asteroid is then transform
            if(AsteroidSize == ESize.Large)
            {
                m.Scale(1.5f,1.5f);
            }
            if(AsteroidSize == ESize.Medium)
            {
                m.Scale(1.0f, 1.0f);
            }
            if(AsteroidSize == ESize.Small)
            {
                m.Scale(0.5f, 0.5f);
            }

            Model.Transform(m);

        }

        /// <summary>
        /// renders the asteroid
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        public override void Render(Graphics g, Color c)
        {
            g.DrawPath(new Pen(Color.Red), GetPath());
        }

        /// <summary>
        /// moves the asteroid
        /// </summary>
        /// <param name="s"></param>
        public void Tick(Size s)
        {
            //set the new rotation and speeds
            GameSize = s;
            Rotation += RotationInc;
            XInc = -(float)Math.Sin(Azimuth * Math.PI / 180) * Accel;
            YInc = (float)Math.Cos(Azimuth * Math.PI / 180) * Accel;

            //wraps the asteroid if it goes past the window extents.
            //else it moves around as normal
            if (Location.X + XInc < 0)
            {
                Location.X = GameSize.Width;
            }
            else if (Location.X + XInc > GameSize.Width)
            {
                Location.X = 0;
            }
            else
            {
                Location.X += XInc;
            }
            if (Location.Y + YInc < 0)
            {
                Location.Y = GameSize.Height;
            }
            else if (Location.Y + YInc > GameSize.Height)
            {
                Location.Y = 0;
            }
            else
            {
                Location.Y += YInc;
            }
        }


    }
    #endregion


}


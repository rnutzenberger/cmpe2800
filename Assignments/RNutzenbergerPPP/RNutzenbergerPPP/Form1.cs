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
using GDIDrawer;
using System.Diagnostics;

namespace RNutzenbergerPPP
{
    public partial class Form1 : Form
    {

        //structure to hold both the region and time of the collided shapes
        struct RegionTime
        {
            public Region Region;   //region of shape
            public long Time;       //time shape was made(used from the stopwatch)

            public RegionTime(Region r, long t)
            {
                Region = r;
                Time = t;
            }

        }

        Random rnd = new Random();          //Random for random location with shift modifier
        List<ShapeBase> shapeList = new List<ShapeBase>();  //list of shapes
        List<RegionTime> collisionList = new List<RegionTime>();    //list of Region and time
        Stopwatch sw = new Stopwatch(); //stopwatch for collision removal
        public Form1()
        {
            InitializeComponent();

            Load += Form1_Load;
            _Timer_25ms.Tick += _Timer_25ms_Tick;
            MouseDown += Form1_MouseDown;
            _Timer_25ms.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //restart the stopwatch
            sw.Restart();

        }

        private void _Timer_25ms_Tick(object sender, EventArgs e)
        {

            //Double buffering stuff
            using(BufferedGraphicsContext bgc = new BufferedGraphicsContext())
            {
                using(BufferedGraphics bg = bgc.Allocate(CreateGraphics(),ClientRectangle))
                {
                    //clear the current grapics, setting the form color back to the same color
                    bg.Graphics.Clear(Color.FromKnownColor(KnownColor.Control));
                    //anti alias for smoothing
                    bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    //tick each shape in the list
                    shapeList.ForEach(sh => sh.Tick(ClientSize));
                    //render each shape. Different colors for each shape in there
                    shapeList.ForEach(sh =>
                    {
                        //triangle is black, rock is purple
                        if (sh is Triangle)
                        {
                            sh.Render(Color.Black, bg.Graphics);
                        }
                        else
                        {
                            sh.Render(Color.Purple, bg.Graphics);
                        }

                    });

                    //collision checking. foreach current shape we are looking at
                    foreach(ShapeBase current  in shapeList)
                    {
                        //get the region of that shape
                        Region outerRegion = new Region(current.GetPath());

                        //iterate through the same list checking each shape
                        foreach(ShapeBase check in shapeList)
                        {
                            //if the current distance is less than the diameter AND is not itself
                            if(current.Dist(check) < ShapeBase._cfRadius * 2 && !ReferenceEquals(current, check))
                            {
                                //get the intersecting shape region
                                Region intersectRegion = new Region(check.GetPath());
                                //get our intersecting region
                                intersectRegion.Intersect(outerRegion);

                                //if there is an intersection
                                if(intersectRegion.GetRegionScans(new Matrix()).Length > 0)
                                {
                                    //mark both shapes to be dead(collided)
                                    current.IsMarkedForDeath = true;
                                    check.IsMarkedForDeath = true;
                                    //add the region intersecting region and the time it happened
                                    collisionList.Add(new RegionTime(intersectRegion, sw.ElapsedMilliseconds));
                                }
                            }
                        }
                    }

                    //fill the collision area in with with blue using the intersecting region saved
                    collisionList.ForEach(r => bg.Graphics.FillRegion(new SolidBrush(Color.DarkBlue), r.Region));
                    //remove all collisions if it has been 500ms 
                    collisionList.RemoveAll(r => r.Time + 500 < sw.ElapsedMilliseconds);
                    //remove all of the shapes that are now dead 
                    shapeList.RemoveAll(s => s.IsMarkedForDeath);


                    //render the graphics to the window
                    bg.Render();

                
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //if the shift has not been pressed
            if(Control.ModifierKeys != Keys.Shift)
            {
                //add one triangle if left button is click, one rock if right button is clicked
                if(e.Button == MouseButtons.Left)
                {
                    shapeList.Add(new Triangle(e.Location));
                }
                if(e.Button == MouseButtons.Right)
                {
                    shapeList.Add(new Rock(e.Location));
                }
            }
            //shift is pressed
            else
            {
                //add 1000 triangles or rocks based on if left or right was clicked
                if(e.Button == MouseButtons.Left)
                {
                    for(int i = 0; i< 1000; ++i)
                    {
                        shapeList.Add(new Triangle(new PointF(rnd.Next(0, ClientSize.Width), rnd.Next(0, ClientSize.Height))));
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < 1000; ++i)
                    {
                        shapeList.Add(new Rock(new PointF(rnd.Next(0, ClientSize.Width), rnd.Next(0, ClientSize.Height))));
                    }
                }

            }
        }
    }
}

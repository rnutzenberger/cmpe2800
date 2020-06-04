using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using GDIDrawer;

namespace RNutzenbergerPPP
{
    abstract class ShapeBase
    {
        protected static Random _rnd = new Random();    //random for determining the rotation incremnet, and velocities
        public const float _cfRadius = 10;              //constant radius
        protected float _fXSpeed;                       //x speed
        protected float _fYSpeed;                       //y speed
        protected float _frotation;                     //rotation of shape
        protected float _fRotationIncrement;            //rotation increment

        public PointF Location { get; protected set; }  //Location property
        public bool IsMarkedForDeath { get; set; }      //bool ot determine if shape is dead/intersected
        public Color BaseColor { get; protected set; }  //Shape color

         //constructor
        public ShapeBase(PointF p)
        {
            
            //set the location, rotation, increment, and x and y speeds
            Location = p;
            _frotation = 0;
            _fRotationIncrement = (float)(_rnd.NextDouble() * 6 - 3.0);
            _fXSpeed = (float)(_rnd.NextDouble() * 5 - 2.5);
            _fYSpeed = (float)(_rnd.NextDouble() * 5 - 2.5);
        }

        //abstract GetPath for other classes
        public abstract GraphicsPath GetPath();

        //Fills the graphics path based on the fill color and getPath function
        public void Render(Color fillCol, Graphics canvas)
        {
            canvas.FillPath(new SolidBrush(fillCol), GetPath());
        }

        /// <summary>
        /// Moves and rotates each object within the window extents
        /// </summary>
        /// <param name="size"></param>
        public void Tick(Size size)
        {
            //increment the rotation
            _frotation += _fRotationIncrement;

            //reverse the speed sign if it hits any side of the window extents
            if(Location.X <= _cfRadius || Location.X >= size.Width - _cfRadius)
            {
                _fXSpeed *= -1;
            }
            if(Location.Y <= _cfRadius || Location.Y >= size.Height - _cfRadius)
            {
                _fYSpeed *= -1;

            }

            //create a new point based on current location plus speed
            float x = (float)(Location.X + _fXSpeed);
            float y = (float)(Location.Y + _fYSpeed);
            Location = new PointF(x, y);


            //if statments used to prevent clipping into the extents and the object getting stuck
            //if the location is less than the radius, then just set it to the radius size to prevent clipping
            if(Location.X < _cfRadius)
            {
                x = _cfRadius;
                y = Location.Y;
                Location = new PointF(x, y);
            }
            if (Location.X > size.Width - _cfRadius)
            {
                x = size.Width - _cfRadius;
                y = Location.Y;
                Location = new PointF(x, y);
            }
            if (Location.Y < _cfRadius)
            {
                x = Location.X;
                y = _cfRadius;
                Location = new PointF(x, y);
            }
            if (Location.Y > size.Height - _cfRadius)
            {
                x = Location.X;
                y = size.Height - _cfRadius;
                Location = new PointF(x, y);
            }
        }

        /// <summary>
        /// creates the polygonal shape
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="vertexCount"></param>
        /// <param name="varience"></param>
        /// <returns></returns>
        static protected GraphicsPath MakePolyPath(float radius, int vertexCount, float varience)
        {
            //point array for the polygon sized to however many vertices the object has
            PointF[] points = new PointF[vertexCount];
            //theta to calc the angle between points
            double theta = 0.0;

            //iterate through creating every point
            for(int i = 0; i < vertexCount; ++i)
            {
                //get the rotation
                theta = (Math.PI * 2 / vertexCount) * i;
                //create a new point 
                PointF temp = new PointF();
                //x location is cosine of theta * radius - rnd(between 0 and 1) * radius * varience
                //y is same but using sin
                temp.X = (float)(Math.Cos(theta) * (radius - _rnd.NextDouble() * radius * varience));
                temp.Y = (float)(Math.Sin(theta) * (radius - _rnd.NextDouble() * radius * varience));

                //add the point
                points[i] = temp;
            }

            //create the graphics path and add the new polygon
            GraphicsPath output = new GraphicsPath();
            output.AddPolygon(points);

            return output;
        }

        /// <summary>
        /// Gets the distance between two shapes
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public float Dist(ShapeBase arg)
        {
            return (float)Math.Sqrt(Math.Pow(this.Location.X - arg.Location.X, 2) + Math.Pow(this.Location.Y - arg.Location.Y, 2));
        }
    }

    class Triangle : ShapeBase
    {
        //unchangeable model
        static readonly GraphicsPath model;
        //model is made with 3 vertex polygon(a Triangle)
        static Triangle()
        {
            model = MakePolyPath(_cfRadius, 3, 0);
        }
        //call the base constructor to set the location and whatnot
        public Triangle(PointF p) : base(p) { }
        
        //override the getPath() to make a copy of our model with appropriate transforms
        public override GraphicsPath GetPath()
        {
            //Make a tranform using Matrix
            Matrix matrix = new Matrix();
            //Rotate, translate tranform
            matrix.Rotate(this._frotation);
            matrix.Translate(this.Location.X, this.Location.Y,MatrixOrder.Append);
            //clone the model and tranform it, then return
            GraphicsPath gp = (model.Clone() as GraphicsPath);
            gp.Transform(matrix);
            return gp;

        }
    }

    class Rock : ShapeBase
    {
        //non-static model
        readonly GraphicsPath model;
        //call the base constructor, using a radnom amount of vertices and a varience of 30%
        public Rock(PointF p) : base(p) 
        {
            this.model = MakePolyPath(_cfRadius, _rnd.Next(6, 12), .30F);
        }

        //override the getPath() to make a copy of our model with appropriate transforms
        public override GraphicsPath GetPath()
        {
            //make transform using Matrix
            Matrix matrix = new Matrix();
            //Rotate, Translate
            matrix.Rotate(this._frotation);
            matrix.Translate(this.Location.X, this.Location.Y,MatrixOrder.Append);
            //clone model and tranform it, then return
            GraphicsPath gp = (model.Clone() as GraphicsPath);
            gp.Transform(matrix);
            return gp;
        }
    }
}

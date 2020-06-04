using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GDIDrawer;


namespace RNutzenebergerICA2 
{
    public class Ball : IComparable
    {
        private static Random _rnd = new Random();
        private Point _ballLoc;
        private int _xVel;
        private int _yVel;
        public int _ballRad;
        public Color BallColor { get; set; }

        public Ball(Point ballLoc, Color ballColor)
        {
            _xVel = _rnd.Next(-5, 6); // -5  to 5 range? 10, 0-10 set offset ? -5
            _yVel = _rnd.Next(-5, 6);
            _ballRad = _rnd.Next(20, 51);
            _ballLoc = ballLoc;
            BallColor = ballColor;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ball arg))
            {
                return false;
            }

            return (CheckDiff(this, arg) < _ballRad + arg._ballRad);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Ball arg))
            {
                throw new Exception("Object is not a Block.");
            }
            return _ballRad.CompareTo(arg._ballRad);

        }

        public override int GetHashCode()
        {
            return 1;
        }

        private static double CheckDiff(Ball _ballOne, Ball _ballTwo)
        {
            double _diffX = _ballTwo._ballLoc.X - _ballOne._ballLoc.X;
            double _diffY = _ballTwo._ballLoc.Y - _ballOne._ballLoc.Y;
            double _diffZ = Math.Sqrt(Math.Pow(_diffX, 2) + Math.Pow(_diffY, 2));
            return _diffZ;
        }

        public void Move(CDrawer _canvas)
        {

            // conditions to make sure the ball does not pass throught the window extents left and right
            //if the ball's x coordinate is less than 40(the radius) or greater than 760(the extent minus radius)
            //then flip the sign of the current x velocity
            if (_ballLoc.X <= _ballRad || _ballLoc.X >= _canvas.ScaledWidth - _ballRad)
            {

                _xVel *= -1;
            }


            //conditions to make sure the ball does not pass throught the window extents up and down
            //if the ball's y coordinate is less than 40(the radius) or greater than 560(the extent minus radius)
            //then flip the sign of the current y velocity
            if (_ballLoc.Y <= _ballRad || _ballLoc.Y >= _canvas.ScaledHeight - _ballRad)
            {

                _yVel *= -1;
            }

            //the centre position of the ball will increment by the x and y velocities
            _ballLoc.X += _xVel;
            _ballLoc.Y += _yVel;

            //check if both the x and y positions after incrementing are prone to clipping past the 
            //extents of the window. So if the x or y positions are EX) 30, that would result in it clipping past.
            //If it has potential of clipping past, then make the x and y positions equal to the radius of the ball
            //or the canvas width minus the ball radius, depending on where the ball is hitting the the canvas
            if (_ballLoc.X < _ballRad)
            {
                _ballLoc.X = _ballRad;
            }
            if (_ballLoc.X > _canvas.ScaledWidth - _ballRad)
            {
                _ballLoc.X = _canvas.ScaledWidth - _ballRad;
            }

            if (_ballLoc.Y < _ballRad)
            {
                _ballLoc.Y = _ballRad;
            }
            if (_ballLoc.Y > _canvas.ScaledHeight - _ballRad)
            {
                _ballLoc.Y = _canvas.ScaledHeight - _ballRad;
            }


        }

        public void Show(CDrawer _canvas, int _ballNum)
        {
            Color comp = Color.FromArgb(BallColor.ToArgb() ^ 0x00FFFFFF);
            _canvas.AddCenteredEllipse((int)_ballLoc.X, (int)_ballLoc.Y, _ballRad * 2, _ballRad * 2, BallColor);
            _canvas.AddText(_ballNum.ToString(), 14, (int)_ballLoc.X - _ballRad, (int)_ballLoc.Y - _ballRad, _ballRad * 2, _ballRad * 2, comp);
        }
    }
}

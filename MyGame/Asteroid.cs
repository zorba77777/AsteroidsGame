using System;
using System.Drawing;

namespace MyGame
{
    class Asteroid : BaseObject, ICloneable, IComparable

    {
        public int Power { get; set; } = 3;

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height));
            asteroid.Power = Power;
            return asteroid;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is Asteroid temp) {
                if (Power > temp.Power)
                    return 1;
                if (Power < temp.Power)
                    return -1;
                else
                    return 0;
            }
            throw new ArgumentException("Parameter is not аn Asteroid!");
        }


        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) {
                Dir.X = -Dir.X;
            }
            if (Pos.X > Game.Width) {
                Dir.X = -Dir.X;
            }
            if (Pos.Y < 0) {
                Dir.Y = -Dir.Y;
            }

            if (Pos.Y > Game.Height) {
                Dir.Y = -Dir.Y;
            }
        }
    }

}

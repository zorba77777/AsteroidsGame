using System;
using System.Drawing;

namespace MyGame
{
    // Class for creating object "First aid kit". If ship collide this object, his energy will be replenished.
    class FirstAidKit : BaseObject
    {
        public FirstAidKit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillRectangle(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
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

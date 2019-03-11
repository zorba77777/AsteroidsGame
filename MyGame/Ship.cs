using System;
using System.Drawing;

namespace MyGame
{
    // Class for creating object "Space ship". 
    // A ship has the energy that it loses when collide with asteroids and replenish when collide with First Aid Kit.
    // A ship dies when it loses all its energy.
    class Ship : BaseObject
    {
        private int _energy = 100;
        public int Energy => _energy;

        public static event Message MessageDie;

        public void EnergyLow(int n)
        {
            _energy -= n;
        }

        public void EnergyHigh(int n)
        {
            _energy += n;
        }

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }
    }

}

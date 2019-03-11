using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace MyGame
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }

        // Define variables that contain objects on the gamefield
        private static int asteroidCount = 3;
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        private static List<Bullet> _bullets = new List<Bullet>();
        public static Star[] _stars;
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
        private static FirstAidKit _firstAidKit = new FirstAidKit(new Point(400, 400), new Point(5, 5), new Size(10, 10));

        // The timer is used to control movement on the gamefield
        private static Timer _timer = new Timer { Interval = 100 };

        // Rnd is used to create random size and position to the objects
        private static Random Rnd = new Random();

        // Variable to count points for hit asteroids
        private static int _score = 0;

        static Game()
        {
        }

        // Method initiates the gamefield and the timer
        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;            

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            // Use the timer to create movement on the gamefield
            _timer.Start();
            _timer.Tick += Timer_Tick;

            // Bind the method Form_KeyDown to the form event "KeyDown" to control the ship on the gamefield
            form.KeyDown += Form_KeyDown;

            // Bind the method Finish to the event of the ship. This event invokes when ship loses all his energy.
            Ship.MessageDie += Finish;           

        }

        // Method draws all objects in the gamefield each tick of the timer
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            foreach (Star star in _stars) {
                star?.Draw();
            }

            foreach (Asteroid asteroid in _asteroids) {
                asteroid?.Draw();
            }

            foreach (Bullet bul in _bullets) {
                bul.Draw();
            }

            _firstAidKit.Draw();
            _ship?.Draw();

            if (_ship != null) {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }

            Buffer.Graphics.DrawString("Score: " + _score, SystemFonts.DefaultFont, Brushes.White, 70, 0);

            Buffer.Render();
        }

        // Method initially creates all objects on the gamefield, except bullets
        public static void Load()
        {            
            CreateStars();
            CreateAsteroids();
        }

        // Method updates all objects on the gamefield and handle collisions, if it happens, each tick of the timer
        public static void Update()
        {
            foreach (Star star in _stars) {
                star.Update();
            }

            foreach (Bullet bul in _bullets) {
                bul.Update();
            }

            if (_asteroids.Count == 0) {
                CreateAsteroids();
            }

            for (var i = 0; i < _asteroids.Count; i++) {

                _asteroids[i].Update();

                if (_ship.Collision(_asteroids[i])) {
                    _ship.EnergyLow(Rnd.Next(1, 10));

                    System.Media.SystemSounds.Asterisk.Play();

                    if (_ship.Energy <= 0) {
                        _ship.Die();
                    }
                }

                for (int j = 0; j < _bullets.Count; j++) {

                    if (_bullets[j].Collision(_asteroids[i])) {
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids.RemoveAt(i);
                        _bullets.RemoveAt(j);
                        j--;
                        _score++;
                        break;
                    }
                }                
            }

            _firstAidKit.Update();

            if (_firstAidKit != null && _firstAidKit.Collision(_ship)) {
                _ship.EnergyHigh(1);
            }
        }

        // Method is used for control the ship
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) {
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1)));
            }

            if (e.KeyCode == Keys.Up) {
                _ship.Up();
            }

            if (e.KeyCode == Keys.Down) {
                _ship.Down();
            }
        }

        // Method creates movement on the gamefield using the timer
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        // Method calls when the game is over, because the player lost all energy of the ship
        public static void Finish()
        {
            _timer.Stop();

            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            
            Buffer.Render();
        }

        // Create objects named "Star"
        private static void CreateStars()
        {
            _stars = new Star[30];
            for (var i = 0; i < _stars.Length; i++) {
                int r = Rnd.Next(5, 50);
                _stars[i] = new Star(new Point(Rnd.Next(0, Game.Width), Rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
        }

        // Create objects named "Asteroid"
        private static void CreateAsteroids()
        {
            for (var i = 0; i < asteroidCount; i++) {
                int r = Rnd.Next(5, 50);
                _asteroids.Add(new Asteroid(new Point(Rnd.Next(0, Game.Width), Rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
            }
            asteroidCount++;
        }

    }
}

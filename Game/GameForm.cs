using Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GameForm : Form
{
    private bool started = false;
    private bool wasHouse = false;
    private bool wasOut = true;
    private bool mouseClicked = false;
    private bool spacePressed = false;
    private Rectangle mouseRect = new Rectangle(0, 0, 0, 0);
    private static Rectangle sB = new Rectangle(382, 263, 304, 92);
    private static Rectangle qB = new Rectangle(382, 383, 304, 92);
    private StartButton start = new StartButton(sB);
    private StartButton quit = new StartButton(qB);
    private Bitmap Menu = Methods.LoadTexture("Assets/UI/Menu.png");

    private Timer gameTimer;
    private Player player;
    private Camera camera;
    private Bitmap Grass1 = Methods.LoadTexture("Assets/Tiles/Mappy.png");

    private List<Rectangle> outSolidObjects = new List<Rectangle>();
    private List<Rectangle> inSolidObjects = new List<Rectangle>();
    private Tile house = new Tile(new Rectangle(0, 0, 110, 140), new Rectangle(80, 40, 110, 140), "Assets/Tiles/Haus1.png", 2);
    private static Rectangle door = new Rectangle(188, 271, 10, 10);
    private static Rectangle goOut = new Rectangle(188, 271, 100, 100);

    //Polymorphism
    private List<IDrawEntities> inside = new List<IDrawEntities>();

    private List<Enemy> enemies = new List<Enemy>();
    private Random rng = new Random();
    private string enemySpritePath = "Assets/NPCs/Enemy.png";

    public GameForm()
    {
        this.DoubleBuffered = true;
        this.ClientSize = new Size(1024, 640);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = true;

        player = new Player(Methods.WorldLimit, outSolidObjects, inSolidObjects);
        camera = new Camera(ClientSize.Width, ClientSize.Height, Methods.WorldLimit);

        this.KeyDown += (s, e) =>
        {
            player.OnKeyDown(e.KeyCode);
            if (e.KeyCode == Keys.Space)
                spacePressed = true;
        };
        this.KeyUp += (s, e) =>
        {
            player.OnKeyUp(e.KeyCode);
            if (e.KeyCode == Keys.Space)
                spacePressed = false;
        };

        inside.Add(house);
        inside.Add(player);

        outSolidObjects.Add(new Rectangle(90, 170, 200, 100));
        inSolidObjects.Add(new Rectangle(0, 0, 10, 10));

        SpawnEnemies();

        gameTimer = new Timer();
        gameTimer.Interval = 16;
        gameTimer.Tick += (s, e) =>
        {
            player.Update();

            foreach (var enemy in enemies)
                enemy.Update();

            if (spacePressed)
            {
                Rectangle playerRect = player.GetPlayerRect();
                foreach (var enemy in enemies)
                {
                    if (!enemy.IsDead && playerRect.IntersectsWith(enemy.GetHitbox()))
                    {
                        enemy.TakeDamage(1);
                        break;
                    }
                }
            }
            if (Methods.Scene == "Outside")
            {
                camera.Follow(player._posX, player._posY);
            }
            else
            {
                camera.CameraStop();
            }
                Invalidate();
        };
        gameTimer.Start();
    }

    private void SpawnEnemies()
    {
        int count = rng.Next(3, 13);

        Rectangle screenBounds = new Rectangle(0, 0, 1024, 640);

        for (int i = 0; i < count; i++)
        {
            int x, y;
            Rectangle spawnRect;

            do
            {
                x = rng.Next(Methods.WorldLimit.xMin + 50, Methods.WorldLimit.xMax - 50);
                y = rng.Next(Methods.WorldLimit.yMin + 50, Methods.WorldLimit.yMax - 50);
                spawnRect = new Rectangle(x, y, 64, 64);
            }
            while (screenBounds.IntersectsWith(spawnRect));

            enemies.Add(new Enemy(x, y, enemySpritePath));
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.Clear(Color.White);

        if (!started)
        {
            g.DrawImage(Menu, 0, 0);
            start.IsHovered(mouseRect);
            quit.IsHovered(mouseRect);

            if (start.IsClicked(mouseRect, mouseClicked))
                started = true;

            if (quit.IsClicked(mouseRect, mouseClicked))
                Application.Exit();

            start.Draw(g, sB.X, sB.Y);
            quit.Draw(g, qB.X, qB.Y);
            return;
        }

        if (Methods.Scene == "Outside")
        {
            g.DrawImage(Grass1, -camera.X - 1024, -camera.Y - 640);

            if (!wasOut)
            {
                player.OutHouse();
                wasOut = true;
            }
            //Polymorphism
            foreach (IDrawEntities i in inside)
                i.Draw(g, camera.X, camera.Y);

            foreach (var enemy in enemies)
                enemy.Draw(g, camera.X, camera.Y);

            //foreach (var solid in outSolidObjects)
            //{
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), solid.X - camera.X, solid.Y - camera.Y, solid.Width, solid.Height);
            //}

            if (player.GetPlayerRect().IntersectsWith(door))
            {
                Methods.Scene = "House";
                wasOut = false;
                enemies.Clear();
            }
        }

        if (Methods.Scene == "House")
        {
            if (!wasHouse)
            {
                player.ResetPosition();
                wasHouse = true;
            }

            player.Draw(g, camera.X, camera.Y);

            //foreach (var solid in inSolidObjects)
            //{
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), solid.X - camera.X, solid.Y - camera.Y, solid.Width, solid.Height);
            //}

            g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), goOut);
            if (player.GetPlayerRect().IntersectsWith(goOut))
            {
                Methods.Scene = "Outside";
                wasHouse = false;
                SpawnEnemies();
            }
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        mouseRect.Location = e.Location;
        this.Text = $"Mouse Position: {e.X}, {e.Y}";
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        mouseClicked = true;

        if (!started) return;

        Point worldClick = new Point(e.X + camera.X, e.Y + camera.Y);
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDead && enemy.GetHitbox().Contains(worldClick))
            {
                enemy.TakeDamage(1);
                break;
            }
        }
    }
}

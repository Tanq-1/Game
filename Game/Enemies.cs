using System;
using System.Drawing;

namespace Game
{
    public class Enemy : IDrawEntities
    {
        private Bitmap spriteSheet;
        private Rectangle position;
        private int scale = 1;

        private int frameWidth = 64;
        private int frameHeight = 64;
        private int currentFrame = 0;
        private int frameCount = 6;
        private int frameTimer = 0;
        private int frameDelay = 10;

        private int moveCooldown = 30;
        private int moveTimer = 0;

        private Random rand = new Random();

        private int speed = 10;
        private int health = 3;
        public bool IsDead => health <= 0;

        public Enemy(int x, int y, string spritePath)
        {
            spriteSheet = Methods.LoadTexture(spritePath);
            position = new Rectangle(x, y, frameWidth, frameHeight);
        }

        public void Update()
        {
            if (IsDead) return;

            // Handle animation
            frameTimer++;
            if (frameTimer >= frameDelay)
            {
                frameTimer = 0;
                currentFrame = (currentFrame + 1) % frameCount;
            }

            moveTimer++;
            if (moveTimer >= moveCooldown)
            {
                moveTimer = 0;
                int dir = rand.Next(4);// 0=up, 1=down, 2=left, 3=right

                switch (dir)
                {
                    case 0: position.Y -= speed; break;
                    case 1: position.Y += speed; break;
                    case 2: position.X -= speed; break;
                    case 3: position.X += speed; break;
                }
            }
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X, position.Y, frameWidth, frameHeight);
        }

        public void Draw(Graphics g, int cameraX, int cameraY)
        {
            if (IsDead) return;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            Rectangle src = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            Rectangle dest = new Rectangle(
                position.X - cameraX,
                position.Y - cameraY,
                frameWidth * scale,
                frameHeight * scale
            );

            g.DrawImage(spriteSheet, dest, src, GraphicsUnit.Pixel);
        }
    }
}

using System;
using System.Numerics;
using Raylib_cs;

namespace GameTest
{
    public class NPC
    {
        public Vector2 Position;
        private Texture2D texture;
        private Rectangle sourceRec;
        private float scale = 2f;

        private int frameWidth = 32;
        private int frameHeight = 32;
        private int maxFrames = 6;

        private int currentFrame = 0;
        private int frameCounter = 0;
        private int frameSpeed = 8;

        private Vector2 direction = Vector2.Zero;
        private float speed = 1.5f;
        private float directionChangeTimer = 0f;
        private float directionChangeInterval = 2f; // seconds
        private Random rng = new Random();

        public NPC(float x, float y, string spritePath)
        {
            Position = new Vector2(x, y);
            texture = Raylib.LoadTexture(spritePath);
            sourceRec = new Rectangle(0, 0, frameWidth, frameHeight);
        }

        public void Update()
        {
            // --- Animation ---
            frameCounter++;
            if (frameCounter >= 60 / frameSpeed)
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % maxFrames;
                sourceRec.X = currentFrame * frameWidth;
            }

            // --- Movement ---
            directionChangeTimer += Raylib.GetFrameTime();
            if (directionChangeTimer >= directionChangeInterval)
            {
                directionChangeTimer = 0f;
                direction = new Vector2(rng.Next(-1, 2), rng.Next(-1, 2));
            }
            Position += direction * speed;
            Position.X = Math.Clamp(Position.X, 0, 800);
            Position.Y = Math.Clamp(Position.Y, 0, 600);
        }

        public void Draw()
        {
            Rectangle destRec = new Rectangle(Position.X, Position.Y, frameWidth * scale, frameHeight * scale);
            Raylib.DrawTexturePro(texture, sourceRec, destRec, Vector2.Zero, 0f, Raylib_cs.Color.White);
        }

        public void Unload()
        {
            Raylib.UnloadTexture(texture);
        }
    }
}

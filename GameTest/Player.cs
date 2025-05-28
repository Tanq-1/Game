using System.Numerics;
using Raylib_cs;

namespace GameTest
{
    public class Player : ITextureLoading
    {
        public Vector2 Position;
        public float Speed = 4f;
        public float Scale = 2f;
        public int Size => 32 * (int)Scale;
        private bool isPlayerLoaded = true;

        public Texture2D Idle;
        public Texture2D Front;
        public Texture2D Back;
        public Texture2D Left;
        public Texture2D Right;
        public Texture2D Sprite;

        private Rectangle sourceRec;
        private int frameWidth;
        private int frameHeight;
        private int currentFrame = 0;
        private int frameCounter = 0;
        private int frameSpeed = 4;

        public Player(float startX, float startY)
        {
            Position = new Vector2(startX, startY);

            Idle = Raylib.LoadTexture("Assets/Player/Player_Idle.png");
            Raylib.SetTextureFilter(Idle, TextureFilter.Point);
            Front = Raylib.LoadTexture("Assets/Player/Player_Front.png");
            Raylib.SetTextureFilter(Front, TextureFilter.Point);
            Back = Raylib.LoadTexture("Assets/Player/Player_Back.png");
            Raylib.SetTextureFilter(Back, TextureFilter.Point);
            Left = Raylib.LoadTexture("Assets/Player/Player_Left.png");
            Raylib.SetTextureFilter(Left, TextureFilter.Point);
            Right = Raylib.LoadTexture("Assets/Player/Player_Right.png");
            Raylib.SetTextureFilter(Right, TextureFilter.Point);
            Sprite = Idle;

            frameWidth = Idle.Width / 4;
            frameHeight = Idle.Height;
            sourceRec = new Rectangle(0, 0, frameWidth, frameHeight);

        }

        public void Update()
        {
            bool isMoving = false;

            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
            {
                Position.Y -= Speed;
                isMoving = true;
                Sprite = Back;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
            {
                Position.Y += Speed;
                isMoving = true;
                Sprite = Front;
            }
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
            {
                Position.X -= Speed;
                isMoving = true;
                Sprite = Left;
            }
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
            {
                Position.X += Speed;
                isMoving = true;
                Sprite = Right;
            }

            if (!isMoving) Sprite = Idle;

            // The animations have been moved here
            frameCounter++;
            if (frameSpeed > 0 && frameCounter >= 60 / frameSpeed)
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % 4;
                sourceRec.X = currentFrame * frameWidth;
            }
        }

        public void Draw()
        {
            Rectangle destRec = new Rectangle(Position.X, Position.Y, sourceRec.Width * Scale, sourceRec.Height * Scale);
            Raylib.DrawTexturePro(Sprite, sourceRec, destRec, Vector2.Zero, 0f, Color.White);
        }
        // The functions Unload and Load were added just for good practice
        // This Unloads the texture from memory when not being used (when on main menu)
        // Encapsulation :) 
        private void doUnload()
        {
            Raylib.UnloadTexture(Idle);
            Raylib.UnloadTexture(Front);
            Raylib.UnloadTexture(Back);
            Raylib.UnloadTexture(Left);
            Raylib.UnloadTexture(Right);
        }

        private void doLoad()
        {
            Idle = Raylib.LoadTexture("Assets/Player/Player_Idle.png");
            Raylib.SetTextureFilter(Idle, TextureFilter.Point);
            Front = Raylib.LoadTexture("Assets/Player/Player_Front.png");
            Raylib.SetTextureFilter(Front, TextureFilter.Point);
            Back = Raylib.LoadTexture("Assets/Player/Player_Back.png");
            Raylib.SetTextureFilter(Back, TextureFilter.Point);
            Left = Raylib.LoadTexture("Assets/Player/Player_Left.png");
            Raylib.SetTextureFilter(Left, TextureFilter.Point);
            Right = Raylib.LoadTexture("Assets/Player/Player_Right.png");
            Raylib.SetTextureFilter(Right, TextureFilter.Point);
        }
        public void Unload()
        {
            if (isPlayerLoaded == true)
            {
                doUnload();
                isPlayerLoaded = false;
            }
        }
        public void Load()
        {
            if (isPlayerLoaded == false)
            {
                doLoad();
                isPlayerLoaded = true;
            }
        }
    }

}
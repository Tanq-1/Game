using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class Player : ITickEntities, IDrawEntities
    {
        List<Rectangle> solidRects_outside;
        List<Rectangle> solidRects_inside;

        private Bitmap _idle;
        private Bitmap _up;
        private Bitmap _down;
        private Bitmap _left;
        private Bitmap _right;
        public Bitmap current;

        private int _frameWidth = 32;
        private int _frameHeight = 32;
        private int _frameCount = 4;
        private int _currentFrame = 0;
        private int _frameDelay = 10;
        private int _frameTimer = 0;

        (int xMin, int xMax, int yMin, int yMax) _worldLimit;
        public int _posX { get; private set; } = 512;
        public int _posY { get; private set; } = 320;
        private int _moveSpeed = 5;
        public int scale = 2;

        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveUp = false;
        private bool moveDown = false;

        public Player((int xMin, int xMax, int yMin, int yMax) worldLimit, List<Rectangle> outSolidRects, List <Rectangle> inSolidRects)
        {
            _idle = Methods.LoadTexture("Assets/Player/ThePlayer_Idle.png");
            _up = Methods.LoadTexture("Assets/Player/ThePlayer_Up.png");
            _down = Methods.LoadTexture("Assets/Player/ThePlayer_Down.png");
            _left = Methods.LoadTexture("Assets/Player/ThePlayer_Left.png");
            _right = Methods.LoadTexture("Assets/Player/ThePlayer_Right.png");
            current = _idle;
            _worldLimit = worldLimit;
            this.solidRects_outside = outSolidRects;
            this.solidRects_inside = inSolidRects;
        }

        public void OnKeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.A:
                    moveLeft = true;
                    break;
                case Keys.Right:
                case Keys.D:
                    moveRight = true;
                    break;
                case Keys.Up:
                case Keys.W:
                    moveUp = true;
                    break;
                case Keys.Down:
                case Keys.S:
                    moveDown = true;
                    break;
            }
        }

        public void OnKeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.A:
                    moveLeft = false;
                    break;
                case Keys.Right:
                case Keys.D:
                    moveRight = false;
                    break;
                case Keys.Up:
                case Keys.W:
                    moveUp = false;
                    break;
                case Keys.Down:
                case Keys.S:
                    moveDown = false;
                    break;
            }
        }

        public void Update()
        {
            if (Methods.Scene == "Outside")
            {
                _worldLimit = Methods.WorldLimit;
            }
            else
            {
                _worldLimit = Methods.HouseLimit;
            }
            HandleInput();
            ResolveCollisions();

            _frameTimer++;
            if (_frameTimer >= _frameDelay)
            {
                _frameTimer = 0;
                _currentFrame = (_currentFrame + 1) % _frameCount;
            }
        }

        private void HandleInput()
        {
            current = _idle;

            if (moveLeft)
            {
                _posX -= _moveSpeed;
                current = _left;
            }
            if (moveRight)
            {
                _posX += _moveSpeed;
                current = _right;
            }
            if (moveUp)
            {
                _posY -= _moveSpeed;
                current = _up;
            }
            if (moveDown)
            {
                _posY += _moveSpeed;
                current = _down;
            }

            _posX = Math.Clamp(_posX, _worldLimit.xMin + _frameWidth / 2, _worldLimit.xMax - _frameWidth / 2);
            _posY = Math.Clamp(_posY, _worldLimit.yMin + _frameHeight / 2, _worldLimit.yMax - _frameHeight / 2);
        }

        private void ResolveCollisions()
        {
            Rectangle playerRect = GetPlayerRect();
            List<Rectangle> CurrentSolids;
            if (Methods.Scene == "Outside")
            {
                CurrentSolids = this.solidRects_outside;
            }
            else
            {
                CurrentSolids = this.solidRects_inside;
            }

                foreach (var solid in CurrentSolids)
                {
                    if (playerRect.IntersectsWith(solid))
                    {
                        Rectangle intersection = Rectangle.Intersect(playerRect, solid);

                        if (intersection.Width < intersection.Height)
                        {
                            if (playerRect.X < solid.X)
                                _posX -= intersection.Width;
                            else
                                _posX += intersection.Width;
                        }
                        else
                        {
                            if (playerRect.Y < solid.Y)
                                _posY -= intersection.Height;
                            else
                                _posY += intersection.Height;
                        }
                        playerRect = GetPlayerRect();
                    }
                }
        }

        public Rectangle GetPlayerRect()
        {
            return new Rectangle(
                _posX - _frameWidth * scale / 2,
                _posY - _frameHeight * scale / 2,
                _frameWidth * scale,
                _frameHeight * scale
            );
        }

        public void Draw(Graphics g, int cameraX, int cameraY)
        {
            int drawX = _posX - cameraX - _frameWidth * scale / 2;
            int drawY = _posY - cameraY - _frameHeight * scale / 2;
            int drawW = _frameWidth * scale;
            int drawH = _frameHeight * scale;

            Rectangle sourceRect = new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, _frameHeight);
            Rectangle destRect = new Rectangle(drawX, drawY, drawW, drawH);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //g.FillRectangle(new SolidBrush(Color.FromArgb(135, 184, 105, 91)), destRect);
            g.DrawImage(current, destRect, sourceRect, GraphicsUnit.Pixel);
        }

        public void ResetPosition()
        {
            _posX = 512;
            _posY = 320;
        }
        public void OutHouse()
        {
            _posX = 190;
            _posY = 365;
        }
    }
}

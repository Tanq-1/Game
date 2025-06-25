using System;
using System.Drawing;

namespace Game
{
    interface ITickEntities
    {
        public abstract void Update();
    }
    interface IDrawEntities
    {
        public abstract void Draw(Graphics g, int X, int Y);
    }
    public abstract class UIButton
    {
        public Rectangle rect;
        protected abstract void onHover();
        protected abstract void onClick();
    }
}

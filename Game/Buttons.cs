using System;
using System.Windows.Forms;
using System.Drawing;


namespace Game
{
    public class StartButton : UIButton, IDrawEntities
    {
        private Color hoverColor = Color.FromArgb(125, 125, 245, 245);
        public StartButton(Rectangle rect)
        {
            base.rect = rect;
        }
        public bool IsHovered(Rectangle mousePos)
        {
            if (mousePos.IntersectsWith(rect))
            {
                onHover();
                return true;
            }
            hoverColor = Color.FromArgb(0, 0, 0, 0);
            return false;
        }
        public bool IsClicked(Rectangle mousePos, bool mouseClicked)
        {
            if (IsHovered(mousePos))
            {
                if (mouseClicked)
                {
                    onClick();
                    return true;
                }
            }
            return false;
        }
        protected override void onHover()
        {
            hoverColor = Color.FromArgb(125, 125, 245, 245);
        }
        protected override void onClick()
        {
            hoverColor = Color.FromArgb(0, 0, 0, 0);
        }
        public void Draw(Graphics g, int x, int y)
        {
            g.FillRectangle(new SolidBrush(hoverColor), rect);
        }
    }
}

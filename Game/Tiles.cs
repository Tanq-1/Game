using System;
using System.Drawing;
using System.Windows.Forms;
namespace Game
{
    public class Tile : IDrawEntities
    {
        public Rectangle souceRect;
        public Rectangle destRect;
        public Bitmap image;
        private bool showHitbox = false;
        public int scale;
        public Tile(Rectangle souceRect, Rectangle destRect, string path, int scale)
        {
            this.souceRect = souceRect;
            this.destRect = destRect;
            this.image = new Bitmap(path);
            this.scale = scale;
        }
        public void Draw(Graphics g, int x, int y)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle screenRect = new Rectangle(destRect.X - x, destRect.Y - y, destRect.Width*scale, destRect.Height*scale);
            if (showHitbox) g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 255, 0)), screenRect);
            g.DrawImage(image, screenRect, souceRect, GraphicsUnit.Pixel);
        }
    }
}

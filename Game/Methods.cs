using System;
using System.Drawing;

namespace Game
{
    public static class Methods
    {
        public static Bitmap LoadTexture(string path)
        {
            try
            {
                return new Bitmap(path);
            }
            catch (Exception ex)
            {
                return new Bitmap("Assets/Error.png");
            }
        }
        public static string Scene = "Outside";
        public static (int xMin, int xMax, int yMin, int yMax) WorldLimit = (-1024, 2048, -640, 1280);
        public static (int xMin, int xMax, int yMin, int yMax) HouseLimit = (0, 1024, 0, 640);
    }
}

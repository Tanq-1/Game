using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;

namespace GameTest
{
    public enum GameState
    {
        Menu,
        Playing,
        Quit
    }

    public class Menu
    {
        public Rectangle startButton = new Rectangle(300, 200, 200, 50);
        public Rectangle quitButton = new Rectangle(300, 260, 200, 50);

        public void Draw()
        {
            DrawButton(startButton, "Start Game");
            DrawButton(quitButton, "Quit Game");
        }

        public GameState Update()
        {
            Vector2 mousePos = Raylib.GetMousePosition();

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (Raylib.CheckCollisionPointRec(mousePos, startButton))
                    return GameState.Playing;

                if (Raylib.CheckCollisionPointRec(mousePos, quitButton))
                    return GameState.Quit;
            }

            return GameState.Menu;
        }

        private void DrawButton(Rectangle rect, string text)
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            bool hovered = Raylib.CheckCollisionPointRec(mousePos, rect);

            Raylib.DrawRectangleRec(rect, hovered ? LightGray : Gray);
            Raylib.DrawRectangleLines((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height, Black);

            int fontSize = 20;
            int textWidth = Raylib.MeasureText(text, fontSize);
            int textX = (int)(rect.X + (rect.Width - textWidth) / 2);
            int textY = (int)(rect.Y + (rect.Height - fontSize) / 2);

            Raylib.DrawText(text, textX, textY, fontSize, Black);
        }
    }
}

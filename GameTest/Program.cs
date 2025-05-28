using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Color;
using GameTest;
class Program
{
    static void Main()
    {
        int screenWidth = 800;
        int screenHeight = 600;

        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(screenWidth, screenHeight, "Sleeping in the Dream");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        GameState state = GameState.Menu;
        Player player = new Player(400, 300);
        NPC Dropsy = new NPC(0, 0, "Assets/NPCs/Dropsy.png");
        Menu menu = new Menu();

        Camera2D camera = new Camera2D
        {
            Zoom = 1f,
            Rotation = 0f
        };
        Sound click = Raylib.LoadSound("Assets/Audio/SFX/click.wav");

        while (!Raylib.WindowShouldClose() && state != GameState.Quit)
        {
            screenWidth = Raylib.GetScreenWidth();
            screenHeight = Raylib.GetScreenHeight();

            Rectangle backButton = new Rectangle(screenWidth - 120, 10, 110, 40);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(RayWhite);

            if (state == GameState.Menu)
            {
                player.Unload();

                menu.startButton.X = screenWidth / 2f - 100;
                menu.startButton.Y = screenHeight / 2f - 100;
                menu.quitButton.X = screenWidth / 2f - 100;
                menu.quitButton.Y = screenHeight / 2f + 40;
                menu.Draw();
                state = menu.Update();
            }
            else if (state == GameState.Playing)
            {
                player.Load();
                Dropsy.Update();
                player.Update();

                camera.Offset = new Vector2(screenWidth / 2f, screenHeight / 2f);
                camera.Target = player.Position + new Vector2(player.Size / 2f, player.Size / 2f);

                Raylib.DrawText("Use WASD or arrow keys to move", 10, 10, 20, DarkGray);

                // Always add UI (fix to screen) before the Camera Functions alright
                DrawButton(backButton, "Main Menu");
                Vector2 mousePos = Raylib.GetMousePosition();
                if (Raylib.IsMouseButtonPressed(MouseButton.Left) &&
                    Raylib.CheckCollisionPointRec(mousePos, backButton))
                {
                    state = GameState.Menu;
                    Raylib.PlaySound(click);
                }

                Raylib.BeginMode2D(camera);

                Raylib.DrawText("0---------------", 0, 0, 100, SkyBlue);
                Raylib.DrawText("\n|\n|\n|\n|\n|", 0, 0, 100, SkyBlue);
                Raylib.DrawText("0\n |\n |\n |\n |\n |", 800, 0, 100, SkyBlue);
                Raylib.DrawText("0---------------", 0, 600, 100, SkyBlue);
                Raylib.DrawText("0", 800, 600, 100, SkyBlue);

                player.Draw();
                Dropsy.Draw();
                Raylib.EndMode2D();

                if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                    state = GameState.Menu;
            }

            Raylib.EndDrawing();
        }
        player.Unload();
        Raylib.CloseWindow();
    }

    static void DrawButton(Rectangle rect, string text)
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

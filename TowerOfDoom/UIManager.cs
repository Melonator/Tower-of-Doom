using Microsoft.Xna.Framework;
using SadConsole;
using actor;
using TowerOfDoom;
using System;

namespace UI
{
    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole
    {
        public ScrollingConsole MapConsole;
        public Window MapWindow;
        public MessageLogWindow MessageLog;
        public SadConsole.FontMaster fontMaster = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json");
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = SadConsole.Global.CurrentScreen;
        }
        public void Init()
        {
            CreateConsoles();
            CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "TOWER OF DOOM");
            MessageLog = new MessageLogWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "");

            Children.Add(MessageLog);

            MessageLog.Show();
            MessageLog.Position = new Point(0, GameLoop.GameHeight / 2);
            CenterOnActor(GameLoop.World.Player);


        }
        public void CreateConsoles()
        {
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, normalSizedFont, new Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), GameLoop.World.CurrentMap.Tiles);
        }

        public void CenterOnActor(Actor actor)
        {
            MapConsole.CenterViewPortOnPoint(actor.Position);

        }

        public override void Update(TimeSpan timeElapsed)
        {
            CheckKeyboard();
            base.Update(timeElapsed);
        }

        private void CheckKeyboard()
        {
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                GameLoop.World.Player.MoveBy(new Point(0, -1));
                CenterOnActor(GameLoop.World.Player);

            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                GameLoop.World.Player.MoveBy(new Point(0, 1));
                CenterOnActor(GameLoop.World.Player);

            }

            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                GameLoop.World.Player.MoveBy(new Point(-1, 0));
                CenterOnActor(GameLoop.World.Player);

            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                GameLoop.World.Player.MoveBy(new Point(1, 0));
                CenterOnActor(GameLoop.World.Player);

            }
        }

        public void CreateMapWindow(int width, int height, string title)
        {

            MapWindow = new Window(width, height, SadConsole.Global.FontEmbedded);
            MapWindow.CanDrag = false;
            SadConsole.Themes.WindowTheme windowTheme = new SadConsole.Themes.WindowTheme();
            SadConsole.Themes.Library.Default.WindowTheme = windowTheme;
            SadConsole.Themes.Library.Default.Colors.TitleText = Color.White;
            SadConsole.Themes.Library.Default.Colors.Lines = Color.Black;
            SadConsole.Themes.Library.Default.Colors.ControlHostBack = Color.Transparent;

            int mapConsoleWidth = width;
            int mapConsoleHeight = height;

            var size = new Point(MapWindow.Width, MapWindow.Height).TranslateFont(MapWindow.Font, MapConsole.Font);

            MapConsole.ViewPort = new Rectangle(0, 0, size.X, size.Y);

            MapConsole.Position = new Point(1, 1);

            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);

            MapWindow.Children.Add(MapConsole);

            Children.Add(MapWindow);


            MapConsole.Children.Add(GameLoop.World.Player);

            MapWindow.Show();

        }
    }

}
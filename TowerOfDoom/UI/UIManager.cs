using System;
using Microsoft.Xna.Framework;
using SadConsole;
using TowerOfDoom.Entities;

namespace TowerOfDoom.UI
{
    public class UIManager : ContainerConsole
    {
        public SadConsole.ScrollingConsole MapConsole;
        public MessageLogWindow MessageLog;
        public Window MapWindow;
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public UIManager()
        {
            IsVisible = true;
            IsFocused = true;

            Parent = SadConsole.Global.CurrentScreen;
        }
        public void CreateConsoles()
        {
            MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight, normalSizedFont);
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
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, -1));
                CenterOnActor(GameLoop.World.Player);
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, 1));
                CenterOnActor(GameLoop.World.Player);
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 0));
                CenterOnActor(GameLoop.World.Player);
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 0));
                CenterOnActor(GameLoop.World.Player);
            }
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                GameLoop.CommandManager.UndoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.X))
            {
                GameLoop.CommandManager.RedoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
            }
        }
        public void Init()
        {
            CreateConsoles();
            MessageLog = new MessageLogWindow(GameLoop.GameWidth, GameLoop.GameHeight / 2, "");
            Children.Add(MessageLog);
            MessageLog.Show();
            MessageLog.Position = new Point(0, GameLoop.GameHeight / 2);
            LoadMap(GameLoop.World.CurrentMap);
            CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "RIP AND TEAR");
            UseMouse = true;
            CenterOnActor(GameLoop.World.Player);
        }
        private void SyncMapEntities(Map map)
        {
            MapConsole.Children.Clear();

            foreach (Entity entity in map.Entities.Items)
            {
                MapConsole.Children.Add(entity);
            }
            map.Entities.ItemAdded += OnMapEntityAdded;
            map.Entities.ItemRemoved += OnMapEntityRemoved;
        }

        public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Remove(args.Item);
        }
        public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Add(args.Item);
        }
        private void LoadMap(Map map)
        {
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), map.Tiles);
            SyncMapEntities(map);
        }

        public void CreateMapWindow(int width, int height, string title)
        {
            SadConsole.Themes.WindowTheme windowTheme = new SadConsole.Themes.WindowTheme();
            SadConsole.Themes.Library.Default.WindowTheme = windowTheme;
            SadConsole.Themes.Library.Default.Colors.TitleText = Color.White;
            SadConsole.Themes.Library.Default.Colors.Lines = Color.Black;
            SadConsole.Themes.Library.Default.Colors.ControlHostBack = Color.Transparent;

            MapWindow = new Window(width, height, SadConsole.Global.FontEmbedded);
            MapWindow.CanDrag = true;


            int mapConsoleWidth = width - 2;

            var size = new Point(MapWindow.Width, MapWindow.Height).TranslateFont(MapWindow.Font, MapConsole.Font);

            MapConsole.ViewPort = new Rectangle(0, 0, size.X, size.Y);
            MapConsole.Position = new Point(1, 1);

            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);
            MapWindow.Children.Add(MapConsole);
            Children.Add(MapWindow);
            MapWindow.Show();
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using SadConsole;
using TowerOfDoom.Entities;

namespace TowerOfDoom.UI
{
    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole
    {
        public SadConsole.ScrollingConsole MapConsole;
        public MessageLogWindow MessageLog;
        public Window MapWindow;
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

        // Creates all child consoles to be managed
        public void CreateConsoles()
        {
            // Temporarily create a console with *no* tile data that will later be replaced with map data
            MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight, normalSizedFont);
        }

        // centers the viewport camera on an Actor
        public void CenterOnActor(Actor actor)
        {
            MapConsole.CenterViewPortOnPoint(actor.Position);
        }

        public override void Update(TimeSpan timeElapsed)
        {
            CheckKeyboard();
            base.Update(timeElapsed);
        }

        // Scans the SadConsole's Global KeyboardState and triggers behaviour
        // based on the button pressed.
        private void CheckKeyboard()
        {

            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            // Decrement player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, -1));
                CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Down arrow
            // Increment player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, 1));
                CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Left arrow
            // Decrement player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 0));
                CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Right arrow
            // Increment player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 0));
                CenterOnActor(GameLoop.World.Player);
            }

            // Undo last command: Z
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                GameLoop.CommandManager.UndoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
            }

            // Repeat last command: X
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.X))
            {
                GameLoop.CommandManager.RedoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
            }
        }


        // Initializes all windows and consoles
        public void Init()
        {
            CreateConsoles();

            //Message Log initialization
            MessageLog = new MessageLogWindow(GameLoop.GameWidth, GameLoop.GameHeight / 2, "");
            Children.Add(MessageLog);
            MessageLog.Show();
            MessageLog.Position = new Point(0, GameLoop.GameHeight / 2);

            // Load the map into the MapConsole
            LoadMap(GameLoop.World.CurrentMap);

            // Now that the MapConsole is ready, build the Window
            CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "RIP AND TEAR");
            UseMouse = true;

            // Start the game with the camera focused on the player
            CenterOnActor(GameLoop.World.Player);
        }

        // Adds the entire list of entities found in the
        // World.CurrentMap's Entities SpatialMap to the
        // MapConsole, so they can be seen onscreen
        private void SyncMapEntities(Map map)
        {
            // remove all Entities from the console first
            MapConsole.Children.Clear();

            // Now pull all of the entities into the MapConsole in bulk
            foreach (Entity entity in map.Entities.Items)
            {
                MapConsole.Children.Add(entity);
            }

            // Subscribe to the Entities ItemAdded listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemAdded += OnMapEntityAdded;

            // Subscribe to the Entities ItemRemoved listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemRemoved += OnMapEntityRemoved;
        }

        // Remove an Entity from the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Remove(args.Item);
        }

        // Add an Entity to the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Add(args.Item);
        }

        // Loads a Map into the MapConsole
        private void LoadMap(Map map)
        {
            // First load the map's tiles into the console
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), map.Tiles);

            // Now Sync all of the map's entities
            SyncMapEntities(map);
        }

        // Creates a window that encloses a map console
        // of a specified height and width
        // and displays a centered window title
        // make sure it is added as a child of the UIManager
        // so it is updated and drawn
        public void CreateMapWindow(int width, int height, string title)
        {
            SadConsole.Themes.WindowTheme windowTheme = new SadConsole.Themes.WindowTheme();
            SadConsole.Themes.Library.Default.WindowTheme = windowTheme;
            SadConsole.Themes.Library.Default.Colors.TitleText = Color.White;
            SadConsole.Themes.Library.Default.Colors.Lines = Color.Black;
            SadConsole.Themes.Library.Default.Colors.ControlHostBack = Color.Transparent;

            MapWindow = new Window(width, height, SadConsole.Global.FontEmbedded);
            MapWindow.CanDrag = true;

            //make console short enough to show the window title
            //and borders, and position it away from borders
            int mapConsoleWidth = width - 2;
            int mapConsoleHeight = height - 2;

            // Resize the Map Console's ViewPort to fit inside of the window's borders snugly
            var size = new Point(MapWindow.Width, MapWindow.Height).TranslateFont(MapWindow.Font, MapConsole.Font);

            MapConsole.ViewPort = new Rectangle(0, 0, size.X, size.Y);

            //reposition the MapConsole so it doesnt overlap with the left/top window edges
            MapConsole.Position = new Point(1, 1);

            // Centre the title text at the top of the window
            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);

            //add the map viewer to the window
            MapWindow.Children.Add(MapConsole);

            // The MapWindow becomes a child console of the UIManager
            Children.Add(MapWindow);

            // Without this, the window will never be visible on screen
            MapWindow.Show();
        }
    }
}

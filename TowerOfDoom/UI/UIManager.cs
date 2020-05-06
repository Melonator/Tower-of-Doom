using System;
using Microsoft.Xna.Framework;
using SadConsole;
using Console = SadConsole.Console;
using TowerOfDoom.Entities;
using GoRogue;
using GoRogue.Pathing;
using GoRogue.DiceNotation;

namespace TowerOfDoom.UI
{
    public class UIManager : ContainerConsole
    {
        public SadConsole.ScrollingConsole MapConsole;
        public SadConsole.ScrollingConsole HealthBars;
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
            string taunts = "Taunt Counter " + GameLoop.World.Player.TauntCounter.ToString("0") + " / 4";
            string hp = "Slayer's HP " + GameLoop.World.Player.Health.ToString("00") + " / " + GameLoop.World.Player.MaxHealth.ToString("00");
            HealthBars.Print(50, 9, taunts);
            HealthBars.Print(50, 10, hp);
            CheckKeyboard();
            base.Update(timeElapsed);
        }
        private void CheckKeyboard()
        {
            var mapView = new GoRogue.MapViews.LambdaMapView<bool>(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, pos => GameLoop.World.CurrentMap.IsTileWalkable(new Point(pos.X, pos.Y)));

            var AStar = new GoRogue.Pathing.AStar(mapView, Distance.CHEBYSHEV);

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

            //If the player moved, move the enemy
            if(SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up) ||
               SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down) ||
               SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left) ||
               SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                foreach (Entity monster in GameLoop.World.CurrentMap.Entities.Items)
                {
                    if (monster is Monster)
                    {
                        Path path = AStar.ShortestPath(monster.Position, GameLoop.World.Player.Position);
                        if(path.Length < monster.VisibleRange &&
                           GameLoop.World.CurrentMap.GetEntityAt<Monster>(path.GetStep(0)) == null)
                        {
                            int diceOutcome = Dice.Roll("1d100");
                            if (diceOutcome >= 100 - monster.MoveChance && path.GetStep(0) != GameLoop.World.Player.Position) GameLoop.CommandManager.MoveMonster((Actor)monster, path.GetStep(0));
                            if (path.GetStep(0) == GameLoop.World.Player.Position && !(GameLoop.World.Player.Attacked) && diceOutcome >= 50) GameLoop.CommandManager.MoveMonster((Actor)monster, path.GetStep(0), true);
                        }
                    }
                }
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
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Height, Global.FontDefault, new Microsoft.Xna.Framework.Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), map.Tiles);
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
            var size = new Point(MapWindow.Width, MapWindow.Height).TranslateFont(MapWindow.Font, MapConsole.Font);
            MapWindow.CanDrag = true;
            HealthBars = new SadConsole.ScrollingConsole(80, height, SadConsole.Global.FontEmbedded);
            HealthBars.ViewPort = new Microsoft.Xna.Framework.Rectangle(width , 0, size.X * 2, size.Y);
            HealthBars.Position = new Point(width, 1);
            
            int mapConsoleWidth = width - 2;

            MapConsole.ViewPort = new Microsoft.Xna.Framework.Rectangle(0, 0, size.X, size.Y);
            MapConsole.Position = new Point(1, 1);

            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);
            MapWindow.Children.Add(HealthBars);
            MapWindow.Children.Add(MapConsole);
            Children.Add(MapWindow);
            MapWindow.Show();
           
        }
    }
}

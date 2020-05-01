using SadConsole.Components;
using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    // All game state data is stored in World
    // also creates and processes generators
    // for map creation
    public class World
    {
        // map creation and storage data
        private int _mapWidth = 100;
        private int _mapHeight = 100;
        private TileBase[] _mapTiles;
        private int _maxRooms = 50;
        private int _minRoomSize = 10;
        private int _maxRoomSize = 15;
        public Point Spawn;
        public Map CurrentMap { get; set; }
        // player data
        public Player Player { get; set; }
        public World()
        {
            // Build a map
            CreateMap();
            // create an instance of player
            CreatePlayer();
        }
        private void SetSpawn()
        {
            for (int x = 0; x < _mapTiles.Length; x++)
            {
                for (int y = 0; y < _mapTiles.Length; y++)
                {
                    if (CurrentMap.IsTileWalkable(new Point(x, y)) && x > 30 && y > 30)
                    {
                        Spawn.X = x;
                        Spawn.Y = y;
                        break;
                    }
                }
            }
        }
        private void CreateMap()
        {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            MapGenerator mapGen = new MapGenerator();
            CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);

        }

        public void CreatePlayer()
        {
            var fontMaster = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json");
            var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
            SadConsole.Global.FontDefault = normalSizedFont;
            Player = new Player(Color.Transparent, 1);
            Player.Components.Add(new EntityViewSyncComponent());
            SetSpawn();
            Player.Position = new Point(Spawn.X, Spawn.Y);
            // Add the ViewPort sync Component to the player

        }
    }
}
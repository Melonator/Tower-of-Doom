using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace TowerOfDoom
{
    public class Map
    {
        TileBase[] _tiles;
        private int _width;
        private int _height;

        public TileBase[] Tiles { get { return _tiles; } set { _tiles = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        public Map(int width, int height)
        {
            _width = width;
            _height = height;
            Tiles = new TileBase[width * height];
        }

        public bool IsTileWalkable(Point location)
        {

            if (location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
                return false;

            return !_tiles[location.Y * Width + location.X].IsBlockingMove;
        }
    }

    public class MapGenerator
    {
        public MapGenerator()
        {
        }
        Map _map;
        List<Point> VerticalPaths = new List<Point>();
        List<Point> HorizontalPaths = new List<Point>();
        List<Point> UpperLeft = new List<Point>();
        List<Point> DownLeft = new List<Point>();
        List<Point> UpperRight = new List<Point>();
        List<Point> DownRight = new List<Point>();

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            _map = new Map(mapWidth, mapHeight);
            Random randNum = new Random();
            List<Rectangle> Rooms = new List<Rectangle>();

            for (int i = 0; i < maxRooms; i++)
            {
                int newRoomWidth = randNum.Next(minRoomSize, maxRoomSize);
                int newRoomHeight = randNum.Next(minRoomSize, maxRoomSize);
                int newRoomX = randNum.Next(1, mapWidth - newRoomWidth - 1);
                int newRoomY = randNum.Next(1, mapHeight - newRoomHeight - 1);

                Rectangle newRoom = new Rectangle(newRoomX, newRoomY, newRoomWidth, newRoomHeight);
                bool newRoomIntersects = Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    Rooms.Add(newRoom);
                }
            }
            FloodWalls();
            foreach (Rectangle room in Rooms)
            {
                CreateRoom(room);
            }
            foreach (Rectangle room in Rooms)
            {
                BeautifyFloor(room);
            }
            for (int r = 1; r < Rooms.Count; r++)
            {
                Point previousRoomCenter = Rooms[r - 1].Center;
                Point currentRoomCenter = Rooms[r].Center;
                if (randNum.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, previousRoomCenter.Y);
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, currentRoomCenter.X);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, previousRoomCenter.X);
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, currentRoomCenter.Y);
                }
            }
            //Store the corners
            foreach (Point location in VerticalPaths)
            {
                StoreVerticalCorners(location);
            }
            foreach (Point location in HorizontalPaths)
            {
                StoreHorizontalCorners(location);
            }

            //GenerateCorners
            foreach (Point location in UpperLeft)
            {
                _map.Tiles[location.ToIndex(_map.Width)] = new TileWall(17, true);
            }
            foreach (Point location in DownLeft)
            {
                _map.Tiles[location.ToIndex(_map.Width)] = new TileWall(19, true);
            }
            foreach (Point location in UpperRight)
            {
                _map.Tiles[location.ToIndex(_map.Width)] = new TileWall(16, true);
            }
            foreach (Point location in DownRight)
            {
                _map.Tiles[location.ToIndex(_map.Width)] = new TileWall(18, true);
            }

            //Once corners are generated, make the walls
            foreach (Point location in VerticalPaths)
            {
                GenerateVerticalWalls(location);
            }
            foreach (Point location in HorizontalPaths)
            {
                GenerateHorizontalWalls(location);
            }

            //Generate those pesky one tiles
            foreach (Point location in VerticalPaths)
            {
                GenerateOneTileVertical(location);
            }
            foreach (Point location in HorizontalPaths)
            {
                GenerateOneTileHorizontal(location);
            }
            return _map;
        }

        private void GenerateOneTileVertical(Point location)
        {
            if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
               _map.Tiles[((location.Y) * _map.Width + (location.X + 2))].Name == "Floor" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))].Name == "Wall" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 2))].Name == "Floor")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))] = new TileWall(14);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
               _map.Tiles[((location.Y) * _map.Width + (location.X - 2))].Name == "Floor" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))].Name == "Wall" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 2))].Name == "Floor")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))] = new TileWall(14);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                     _map.Tiles[((location.Y) * _map.Width + (location.X + 2))].Name == "Floor" &&
                     _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                     _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Wall" &&
                     _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 2))].Name == "Floor")
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))] = new TileWall(46);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
               _map.Tiles[((location.Y) * _map.Width + (location.X - 2))].Name == "Floor" &&
               _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
               _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))].Name == "Wall" &&
               _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 2))].Name == "Floor")
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))] = new TileWall(46);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Wall" &&
                 _map.Tiles[((location.Y) * _map.Width + (location.X + 2))].Name == "Floor" &&
                 _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))] = new TileWall(30);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Wall" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X - 2))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))] = new TileWall(30);
            }
        }
        private void GenerateOneTileHorizontal(Point location)
        {
            if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y + 2) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y + 2) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))] = new TileWall(48);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y - 2) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y - 2) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))] = new TileWall(48);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y + 2) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y + 2) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))] = new TileWall(50);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y - 2) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y - 2) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))] = new TileWall(50);
            }
            else if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Wall" &&
                    _map.Tiles[((location.Y + 2) * _map.Width + (location.X))].Name == "Floor" &&
                    _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X))] = new TileWall(49);
            }
            else if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Wall" &&
                    _map.Tiles[((location.Y - 2) * _map.Width + (location.X))].Name == "Floor" &&
                    _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X))] = new TileWall(49);
            }
        }
        private void GenerateVerticalWalls(Point location)
        {
            if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].IsCorner != true &&
               _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].IsCorner != true &&
               _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Wall" &&
               _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))] = new TileWall(13);
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))] = new TileWall(12);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].IsCorner == true &&
                      _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].IsCorner != true &&
                    _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))] = new TileWall(12);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].IsCorner == true &&
                     _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].IsCorner != true &&
                     _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))] = new TileWall(13);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                     _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Wall" &&
                     _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].IsCorner != true)
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))] = new TileWall(13);
            }
            else if (_map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                    _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Wall" &&
                    _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].IsCorner != true)
            {
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))] = new TileWall(12);
            }
        }
        private void GenerateHorizontalWalls(Point location)
        {
            if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].IsCorner == false &&
               _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].IsCorner == false &&
               _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Wall" &&
               _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Wall")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X))] = new TileWall(11);
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X))] = new TileWall(11);
            }
            else if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].IsCorner == true &&
                     _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].IsCorner != true &&
                     _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Wall")
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X))] = new TileWall(11);
            }
            else if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].IsCorner == true &&
                     _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].IsCorner != true &&
                     _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Wall")
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X))] = new TileWall(11);
            }
            else if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                     _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Wall" &&
                     _map.Tiles[((location.Y + 1) * _map.Width + (location.X))].IsCorner != true)
            {
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X))] = new TileWall(11);
            }
            else if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                    _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Wall" &&
                    _map.Tiles[((location.Y - 1) * _map.Width + (location.X))].IsCorner != true)
            {
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X))] = new TileWall(11);
            }
        }
        private void StoreVerticalCorners(Point location)
        {
            if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                //UpperLeft
                Point newCorner = new Point(location.X - 1, location.Y - 1);
                UpperLeft.Add(newCorner);
            }
            if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                //DownLeft
                Point newCorner = new Point(location.X - 1, location.Y + 1);
                DownLeft.Add(newCorner);
            }
            if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                //UpperRight
                Point newCorner = new Point(location.X + 1, location.Y - 1);
                UpperRight.Add(newCorner);
            }
            if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                //DownRight
                Point newCorner = new Point(location.X + 1, location.Y + 1);
                DownRight.Add(newCorner);
            }
        }
        private void StoreHorizontalCorners(Point location)
        {
            if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                //UpperLeft
                Point newCorner = new Point(location.X - 1, location.Y - 1);
                UpperLeft.Add(newCorner);
            }
            if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X - 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))].Name == "Wall")
            {
                //DownLeft
                Point newCorner = new Point(location.X - 1, location.Y + 1);
                DownLeft.Add(newCorner);
            }
            if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                //UpperRight
                Point newCorner = new Point(location.X + 1, location.Y - 1);
                UpperRight.Add(newCorner);
            }
            if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X))].Name == "Floor" &&
                _map.Tiles[((location.Y) * _map.Width + (location.X + 1))].Name == "Floor" &&
                _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))].Name == "Wall")
            {
                //DownRight
                Point newCorner = new Point(location.X + 1, location.Y + 1);
                DownRight.Add(newCorner);
            }
        }
        private void BeautifyFloor(Rectangle room)
        {
            List<Point> perimeter = GetBorderCellLocations(room);
            foreach (Point location in perimeter)
            {
                FinalizeFloor(location);
            }
        }
        private void FinalizeFloor(Point location)
        {
            if (location.X > 1 && location.Y > 1)
            {
                if (CheckType(location) == 1)
                {
                    //UL CORNER
                    _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))] = new TileFloor(20);
                }
                else if (CheckType(location) == 2)
                {
                    //L MIDDLE
                    _map.Tiles[(location.Y * _map.Width + (location.X + 1))] = new TileFloor(21);
                }
                else if (CheckType(location) == 3)
                {
                    //DL CORNER 
                    _map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))] = new TileFloor(22);
                }
                else if (CheckType(location) == 4)
                {
                    //U MIDDLE
                    _map.Tiles[((location.Y + 1) * _map.Width + location.X)] = new TileFloor(23);
                }
                else if (CheckType(location) == 5)
                {
                    //UR 
                    _map.Tiles[((location.Y + 1) * _map.Width + location.X - 1)] = new TileFloor(24);
                }
                else if (CheckType(location) == 6)
                {
                    //R MIDDLE
                    _map.Tiles[(location.Y * _map.Width + (location.X - 2))] = new TileFloor(25);
                }
                else if (CheckType(location) == 7)
                {
                    _map.Tiles[((location.Y - 2) * _map.Width + (location.X - 2))] = new TileFloor(26);
                }
                else if (CheckType(location) == 8)
                {
                    _map.Tiles[((location.Y - 2) * _map.Width + location.X)] = new TileFloor(27);
                }
            }
        }
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                Point location = new Point(x, yPosition);
                _map.Tiles[(location.Y * _map.Width + location.X)] = new TileFloor(35);
                HorizontalPaths.Add(location);
            }
        }
        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                Point location = new Point(xPosition, y);
                _map.Tiles[(location.Y * _map.Width + location.X)] = new TileFloor(34);
                VerticalPaths.Add(location);
            }
        }
        private void CreateRoom(Rectangle room)
        {

            for (int x = room.Left + 1; x < room.Right - 1; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom - 1; y++)
                {
                    CreateFloor(new Point(x, y));

                }
            }
            List<Point> perimeter = GetBorderCellLocations(room);
            foreach (Point location in perimeter)
            {
                CreateWall(location);
            }
        }
        private void CreateFloor(Point location)
        {
            _map.Tiles[location.ToIndex(_map.Width)] = new TileFloor(28);
        }
        private int CheckType(Point location)
        {
            if (_map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))].Name == "Floor" &&
                      _map.Tiles[(location.Y * _map.Width + location.X + 1)].Name == "Wall" &&
                      _map.Tiles[((location.Y + 1) * _map.Width + location.X)].Name == "Wall")
            {
                //UL
                return 1;
            }
            else if (_map.Tiles[(location.Y * _map.Width + location.X + 1)].Name == "Floor" &&
                    _map.Tiles[((location.Y - 1) * _map.Width + location.X + 1)].Name == "Floor" &&
                      _map.Tiles[((location.Y + 1) * _map.Width + location.X + 1)].Name == "Floor")
            {
                //L MIDDLE
                return 2;
            }
            else if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X + 1))].Name == "Floor" &&
                     _map.Tiles[((location.Y - 1) * _map.Width + location.X)].Name == "Wall" &&
                      _map.Tiles[(location.Y * _map.Width + location.X + 1)].Name == "Wall")
            {
                //DL 
                return 3;
            }
            else if (_map.Tiles[((location.Y + 1) * _map.Width + location.X)].Name == "Floor" &&
                    _map.Tiles[((location.Y + 1) * _map.Width + (location.X + 1))].Name == "Floor" &&
                    _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 1))].Name == "Floor")
            {
                //NORMAL WALL ABOVE
                return 4;
            }
            else if (_map.Tiles[((location.Y + 1) * _map.Width + location.X - 1)].Name == "Floor" &&
                   _map.Tiles[((location.Y + 1) * _map.Width + location.X)].Name == "Wall" &&
                    _map.Tiles[(location.Y * _map.Width + location.X - 1)].Name == "Wall")
            {
                //UR
                return 5;
            }
            else if (_map.Tiles[(location.Y * _map.Width + (location.X - 2))].Name == "Floor" &&
                    _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 2))].Name == "Floor" &&
                      _map.Tiles[((location.Y + 1) * _map.Width + (location.X - 2))].Name == "Floor")
            {
                //R MIDDLE
                return 6;
            }
            else if (_map.Tiles[((location.Y - 1) * _map.Width + (location.X - 1))].Name == "Wall" &&
                 _map.Tiles[((location.Y - 2) * _map.Width + (location.X - 1))].Name == "Wall" &&
                  _map.Tiles[((location.Y - 1) * _map.Width + (location.X - 2))].Name == "Wall" &&
                  _map.Tiles[((location.Y - 2) * _map.Width + (location.X - 2))].Name == "Floor")
            {
                //DR 
                return 7;
            }
            else if (_map.Tiles[((location.Y - 2) * _map.Width + location.X)].Name == "Floor" &&
                    _map.Tiles[((location.Y - 2) * _map.Width + location.X - 1)].Name == "Floor" &&
                    _map.Tiles[((location.Y - 2) * _map.Width + location.X + 1)].Name == "Floor")
            {
                //NORMAL WALL BELOW
                return 8;
            }
            return 0;
        }
        private void CreateWall(Point location)
        {
            if (location.Y > 1 && location.X > 1)
            {
                if (CheckType(location) == 1)
                {
                    //UL
                    _map.Tiles[(location.Y * _map.Width + location.X)] = new TileWall(12);
                    _map.Tiles[((location.Y + 1) * _map.Width + location.X)] = new TileWall(12);
                    _map.Tiles[(location.Y * _map.Width + location.X + 1)] = new TileWall(11);
                }
                else if (CheckType(location) == 2)
                {
                    //L MIDDLE
                    _map.Tiles[(location.Y * _map.Width + location.X)] = new TileWall(12);
                }
                else if (CheckType(location) == 3)
                {
                    //DL 
                    _map.Tiles[(location.Y * _map.Width + location.X)] = new TileWall(36);
                    _map.Tiles[(location.Y * _map.Width + location.X + 1)] = new TileWall(11);
                    _map.Tiles[((location.Y - 1) * _map.Width + location.X)] = new TileWall(12);
                }
                else if (CheckType(location) == 4)
                {
                    //NORMAL WALL
                    _map.Tiles[(location.Y * _map.Width + location.X)] = new TileWall(11);
                }
                else if (CheckType(location) == 5)
                {
                    //UR
                    _map.Tiles[(location.Y * _map.Width + location.X)] = new TileWall(13);
                    _map.Tiles[((location.Y + 1) * _map.Width + location.X)] = new TileWall(13);
                    _map.Tiles[(location.Y * _map.Width + location.X - 1)] = new TileWall(11);
                }
                else if (CheckType(location) == 6)
                {
                    //R MIDDLE
                    _map.Tiles[(location.Y * _map.Width + location.X - 1)] = new TileWall(13);
                }
                else if (CheckType(location) == 7)
                {
                    //DR 
                    _map.Tiles[((location.Y - 1) * _map.Width + location.X - 1)] = new TileWall(37);
                    _map.Tiles[((location.Y - 2) * _map.Width + location.X - 1)] = new TileWall(13);
                    _map.Tiles[((location.Y - 1) * _map.Width + location.X - 2)] = new TileWall(11);
                }
                else if (CheckType(location) == 8)
                {
                    _map.Tiles[((location.Y - 1) * _map.Width + location.X)] = new TileWall(11);

                }
            }
        }
        private void FloodWalls()
        {
            for (int i = 0; i < _map.Tiles.Length; i++)
            {
                _map.Tiles[i] = new TileWall(0);
            }
        }
        private List<Point> GetBorderCellLocations(Rectangle room)
        {

            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            List<Point> borderCells = GetTileLocationsAlongLine(xMin, yMin, xMax, yMin).ToList();
            borderCells.AddRange(GetTileLocationsAlongLine(xMin, yMin, xMin, yMax));
            borderCells.AddRange(GetTileLocationsAlongLine(xMin, yMax, xMax, yMax));
            borderCells.AddRange(GetTileLocationsAlongLine(xMax, yMin, xMax, yMax));

            return borderCells;
        }
        public IEnumerable<Point> GetTileLocationsAlongLine(int xOrigin, int yOrigin, int xDestination, int yDestination)
        {

            xOrigin = ClampX(xOrigin);
            yOrigin = ClampY(yOrigin);
            xDestination = ClampX(xDestination);
            yDestination = ClampY(yDestination);

            int dx = Math.Abs(xDestination - xOrigin);
            int dy = Math.Abs(yDestination - yOrigin);

            int sx = xOrigin < xDestination ? 1 : -1;
            int sy = yOrigin < yDestination ? 1 : -1;
            int err = dx - dy;

            while (true)
            {

                yield return new Point(xOrigin, yOrigin);
                if (xOrigin == xDestination && yOrigin == yDestination)
                {
                    break;
                }
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    xOrigin = xOrigin + sx;
                }
                if (e2 < dx)
                {
                    err = err + dx;
                    yOrigin = yOrigin + sy;
                }
            }
        }
        private int ClampX(int x)
        {
            if (x < 0)
                x = 0;
            else if (x > _map.Width - 1)
                x = _map.Width - 1;
            return x;
        }
        private int ClampY(int y)
        {
            if (y < 0)
                y = 0;
            else if (y > _map.Height - 1)
                y = _map.Height - 1;
            return y;
        }
    }
}


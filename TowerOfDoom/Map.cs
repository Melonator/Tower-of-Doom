using System.Linq;
using Microsoft.Xna.Framework;
using TowerOfDoom.Entities;

namespace TowerOfDoom
{
    // Stores, manipulates and queries Tile data
    public class Map
    {
        TileBase[] _tiles; // contain all tile objects
        private int _width;
        private int _height;
        public TileBase[] Tiles { get { return _tiles; } set { _tiles = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }

        public GoRogue.MultiSpatialMap<Entity> Entities; // Keeps track of all the Entities on the map
        public static GoRogue.IDGenerator IDGenerator = new GoRogue.IDGenerator(); // A static IDGenerator that all Entities can access

        //Build a new map with a specified width and height
        public Map(int width, int height)
        {
            _width = width;
            _height = height;
            Tiles = new TileBase[width * height];
            Entities = new GoRogue.MultiSpatialMap<Entity>();
        }

        // IsTileWalkable checks
        // to see if the actor has tried
        // to walk off the map or into a non-walkable tile
        // Returns true if the tile location is walkable
        // false if tile location is not walkable or is off-map
        public bool IsTileWalkable(Point location)
        {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if (location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
                return false;
            // then return whether the tile is walkable
            return !_tiles[location.Y * Width + location.X].IsBlockingMove;
        }

        // Checking whether a certain type of
        // entity is at a specified location the manager's list of entities
        // and if it exists, return that Entity
        public T GetEntityAt<T>(Point location) where T : Entity
        {
            return Entities.GetItems(location).OfType<T>().FirstOrDefault();
        }

        // Removes an Entity from the MultiSpatialMap
        public void Remove(Entity entity)
        {
            // remove from SpatialMap
            Entities.Remove(entity);

            // Link up the entity's Moved event to a new handler
            entity.Moved -= OnEntityMoved;
        }

        // Adds an Entity to the MultiSpatialMap
        public void Add(Entity entity)
        {
            // add entity to the SpatialMap
            Entities.Add(entity, entity.Position);

            // Link up the entity's Moved event to a new handler
            entity.Moved += OnEntityMoved;
        }

        // When the Entity's .Moved value changes, it triggers this event handler
        // which updates the Entity's current position in the SpatialMap
        private void OnEntityMoved(object sender, Entity.EntityMovedEventArgs args)
        {
            Entities.Move(args.Entity as Entity, args.Entity.Position);
        }
    }
}

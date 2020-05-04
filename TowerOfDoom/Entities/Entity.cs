using Microsoft.Xna.Framework;

namespace TowerOfDoom.Entities
{
    // Extends the SadConsole.Entities.Entity class
    // by adding an ID to it using GoRogue's ID system
    public abstract class Entity : SadConsole.Entities.Entity, GoRogue.IHasID
    {
        public uint ID { get; set; } // stores the entity's unique identification number

        protected Entity(int glyph, int width = 1, int height = 1) : base(width, height)
        {
            Animation.CurrentFrame[0].Background = Color.Transparent;
            Animation.CurrentFrame[0].Glyph = glyph;
            // Create a new unique identifier for this entity
            ID = Map.IDGenerator.UseID();
        }
    }
}

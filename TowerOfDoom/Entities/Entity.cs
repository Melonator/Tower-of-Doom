using Microsoft.Xna.Framework;

namespace TowerOfDoom.Entities
{
    public abstract class Entity : SadConsole.Entities.Entity, GoRogue.IHasID
    {
        public uint ID { get; set; }

        protected Entity(int glyph, int width = 1, int height = 1) : base(width, height)
        {
            Animation.CurrentFrame[0].Background = Color.Transparent;
            Animation.CurrentFrame[0].Glyph = glyph;
            ID = Map.IDGenerator.UseID();
        }
    }
}

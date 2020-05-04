using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    public class TileWall : TileBase
    {
      
        public TileWall(int glyph, bool corner = false, bool blocksMovement = true, bool blocksLOS = true) : base(Color.LightGray, Color.Transparent, glyph, blocksMovement, blocksLOS)
        {
            Name = "Wall";
            IsCorner = corner;
        }
    }
}

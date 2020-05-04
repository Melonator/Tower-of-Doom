using Microsoft.Xna.Framework;

namespace TowerOfDoom.Entities
{
    public abstract class Actor : Entity
    {
        public int Health { get; set; } // current health
        public int MaxHealth { get; set; } // maximum health
        public int Attack { get; set; } // attack strength
        public int AttackChance { get; set; } // percent chance of successful hit
        public int Defense { get; set; } // defensive strength
        public int DefenseChance { get; set; } // percent chance of successfully blocking a hit
        public int Gold { get; set; } // amount of gold carried

        public int TauntChance;
        protected Actor(int glyph, int width=1, int height=1) : base(glyph, width, height)
        {

        }
        public bool MoveBy(Point positionChange)
        {
            // Check the current map if we can move to this new position
            if (GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
            {
                // if there's a monster here,
                // do a bump attack
                Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange);
                if (monster != null)
                {
                    GameLoop.CommandManager.Attack(this, monster);
                    return true;
                }

                Position += positionChange;
                return true;
            }
            else
                return false;
        }

        public bool MoveTo(Point newPosition)
        {
            Position = newPosition;
            return true;
        }
    }
}

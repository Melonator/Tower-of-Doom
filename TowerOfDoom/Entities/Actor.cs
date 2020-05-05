using Microsoft.Xna.Framework;

namespace TowerOfDoom.Entities
{
    public abstract class Actor : Entity
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int AttackChance { get; set; }
        public int Defense { get; set; }
        public int DefenseChance { get; set; }

        public int TauntChance;
        protected Actor(int glyph, int width = 1, int height = 1) : base(glyph, width, height)
        {

        }
        public bool MoveBy(Point positionChange)
        {
            if (GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
            {
                GameLoop.World.Player.Attacked = false;
                Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange);
                Player slayer = GameLoop.World.Player;
                if (monster != null)
                {
                    GameLoop.World.Player.Attacked = true;
                    GameLoop.CommandManager.Attack(slayer, monster);
                    return true;
                }

                Position += positionChange;
                return true;
            }
            else
                return false;
        }
        public bool MoveTo(Point newPosition, bool attack = false)
        {
            Player slayer = GameLoop.World.CurrentMap.GetEntityAt<Player>(newPosition);
            if(slayer != null && attack)
            {
                GameLoop.CommandManager.AttackPlayer(this, slayer);
            }
            if (attack == false) Position = newPosition;
            return true;
        }
    }
}

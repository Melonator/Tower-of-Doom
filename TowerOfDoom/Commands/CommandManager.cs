using TowerOfDoom.Entities;
using Microsoft.Xna.Framework;
using System.Text;
using GoRogue.DiceNotation;

namespace TowerOfDoom.Commands
{

    public class CommandManager
    {
        private Point _lastMoveActorPoint;
        private Actor _lastMoveActor;

        public CommandManager()
        {

        }
        public void Attack(Actor attacker, Actor defender)
        {
            int SlayerDamage = 0;
            int EnemyDamage;
            int DamageDone;
            int SlayerBlock;
            int EnemyBlock;
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();
            if (GameLoop.World.Player.TauntCounter < 4)
            {
                int SlayerMove = ResolveMove(attacker);
                int EnemyMove = ResolveMove(defender);
                if (SlayerMove == 1)
                {
                    SlayerDamage = RollForAttack(attacker, defender);

                    if (EnemyMove == 1)
                    {
                        EnemyDamage = RollForAttack(defender, attacker);
                        ResolveDamage(attacker, EnemyDamage);
                        ResolveDamage(defender, SlayerDamage);
                    }
                    else if (EnemyMove == 2)
                    {
                        EnemyBlock = RollForDefend(defender, SlayerDamage);
                        DamageDone = SlayerDamage - EnemyBlock;
                        ResolveDamage(defender, DamageDone);
                    }
                    else if (EnemyMove == 3)
                    {
                        ResolveDamage(defender, SlayerDamage);
                    }
                }
                else if (SlayerMove == 2)
                {
                    SlayerBlock = RollForDefend(defender, SlayerDamage);
                    if (EnemyMove == 1)
                    {
                        EnemyDamage = RollForAttack(defender, attacker);
                        DamageDone = EnemyDamage - SlayerBlock;
                        ResolveDamage(attacker, DamageDone);
                    }
                    else if (EnemyMove == 2)
                    {
                        GameLoop.UIManager.MessageLog.Add("You both defend");
                    }
                    else if (EnemyMove == 3)
                    {
                        GameLoop.World.Player.TauntCounter -= 1;
                        GameLoop.UIManager.MessageLog.Add($"{defender.Name} taunts successfully!");
                        if (GameLoop.World.Player.TauntCounter <= 0) GameLoop.World.Player.TauntCounter = 0;
                    }
                }
                else if (SlayerMove == 3)
                {
                    if (EnemyMove == 1)
                    {
                        GameLoop.World.Player.TauntCounter++;
                        EnemyDamage = RollForAttack(defender, attacker);
                        ResolveDamage(attacker, EnemyDamage);
                    }
                    else if (EnemyMove == 2)
                    {
                        GameLoop.World.Player.TauntCounter++;
                    }
                    else if (EnemyMove == 3)
                    {
                        GameLoop.UIManager.MessageLog.Add("You both taunt at each other...");
                        GameLoop.World.Player.TauntCounter++;
                    }
                }
            }
            else
            {            
                if (defender.Name != "The Marauder")
                {
                    GameLoop.UIManager.MessageLog.Add($"THE SLAYER RIPS {defender.Name} IN HALF");
                    ResolveDamage(defender, defender.Health);
                    GameLoop.World.Player.Health += 13;
                }
                else
                {
                    ResolveDamage(defender, 8);
                    GameLoop.UIManager.MessageLog.Add($"The Slayer fataly injured the marauder!");
                    GameLoop.UIManager.MessageLog.Add($"The Slayer is partially healed after a glory kill attempt!");
                    GameLoop.World.Player.Health += 10;
                }
                GameLoop.World.Player.TauntCounter = 0;               
                if (GameLoop.World.Player.Health > 45) GameLoop.World.Player.Health = 45;                                                
            }

            GameLoop.UIManager.MessageLog.Add("___________________________");
        }
        public void AttackPlayer(Actor attacker, Actor defender)
        {
            GameLoop.UIManager.MessageLog.Add("___________________________");
            int EnemyHits = RollForAttack(attacker, defender);
            int SlayerBlocks = RollForDefend(defender, EnemyHits);
            int DamageDone = EnemyHits - SlayerBlocks;
            ResolveDamage(defender, DamageDone);
            GameLoop.UIManager.MessageLog.Add("___________________________");
        }
        private static int ResolveMove(Actor entity)
        {
            int diceOutcome = Dice.Roll("1d100");

            if (diceOutcome <= entity.AttackChance)
            {
                GameLoop.UIManager.MessageLog.Add($"{entity.Name} decides to attack");
                return 1;
            }
            else if (diceOutcome <= entity.DefenseChance + entity.AttackChance)
            {
                GameLoop.UIManager.MessageLog.Add($"{entity.Name} decides to block");
                return 2;
            }
            else if (diceOutcome <= entity.TauntChance + entity.DefenseChance + entity.AttackChance)
            {
                GameLoop.UIManager.MessageLog.Add($"{entity.Name} decides to taunt");
                return 3;
            }
            else
            {
                GameLoop.UIManager.MessageLog.Add($"{entity.Name} decides to attack");
                return 1;
            }

        }
        private static int RollForAttack(Actor attacker, Actor defender)
        {
            int hits = 0;
            for (int dice = 0; dice < attacker.Attack; dice++)
            {
                int diceTwoOutcome = Dice.Roll("1d100");
                if (diceTwoOutcome >= 100 - attacker.AttackChance)
                    hits++;
            }

            return hits;
        }
        private static int RollForDefend(Actor defender, int hits)
        {
            int blocks = 0;
            if (hits > 0)
            {
                for (int dice = 0; dice < defender.Defense; dice++)
                {
                    int diceOutcome = Dice.Roll("1d100");
                    if (diceOutcome >= 100 - defender.DefenseChance)
                        blocks++;
                }
            }
            return blocks;
        }
        private static void ResolveDamage(Actor defender, int damage, bool glorykill = false)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;
                GameLoop.UIManager.MessageLog.Add($" {defender.Name} was hit for {damage} damage");
            }
            else
            {
                GameLoop.UIManager.MessageLog.Add($"{defender.Name} avoided all damage!");
            }

            if (defender.Health <= 0)
            {
                ResolveDeath(defender);
            }
        }
        private static void ResolveDeath(Actor defender)
        {
            GameLoop.World.CurrentMap.Remove(defender);

            if (defender is Player)
            {
                GameLoop.UIManager.MessageLog.Add($" {defender.Name} was killed.");
            }
            else if (defender is Monster)
            {
                GameLoop.UIManager.MessageLog.Add($"{defender.Name} died");
            }
        }
        public bool MoveActorBy(Actor actor, Point position)
        {
            _lastMoveActor = actor;
            _lastMoveActorPoint = position;

            return actor.MoveBy(position);
        }

        public bool MoveMonster(Actor actor,  Point position, bool attack = false)
        {
            return actor.MoveTo(position, attack);
        }
        public bool RedoMoveActorBy()
        {
            if (_lastMoveActor != null)
            {
                return _lastMoveActor.MoveBy(_lastMoveActorPoint);
            }
            else
                return false;
        }
        public bool UndoMoveActorBy()
        {
            if (_lastMoveActor != null)
            {
                _lastMoveActorPoint = new Point(-_lastMoveActorPoint.X, -_lastMoveActorPoint.Y);

                if (_lastMoveActor.MoveBy(_lastMoveActorPoint))
                {
                    _lastMoveActorPoint = new Point(0, 0);
                    return true;
                }
                else
                {
                    _lastMoveActorPoint = new Point(0, 0);
                    return false;
                }
            }
            return false;
        }
    }
}

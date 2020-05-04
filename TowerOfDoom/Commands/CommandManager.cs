using TowerOfDoom.Entities;
using Microsoft.Xna.Framework;
using System.Text;
using GoRogue.DiceNotation;

namespace TowerOfDoom.Commands
{
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public class CommandManager
    {

        //stores the actor's last move action
        private Point _lastMoveActorPoint;
        private Actor _lastMoveActor;

        public CommandManager()
        {

        }

        // Executes an attack from an attacking actor
        // on a defending actor, and then describes
        // the outcome of the attack in the Message Log
        public void Attack(Actor attacker, Actor defender)
        {
            int SlayerDamage = 0;
            int EnemyDamage;
            int DamageDone;
            int SlayerBlock;
            int EnemyBlock;
            // Create two messages that describe the outcome
            // of the attack and defense
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            // Count up the amount of attacking damage done
            // and the number of successful blocks
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
                        EnemyBlock = RollForDefend(defender, SlayerDamage, attackMessage);
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
                    SlayerBlock = RollForDefend(defender, SlayerDamage, attackMessage);
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
                GameLoop.UIManager.MessageLog.Add($"THE SLAYER RIPS {defender.Name} IN HALF");
                GameLoop.UIManager.MessageLog.Add($"Plus 18hp, you have {attacker.Health}HP!");
                ResolveDamage(defender, defender.Health);
                GameLoop.World.Player.TauntCounter = 0;
                GameLoop.World.Player.Health += 18;
                if(GameLoop.World.Player.Health > 45)
                {
                    GameLoop.World.Player.Health = 45;
                }
            }

            GameLoop.UIManager.MessageLog.Add("___________________________");
        }

        private static int ResolveMove(Actor entity)
        {
            int diceOutcome = Dice.Roll("1d100");

            if(diceOutcome <= entity.AttackChance)
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
            // Create a string that expresses the attacker and defender's names
            int hits = 0;

             // The attacker's Attack value determines the number of D100 dice rolled
                for (int dice = 0; dice < attacker.Attack; dice++)
                {
                    //Roll a single D100 and add its results to the attack Message
                    int diceTwoOutcome = Dice.Roll("1d100");

                    //Resolve the dicing outcome and register a hit, governed by the
                    //attacker's AttackChance value.
                    if (diceTwoOutcome >= 100 - attacker.AttackChance)
                        hits++;
                }
            
            return hits;
        }

        // Calculates the outcome of a defender's attempt
        // at blocking incoming hits.
        // Modifies a StringBuilder messages that will be displayed
        // in the MessageLog, expressing the number of hits blocked.
        private static int RollForDefend(Actor defender, int hits, StringBuilder attackMessage)
        {
            int blocks = 0;
            if (hits > 0)
            {
                // Create a string that displays the defender's name and outcomes

                //The defender's Defense value determines the number of D100 dice rolled
                for (int dice = 0; dice < defender.Defense; dice++)
                {
                    //Roll a single D100 and add its results to the defense Message
                    int diceOutcome = Dice.Roll("1d100");

                    //Resolve the dicing outcome and register a block, governed by the
                    //attacker's DefenceChance value.
                    if (diceOutcome >= 100 - defender.DefenseChance)
                        blocks++;
                }
            }
            return blocks;
        }

        // Calculates the damage a defender takes after a successful hit
        // and subtracts it from its Health
        // Then displays the outcome in the MessageLog.
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

        // Removes an Actor that has died
        // and displays a message showing
        // the number of Gold dropped.
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

        // Move the actor BY +/- X&Y coordinates
        // returns true if the move was successful
        // and false if unable to move there
        public bool MoveActorBy(Actor actor, Point position)
        {
            // store the actor's last move state
            _lastMoveActor = actor;
            _lastMoveActorPoint = position;

            return actor.MoveBy(position);
        }

        // Redo last actor move
        public bool RedoMoveActorBy()
        {
            // Make sure there is an actor available to redo first!
            if (_lastMoveActor != null)
            {
                return _lastMoveActor.MoveBy(_lastMoveActorPoint);
            }
            else
                return false;
        }

        // Undo last actor move
        // then clear the undo so it cannot be repeated
        public bool UndoMoveActorBy()
        {
            // Make sure there is an actor available to undo first!
            if (_lastMoveActor != null)
            {
                // reverse the directions of the last move
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

using System;
using System.Collections.Generic;

namespace SmallHax.State.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var battle = new Battle()
            {
                Actors = new List<Actor>()
                {
                    new Actor()
                    {
                        Name = "Knight",
                        Team = "Player",
                        MaxHp = 100,
                        Attack = 10
                    },
                    new Actor()
                    {
                        Name = "Archer",
                        Team = "Player",
                        MaxHp = 50,
                        Attack = 20
                    },
                    new Actor()
                    {
                        Name = "Slime A",
                        Team = "AI",
                        MaxHp = 10,
                        Attack = 1
                    },
                    new Actor()
                    {
                        Name = "Slime B",
                        Team = "AI",
                        MaxHp = 15,
                        Attack = 2
                    },
                    new Actor()
                    {
                        Name = "Slime C",
                        Team = "AI",
                        MaxHp = 20,
                        Attack = 3
                    },
                }

            };
            battle.SetState(BattleState.Start);

            while(true)
            {
                battle.Process();
                if (battle.CurrentState == BattleState.Over)
                {
                    break;
                }
            }
        }
    }
}

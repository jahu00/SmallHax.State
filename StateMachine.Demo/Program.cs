using System;
using System.Collections.Generic;

namespace StateMachine.Demo
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
                        Name = "Hero",
                        Team = "Player",
                        MaxHp = 100,
                        Attack = 10
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
            battle.SetState("Start");

            while(true)
            {
                battle.Process();
                if (battle.CurrentState == "Over")
                {
                    break;
                }
            }
        }
    }
}

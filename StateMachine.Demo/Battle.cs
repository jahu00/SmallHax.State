using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine.Demo
{
    public enum BattleState
    {
        Start,
        NextRound,
        NextTurn,
        PlayerTurn,
        AiTurn,
        ActorUsesSkill,
        Won,
        Lost,
        Over
    }

    public class Battle : IObjectWithState<BattleState>
    {
        public Dictionary<BattleState, Type> StateTypes { get; set; } = new Dictionary<BattleState, Type>();

        public IStateScript CurrentStateScript { get; set; }
        public BattleState CurrentState { get; set; }

        public List<Actor> Actors { get; set; }

        public string[] GetTeamNames() => Actors.Select(actor => actor.Team).Distinct().ToArray();
        public Actor[] GetTeam(string name) => Actors.Where(x => x.Team == name).ToArray();

        public bool HasTeamLost(string name) => GetTeam(name).All(actor => actor.IsDead);

        public Actor CurrentActor { get; set; }

        public int Round { get; set; }

        public Battle()
        {
            InitStates();
        }

        public void InitStates()
        {
            this.AddState<StartState>(BattleState.Start);
            this.AddState<NextRoundState>(BattleState.NextRound);
            this.AddState<NextTurnState>(BattleState.NextTurn);
            this.AddState<PlayerTurnState>(BattleState.PlayerTurn);
            this.AddState<AiTurnState>(BattleState.AiTurn);
            this.AddState<ActorUsesSkillState>(BattleState.ActorUsesSkill);
            this.AddState<WonState>(BattleState.Won);
            this.AddState<LostState>(BattleState.Lost);
            this.AddState<OverState>(BattleState.Over);
        }

    }

    internal class StartState : BattleStateScript
    {
        public override void Process()
        {
            Battle.Actors.ForEach(actor => actor.Hp = actor.MaxHp);
            Battle.SetState(BattleState.NextRound);
        }
    }

    public class NextRoundState : BattleStateScript
    {
        public override void Process()
        {
            Battle.Round++;
            Battle.CurrentActor = null;
            Console.WriteLine($"");
            Console.WriteLine($"--------------------------------------------------");
            Console.WriteLine($"Round {Battle.Round} starts");
            Console.WriteLine($"--------------------------------------------------");
            Console.WriteLine($"");
            Console.WriteLine($"Enemy team:");
            var aiTeam = Battle.GetTeam("AI");
            RenderTeam(aiTeam);
            Console.WriteLine($"");
            Console.WriteLine($"Your team:");
            var playerTeam = Battle.GetTeam("Player");
            RenderTeam(playerTeam);
            Console.WriteLine($"");
            Battle.SetState(BattleState.NextTurn);
        }

        private void RenderTeam(IEnumerable<Actor> team)
        {
            foreach(var actor in team)
            {
                RenderActor(actor);
            }
        }

        private void RenderActor(Actor actor)
        {
            Console.WriteLine($"{actor.Name} {actor.Hp}/{actor.MaxHp} HP");
        }
    }

    public class LostState : BattleStateScript
    {
        public override void Process()
        {
            Console.WriteLine("You lost");
            Battle.SetState(BattleState.Over);
        }
    }

    public class WonState : BattleStateScript
    {
        public override void Process()
        {
            Console.WriteLine("You won");
            Battle.SetState(BattleState.Over);
        }
    }

    public class ActorUsesSkillState : BattleStateScript
    {
        private Actor Target { get; set; }
        public override void Initialize(object owner, object paramObj = null)
        {
            base.Initialize(owner, paramObj);
            Target = (Actor)paramObj;
        }

        public override void Process()
        {
            Console.WriteLine($"{Battle.CurrentActor.Name} attacks {Target.Name}");
            var damage = Battle.CurrentActor.Attack;
            Console.WriteLine($"{Target.Name} takes {damage} damage");
            Target.Hp -= damage;
            if (Target.Hp < 0)
            {
                Target.Hp = 0;
            }
            if (Target.IsDead)
            {
                Console.WriteLine($"{Target.Name} dies");
            }
            Console.WriteLine("");
            Battle.SetState(BattleState.NextTurn);
        }
    }

    public class NextTurnState : BattleStateScript
    {
        public override void Process()
        {
            if ((Battle.Actors?.Count ?? 0) == 0 )
            {
                throw new Exception("No actors");
            }
            if (Battle.HasTeamLost("AI"))
            {
                Battle.SetState(BattleState.Won);
                return;
            }
            if (Battle.HasTeamLost("Player"))
            {
                Battle.SetState(BattleState.Lost);
                return;
            }
            var currentActorId = Battle.Actors.IndexOf(Battle.CurrentActor);
            var nextActor = Battle.Actors.Where((actor, id) => !actor.IsDead && id > currentActorId).FirstOrDefault();
            if (nextActor == null)
            {
                Battle.SetState(BattleState.NextRound);
                return;
            }
            Battle.CurrentActor = nextActor;
            if (nextActor.Team == "Player")
            {
                Battle.SetState(BattleState.PlayerTurn);
                return;
            }
            Battle.SetState(BattleState.AiTurn);
        }
    }

    public class PlayerTurnState : BattleStateScript
    {
        public override void Process()
        {
            Console.WriteLine($"It's {Battle.CurrentActor.Name}'s turn to take action");
            Console.Write("Enter target name: ");
            var targetName = Console.ReadLine();
            var aiTeam = Battle.GetTeam("AI");
            var target = Battle.Actors.FirstOrDefault(actor => actor.Name.Equals(targetName, StringComparison.InvariantCultureIgnoreCase));
            if (target == null)
            {
                Console.WriteLine($"No target with name {targetName} found");
                goto wrongTarget;
            }
            else if (target.IsDead)
            {
                Console.WriteLine($"{target.Name} is already dead");
                goto wrongTarget;
            }

            Battle.SetState(BattleState.ActorUsesSkill, target);
            return;

            wrongTarget:
            Console.WriteLine("");
            Battle.SetState(BattleState.NextTurn);
        }
    }

    public class AiTurnState : BattleStateScript
    {
        public override void Process()
        {
            Console.WriteLine($"{Battle.CurrentActor.Name} takes action");
            var target = Battle.GetTeam("Player").First(actor => !actor.IsDead);
            Battle.SetState(BattleState.ActorUsesSkill, target);
        }
    }

    public class OverState : BattleStateScript
    {
        public override void Initialize(object owner, object paramObj = null)
        {
            base.Initialize(owner, paramObj);
            Console.WriteLine("Battle is over");
        }

        public override void Process()
        {
        }
    }
}

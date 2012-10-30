using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class AIDecision : GameStateBase
    {
        private readonly ActorBase _actor;

        public AIDecision(GameState gameState, ActorBase actor) : base(gameState)
        {
            _actor = actor;
            InputProcessor = new NoInputProcessor();
        }

        public override void OnEnter()
        {
            var aiTask = new Task(() => _actor.MakeAIDecision(GameState.Level, AIDecisionComplete));
            aiTask.Start();
        }

        private void AIDecisionComplete(ActionInformation actionInformation, Tile targetTile)
        {
            ActorBase nextActiveUnit = GameState.GetNextActiveUnit();
            Func<IState> state = () => 
                new ActionAnimationPlaying(GameState, actionInformation, targetTile);
            NextState = new StateTransition(state);
        }

        public override void OnExit()
        {

        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

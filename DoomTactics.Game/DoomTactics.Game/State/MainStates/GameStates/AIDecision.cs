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
            InputProcessor = new NoInputProcessor(Keyboard.GetState(), Mouse.GetState());
        }

        public override void OnEnter()
        {
            var aiTask = new Task(() => _actor.MakeAIDecision(GameState.Level, AIDecisionComplete));
            aiTask.Start();
        }

        private void AIDecisionComplete(ActionInformation actionInformation, Tile targetTile)
        {
            ActorBase nextActiveUnit = GameState.GetNextActiveUnit();
            IState afterScriptStateTemp;
            if (nextActiveUnit == null)
            {
                afterScriptStateTemp = new FreeCamera(GameState, null);
            }
            else
            {
                afterScriptStateTemp = new ActionSelection(GameState, nextActiveUnit);
            }
            

            Func<IState> state = () => new ActionAnimationPlaying(GameState, 
                                                   //new AIDecision(GameState, _actor),
                                                   afterScriptStateTemp, 
                                                   actionInformation.Script(targetTile));
            NextState = new StateTransition(state);
        }

        public override void OnExit()
        {
            GameState.ActiveUnit.EndTurn();
            GameState.ActiveUnit = GameState.GetNextActiveUnit();
        }



        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

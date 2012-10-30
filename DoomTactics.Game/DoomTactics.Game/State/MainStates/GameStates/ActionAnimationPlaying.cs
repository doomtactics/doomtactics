using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class ActionAnimationPlaying : GameStateBase
    {
        private readonly ActionInformation _actionInformation;
        private readonly ActionAnimationScript _script;
        private readonly string _stateName;
        private static int _num = 0;

        public ActionAnimationPlaying(GameState gameState, ActionInformation actionInformation, Tile tile) : base(gameState)
        {
            _actionInformation = actionInformation;
            _stateName = "ActorAnimationPlaying" + _num++;
            _script = _actionInformation.Script(tile);
            InputProcessor = new NoInputProcessor();
        }

        public override void OnEnter()
        {
            MessagingSystem.Subscribe((evt) => ScriptCompletion(evt, _script.Name), DoomEventType.AnimationScriptComplete, _stateName, null);
            _script.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _script.Update();
        }

        public override void OnExit()
        {
            MessagingSystem.Unsubscribe(_stateName);

        }

        public override bool IsPaused
        {
            get { return false; }
        }

        public void ScriptCompletion(IDoomEvent doomEvent, string scriptName)
        {
            AnimationScriptEvent ase = (AnimationScriptEvent) doomEvent;
            if (ase.ScriptName == scriptName)
            {                
                if (_actionInformation.ActionType == ActionType.Attack)
                    GameState.ActiveUnit.SetActioned();
                if (_actionInformation.ActionType == ActionType.Move)
                    GameState.ActiveUnit.SetMoved();

                if (_actionInformation.ActionType == ActionType.Wait)
                {
                    GameState.ActiveUnit.EndTurn();
                    GameState.ActiveUnit = GameState.GetNextActiveUnit();
                    NextState = GameState.ActiveUnit == null
                                    ? new StateTransition(() => new FreeCamera(GameState, null))
                                    : new StateTransition(() => new ActionSelection(GameState, GameState.ActiveUnit));
                }
                else if(!GameState.ActiveUnit.CanAction() && !GameState.ActiveUnit.CanMove())
                {
                    NextState = new StateTransition(() => new SelectWaitDirection(GameState));
                }
                else
                {
                    NextState = new StateTransition(() => new ActionSelection(GameState, GameState.ActiveUnit));
                }
            }
        }
    }
}

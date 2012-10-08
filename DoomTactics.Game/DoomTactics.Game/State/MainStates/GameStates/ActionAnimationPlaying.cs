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
        private readonly IState _nextState;
        private readonly ActionAnimationScript _script;
        private readonly string _stateName;
        private static int _num = 0;

        public ActionAnimationPlaying(GameState gameState, IState nextState, ActionAnimationScript script) : base(gameState)
        {
            _nextState = nextState;
            _script = script;
            _stateName = "ActorAnimationPlaying" + _num++;
            InputProcessor = new NoInputProcessor(Keyboard.GetState(), Mouse.GetState());
        }

        public override void OnEnter()
        {
            MessagingSystem.Subscribe((evt) => ScriptCompletion(evt, _script.Name), DoomEventType.AnimationScriptComplete, _stateName, null);
            _script.Start();
        }

        public override StateTransition Update(GameTime gameTime)
        {
            if (NextState != null)
                return NextState;

            base.Update(gameTime);
            _script.Update();

            return NextState;
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
                NextState = new StateTransition(_nextState);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class AbilitySelectionProcessor : GameStateBaseInputProcessor
    {
        private readonly AbilitySelection _abilitySelection;

        public AbilitySelectionProcessor(GameState gameState, AbilitySelection abilitySelection) 
            : base(gameState)
        {
            _abilitySelection = abilitySelection;
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (KeyPressed(Keys.V, keyState))
            {
                _abilitySelection.ReturnToPrevious();
            }
            if (MouseButtonClicked(mouseState.LeftButton, OldMouseState.LeftButton))
            {
                if (!_abilitySelection.IsConfirming)
                {
                    _abilitySelection.SwitchToConfirming();
                }
                else
                {
                    _abilitySelection.PerformAction();
                }
            }

            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}

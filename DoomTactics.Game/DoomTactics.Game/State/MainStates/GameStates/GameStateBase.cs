﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public abstract class GameStateBase : IState
    {
        protected IInputProcessor InputProcessor;
        public StateTransition NextState { get; protected set; }

        protected readonly GameState GameState;

        protected GameStateBase(GameState gameState)
        {
            GameState = gameState;
        }

        public virtual void OnEnter()
        {
            NextState = null;
        }

        public abstract void OnExit();

        public abstract bool IsPaused { get; }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var actor in GameState.Level.Actors)
            {
                actor.Update(gameTime);
            }

            foreach (var floatingText in GameState.FloatingTexts)
            {
                floatingText.Update(gameTime);
            }

            InputProcessor.ProcessInput(Keyboard.GetState(), Mouse.GetState(), gameTime);
        }

        public virtual void Render(GraphicsDevice device)
        {
            GameState.Level.DrawBackground(device, GameState.SpriteBatch);

            GameState.Effect.WorldViewProj = Matrix.Identity * GameState.Camera.View * GameState.Camera.Projection;

            device.RasterizerState = RasterizerState.CullNone;
            device.DepthStencilState = DepthStencilState.Default;

            RenderTiles(device);
            RenderActors(device);
            RenderText(device);
        }

        private void RenderActors(GraphicsDevice device)
        {

            GameState.SpriteEffect.TextureEnabled = true;
            GameState.SpriteEffect.VertexColorEnabled = true;

            //_spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, _alphaTestEffect);
            // Pass 1: full alpha
            GameState.AlphaTestEffect.AlphaFunction = CompareFunction.Greater;
            GameState.AlphaTestEffect.ReferenceAlpha = 128;
            foreach (var actor in GameState.Level.Actors)
            {
                actor.Render(device, GameState.SpriteBatch, GameState.AlphaTestEffect, GameState.Camera, 0);
            }
            // Pass 2: alpha blend
            foreach (var actor in GameState.Level.Actors)
            {
                GameState.AlphaTestEffect.AlphaFunction = CompareFunction.Less;
                GameState.AlphaTestEffect.ReferenceAlpha = 20;
                actor.Render(device, GameState.SpriteBatch, GameState.AlphaTestEffect, GameState.Camera, 1);
            }
            //_spriteBatch.End();

            if (GameState.Desktop.Visible)
            {
                GameState.Desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
                GameState.Desktop.Draw();
            }
        }

        private void RenderTiles(GraphicsDevice device)
        {
            foreach (var tile in GameState.Level.Tiles)
            {
                tile.Render(device, GameState.Effect);
            }
        }

        private void RenderText(GraphicsDevice device)
        {
            foreach (var text in GameState.FloatingTexts)
            {
                text.Render(device, GameState.Camera, GameState.SpriteBatch, GameState.TextEffect);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public static class SpriteSheetFactory
    {
        private static IDictionary<ActorType, Func<SpriteSheet>> _sheets;

        public static SpriteSheet CreateSpriteSheet(ActorType actorType)
        {
            if (_sheets == null)
                throw new InvalidOperationException("_sheets was not initialized -- need to call SpriteSheetFactory.Initialize");

            return _sheets[actorType].Invoke();
        }

        public static void Initialize(ContentManager contentManager)
        {
            if (_sheets == null)
            {
                _sheets = new Dictionary<ActorType, Func<SpriteSheet>>();
                _sheets[ActorType.Imp] = () => new ImpSheet(contentManager.Load<Texture2D>("sheets\\impsheetnew"));
                _sheets[ActorType.ImpFireball] = () => new ImpFireballSheet(contentManager.Load<Texture2D>("sheets\\impfireballsheet"));
                _sheets[ActorType.Caco] = () => new CacoSheet(contentManager.Load<Texture2D>("sheets\\cacosheet"));
            }
        }
    }
}
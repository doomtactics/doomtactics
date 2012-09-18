﻿// Type: Microsoft.Xna.Framework.Graphics.SpriteSortMode
// Assembly: Microsoft.Xna.Framework.Graphics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553
// Assembly location: C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Graphics.dll

namespace Microsoft.Xna.Framework.Graphics
{
  /// <summary>
  /// Defines sprite sort-rendering options.
  /// </summary>
  /// <param name="BackToFront">Same as Deferred mode, except sprites are sorted by depth in back-to-front order prior to drawing. This procedure is recommended when drawing transparent sprites of varying depths.</param><param name="Deferred">Sprites are not drawn until End is called. End will apply graphics device settings and draw all the sprites in one batch, in the same order calls to Draw were received. This mode allows Draw calls to two or more instances of SpriteBatch without introducing conflicting graphics device settings. SpriteBatch defaults to Deferred mode.</param><param name="FrontToBack">Same as Deferred mode, except sprites are sorted by depth in front-to-back order prior to drawing. This procedure is recommended when drawing opaque sprites of varying depths.</param><param name="Immediate">Begin will apply new graphics device settings, and sprites will be drawn within each Draw call. In Immediate mode there can only be one active SpriteBatch instance without introducing conflicting device settings.</param><param name="Texture">Same as Deferred mode, except sprites are sorted by texture prior to drawing. This can improve performance when drawing non-overlapping sprites of uniform depth.</param>
  public enum SpriteSortMode
  {
    Deferred,
    Immediate,
    Texture,
    BackToFront,
    FrontToBack,
  }
}

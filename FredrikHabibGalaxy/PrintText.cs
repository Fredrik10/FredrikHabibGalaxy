using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FredrikHabibGalaxy
{
    class PrintText
    {
        SpriteFont font;
        private SpriteBatch spriteBatch;

        public PrintText(SpriteFont font)
        {
            this.font = font;
        }

        public PrintText(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Print (string text, SpriteBatch spriteBatch, int X, int Y)
        {
            spriteBatch.DrawString(font, text, new Vector2(X, Y), Color.White);
        }
    }
}

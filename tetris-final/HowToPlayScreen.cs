using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tetris_final
{
    public class HowToPlayScreen
    {
        private SpriteFont _font;
        private string[] _instructions;

        public HowToPlayScreen(SpriteFont font)
        {
            _font = font;

            // Instructions pour jouer à Tetris
            _instructions = new string[]
            {
                "How To Play Tetris:",
                "------------------",
                "- Move the falling blocks using the arrow keys.",
                "- Rotate the blocks using the up arrow key.",
                "- Drop the blocks faster using the down arrow key.",
                "- Clear lines by filling them completely.",
                "- The game ends when the blocks reach the top.",
                "- Have fun and try to beat your high score!"
            };
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
           
        }

        public void LoadContent(ContentManager contentManager)
        {
            _font = contentManager.Load<SpriteFont>("BasicFonts"); // Charger la police de caractères
        }

        public void Update(GameTime gameTime)
        {
            // Mise à jour de l'écran "How To Play"
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, SpriteFont basicFonts)
        {
            graphicsDevice.Clear(Color.Black);

            // Affichage du titre "How To Play"
            Vector2 titlePosition = new Vector2(
                (graphicsDevice.Viewport.Width - _font.MeasureString("How To Play").X) / 2,
                graphicsDevice.Viewport.Height / 4);
            spriteBatch.DrawString(_font, "How To Play", titlePosition, Color.White);

            // Affichage des instructions
            Vector2 instructionsPosition = new Vector2(
                (graphicsDevice.Viewport.Width - _font.MeasureString(_instructions[0]).X) / 2,
                graphicsDevice.Viewport.Height / 2);

            float lineHeight = _font.MeasureString(_instructions[0]).Y + 10;

            for (int i = 0; i < _instructions.Length; i++)
            {
                spriteBatch.DrawString(_font, _instructions[i], instructionsPosition, Color.White);
                instructionsPosition.Y += lineHeight;
            }
        }
    }
}

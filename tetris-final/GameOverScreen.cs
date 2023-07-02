using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tetris_final
{
    public class GameOverScreen
    {
        private Texture2D _backgroundImage;

        public GameOverScreen(Texture2D backgroundImage)
        {
            _backgroundImage = backgroundImage;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            // Initialisation de l'écran de fin de partie
        }

        public void LoadContent()
        {
            // Chargement du contenu de l'écran de fin de partie (si nécessaire)
        }

        public void Update(GameTime gameTime)
        {
            // Mise à jour de l'écran de fin de partie
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(_backgroundImage, graphicsDevice.Viewport.Bounds, Color.White);
        }
    }
}

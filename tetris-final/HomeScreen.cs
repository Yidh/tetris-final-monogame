using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tetris_final
{
    public class HomeScreen
    {
        private Texture2D _backgroundImage;

        public HomeScreen(Texture2D backgroundImage)
        {
            _backgroundImage = backgroundImage;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            // Initialisation de l'écran d'accueil
        }

        public void LoadContent()
        {
            // Chargement du contenu de l'écran d'accueil (si nécessaire)
        }

        public void Update(GameTime gameTime)
        {
            // Mise à jour de l'écran d'accueil
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(_backgroundImage, graphicsDevice.Viewport.Bounds, Color.White);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tetris_final
{
    public class HighScoreScreen
    {
        private SpriteFont _font;
        private List<IScore> _scores;
        private List<int> _highScores;
        private GameScreen _gameScreen;

        public HighScoreScreen(SpriteFont font, List<IScore> scores , GameScreen gameScreen)
        {
            
            _scores = scores.OrderByDescending(score => score.GetTopScores(10).FirstOrDefault()).ToList();
            _gameScreen = gameScreen;
            Console.WriteLine( "test "  + _scores);
            _highScores = new List<int>();
        }
        public void AddScore(int score)
        {
            _highScores.Add(score);
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

            // Affichage du titre "High Scores"
            Vector2 titlePosition = new Vector2(
                (graphicsDevice.Viewport.Width - basicFonts.MeasureString("High Scores").X) / 2,
                50);
            spriteBatch.DrawString(basicFonts, "High Scores", titlePosition, Color.White);

            // Affichage du dernier score
            string lastScoreText = "Score: " + _gameScreen.LastScore; // Utilisation de la référence _gameScreen
            Vector2 lastScorePosition = new Vector2(
                (graphicsDevice.Viewport.Width - basicFonts.MeasureString(lastScoreText).X) / 2,
                100);
            spriteBatch.DrawString(basicFonts, lastScoreText, lastScorePosition, Color.White);

            // ...
        }



    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace tetris_final
{
    public class Tetris : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenType currentScreen;
        private GameScreen _gameScreen;
        private Texture2D _homeScreenImage;
        private HomeScreen _homeScreen;
        private GameOverScreen _gameOverScreen;
        private HowToPlayScreen _howToPlayScreen;
        private HighScoreScreen _highScoreScreen;
        private Texture2D _gameOverScreenImage;
        private bool _gameRestarted = false;

        private int _blockSize = 30;
        private int _columns = 10;
        private int _rows = 20;
        private KeyboardState _prevKeyboardState;
        private Random _random;
        private SpriteFont _BasicFonts;

        private Song _tetrisTheme; 
        private SoundEffectInstance _tetrisThemeInstance;

        public Tetris()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _random = new Random();
            _gameScreen = new GameScreen(this,_highScoreScreen);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _blockSize * _columns;
            _graphics.PreferredBackBufferHeight = _blockSize * _rows;
            _graphics.ApplyChanges();

            currentScreen = ScreenType.Home;

            _gameScreen.Initialize(GraphicsDevice);

            _homeScreen = new HomeScreen(_homeScreenImage);
            _homeScreen.Initialize(GraphicsDevice);

            _highScoreScreen = new HighScoreScreen(_BasicFonts, _gameScreen.GetTopScores(10).Cast<IScore>().ToList(),_gameScreen);
            _highScoreScreen.Initialize(GraphicsDevice);

            _howToPlayScreen = new HowToPlayScreen(_BasicFonts);
            _howToPlayScreen.Initialize(GraphicsDevice);

            _gameOverScreen = new GameOverScreen(_gameOverScreenImage);
            _gameOverScreen.Initialize(GraphicsDevice);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _BasicFonts = Content.Load<SpriteFont>("BasicFonts"); // Charger la police de caractères
            _homeScreenImage = Content.Load<Texture2D>("Tetris");
            _gameOverScreenImage = Content.Load<Texture2D>("GameOver");

            _tetrisTheme = Content.Load<Song>("Tetris Thème Officiel");
           

            _gameScreen.LoadContent();
            _homeScreen = new HomeScreen(_homeScreenImage);
            _homeScreen.LoadContent();
            _gameOverScreen = new GameOverScreen(_gameOverScreenImage);
            _gameOverScreen.LoadContent();
            _howToPlayScreen.LoadContent(Content); // Assurez-vous que la police de caractères est chargée avant de passer à HowToPlayScreen
            _highScoreScreen.LoadContent(Content);
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (currentScreen == ScreenType.Home)
            {
                _homeScreen.Update(gameTime);

                if (keyboardState.IsKeyDown(Keys.G) && _prevKeyboardState.IsKeyUp(Keys.G))
                {
                    currentScreen = ScreenType.Game;
                }
                if (keyboardState.IsKeyDown(Keys.H) && _prevKeyboardState.IsKeyUp(Keys.H))
                {
                    currentScreen = ScreenType.HowToPlay;
                }
            }


            if (currentScreen == ScreenType.HowToPlay)
            {
                _howToPlayScreen.Update(gameTime);

                if (keyboardState.IsKeyDown(Keys.A) && _prevKeyboardState.IsKeyUp(Keys.A))
                {
                    currentScreen = ScreenType.Home;
                }
                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }

            else if (currentScreen == ScreenType.Game)
            {
                _gameScreen.Update(gameTime);

                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_tetrisTheme);
                }

                if (_gameScreen.GameOver)
                {
                    currentScreen = ScreenType.GameOver;
                    _gameOverScreen.Initialize(GraphicsDevice);
                }
                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }
            else if (currentScreen == ScreenType.GameOver)
            {
                _gameOverScreen.Update(gameTime);

                MediaPlayer.Stop();

                if (keyboardState.IsKeyDown(Keys.R) && _prevKeyboardState.IsKeyUp(Keys.R))
                {
                    if (!_gameRestarted) // Vérifiez si le jeu n'a pas déjà été redémarré
                    {
                        currentScreen = ScreenType.Game;
                        _gameScreen.Initialize(GraphicsDevice);
                        _gameScreen.RestartGame(); // Appel de la méthode de redémarrage dans GameScreen
                        _gameRestarted = true; // Définir le drapeau de redémarrage sur true
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.S) && _prevKeyboardState.IsKeyUp(Keys.S))
                {
                   
                    currentScreen = ScreenType.Score;
                }
                else if (keyboardState.IsKeyDown(Keys.A) && _prevKeyboardState.IsKeyUp(Keys.A))
                {
                    currentScreen = ScreenType.Home;
                }

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                if (keyboardState.IsKeyUp(Keys.R)) // Réinitialisez le drapeau de redémarrage lorsque la touche R est relâchée
                {
                    _gameRestarted = false;
                }
            }
            else if (currentScreen == ScreenType.Score)
            {

                _highScoreScreen.Update(gameTime);
                // Logique pour afficher l'écran des scores
                // Assurez-vous de dessiner les scores contenus dans la liste _highScoreScreen.GetScores()

                if (keyboardState.IsKeyDown(Keys.S) && _prevKeyboardState.IsKeyUp(Keys.S))
                {
                    currentScreen = ScreenType.Home;
                }
            }

            _prevKeyboardState = keyboardState;

            base.Update(gameTime);
        }




        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if (currentScreen == ScreenType.Home)
            {
                _homeScreen.Draw(_spriteBatch, GraphicsDevice);
            }
            else if (currentScreen == ScreenType.HowToPlay)
            {
                GraphicsDevice.Clear(Color.Black);
                _howToPlayScreen.Draw(_spriteBatch, GraphicsDevice, _BasicFonts);
            }
            else if (currentScreen == ScreenType.Game)
            {
                GraphicsDevice.Clear(Color.Black);
                _gameScreen.Draw(_spriteBatch, GraphicsDevice, _BasicFonts);
            }
            else if (currentScreen == ScreenType.GameOver)
            {
                GraphicsDevice.Clear(Color.Red);
                _gameOverScreen.Draw(_spriteBatch, GraphicsDevice);
            }
            else if (currentScreen == ScreenType.Pause)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else if (currentScreen == ScreenType.Score)
            {
                GraphicsDevice.Clear(Color.Black);
                _highScoreScreen.Draw(_spriteBatch, GraphicsDevice, _BasicFonts);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}

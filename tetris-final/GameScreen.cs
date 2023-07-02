using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace tetris_final
{
    public class GameScreen : IScore
    {
        private Tetris _game;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private ScreenType currentScreen;
        public bool GameOver { get; private set; }

        private int _blockSize = 30;
        private int _columns = 10;
        private int _rows = 20;
        private int[][] _grid;
        private Dictionary<int, Color> _colors;
        private int _score = 0;
        private List<int> _scores;
        private int _lastScore;

        private HighScoreScreen _highScoreScreen;


        private List<int[][]> _tetrominos;
        private int[][] _currentTetromino;
        private Vector2 _tetrominoPosition;

        private float _fallingSpeed = 0.5f;
        private float _fallingTimer = 0f;

        private KeyboardState _prevKeyboardState;
        private Random _random;
        private SpriteFont _basicFont;

        public int LastScore
        {
            get { return _lastScore; }
        }


        public GameScreen(Tetris game, HighScoreScreen highScoreScreen) : base()
        {
            _game = game;
            _random = new Random();
            _scores = new List<int>();
            _highScoreScreen = highScoreScreen;
        }


        public void AddScore(int score)
        {
            _scores.Add(score);
 
        }
        public List<int> GetTopScores(int count)
        {
           var topScores = _scores.OrderByDescending(s => s).Take(count).ToList();
            Console.WriteLine("Scores obtenus :");
            foreach (var score in topScores)
            {
                Console.WriteLine(score);
            }

            return topScores;
          
        }


        public void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public void LoadContent()
        {
            _content = new ContentManager(_game.Content.ServiceProvider, _game.Content.RootDirectory);

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _basicFont = _content.Load<SpriteFont>("BasicFonts");

            _grid = new int[_rows][];
            for (int row = 0; row < _rows; row++)
            {
                _grid[row] = new int[_columns];
            }

            _colors = new Dictionary<int, Color>
            {
                { 0, Color.Black },
                { 1, Color.Red },
                { 2, Color.Blue },
                { 3, Color.Green },
                { 4, Color.Yellow },
                // Ajoutez plus de couleurs si nécessaire
            };

            _tetrominos = new List<int[][]>
            {
                new int[][] // Carré
                {
                    new int[] { 1, 1 },
                    new int[] { 1, 1 }
                },
                new int[][] // L
                {
                    new int[] { 0, 0, 1 },
                    new int[] { 1, 1, 1 }
                },
                new int[][] // L inversé
                {
                    new int[] { 1, 0, 0 },
                    new int[] { 1, 1, 1 }
                },
                new int[][] // T
                {
                    new int[] { 0, 1, 0 },
                    new int[] { 1, 1, 1 }
                },
                new int[][] // Ligne
                {
                    new int[] { 1, 1, 1, 1 }
                }
            };

            SpawnNewTetromino();
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            

            if (keyboardState.IsKeyDown(Keys.Left) && _prevKeyboardState.IsKeyUp(Keys.Left))
            {
                // Vérifie si le déplacement vers la gauche est possible
                if (IsMovementPossible(_currentTetromino, new Vector2(_tetrominoPosition.X - 1, _tetrominoPosition.Y)))
                {
                    _tetrominoPosition.X -= 1;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Right) && _prevKeyboardState.IsKeyUp(Keys.Right))
            {
                
                // Vérifie si le déplacement vers la droite est possible
                if (IsMovementPossible(_currentTetromino, new Vector2(_tetrominoPosition.X + 1, _tetrominoPosition.Y)))
                {
                    _tetrominoPosition.X += 1;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Down) && _prevKeyboardState.IsKeyUp(Keys.Down))
            {
                // Vérifie si le déplacement vers le bas est possible
                if (IsMovementPossible(_currentTetromino, new Vector2(_tetrominoPosition.X, _tetrominoPosition.Y + 1)))
                {
                    _tetrominoPosition.Y += 1;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Space) && _prevKeyboardState.IsKeyUp(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) && _prevKeyboardState.IsKeyUp(Keys.Up))
            {
                // Effectue une rotation du tétrmino
                RotateTetromino();
            }

            _fallingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_fallingTimer >= _fallingSpeed)
            {
                if (IsMovementPossible(_currentTetromino, new Vector2(_tetrominoPosition.X, _tetrominoPosition.Y + 1)))
                {
                    // Vérifie si le déplacement vers le bas est possible
                    _tetrominoPosition.Y += 1;
                }
                else
                {
                    // Verrouille le tétrmino en place
                    LockTetromino();
                    // Vérifie s'il y a des lignes complètes à effacer
                    CheckForLineClear();
                    if (IsGameOver())
                    {
                        // Si c'est la fin du jeu, affiche l'écran de fin de partie
                        currentScreen = ScreenType.GameOver;
                        GameOver = true;
                    }
                    else
                    {
                        // Sinon, génère un nouveau tétrmino
                        SpawnNewTetromino();
                    }
                }

                _fallingTimer = 0f;
            }

            _prevKeyboardState = keyboardState;
        }


        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, SpriteFont basicFonts)
        {
            // Dessiner la grille
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    if (_grid[row][col] != 0)
                    {
                        Rectangle blockRectangle = new Rectangle(col * _blockSize, row * _blockSize, _blockSize, _blockSize);
                        spriteBatch.Draw(Texture2DExt.FromColor(graphicsDevice, _colors[_grid[row][col]], _blockSize, _blockSize), blockRectangle, Color.White);
                    }
                }
            }

            // Dessiner le tétraminos actuel
            for (int row = 0; row < _currentTetromino.Length; row++)
            {
                for (int col = 0; col < _currentTetromino[row].Length; col++)
                {
                    if (_currentTetromino[row][col] != 0)
                    {
                        int blockX = (int)(_tetrominoPosition.X + col) * _blockSize;
                        int blockY = (int)(_tetrominoPosition.Y + row) * _blockSize;
                        Rectangle blockRectangle = new Rectangle(blockX, blockY, _blockSize, _blockSize);
                        spriteBatch.Draw(Texture2DExt.FromColor(graphicsDevice, _colors[_currentTetromino[row][col]], _blockSize, _blockSize), blockRectangle, Color.White);
                    }
                }
            }

            string scoreText = "Score: " + _score;
            Vector2 scorePosition = new Vector2(10, 10);
            spriteBatch.DrawString(basicFonts, scoreText, scorePosition, Color.White);
        }

        private bool IsMovementPossible(int[][] tetromino, Vector2 position)
        {
            for (int row = 0; row < tetromino.Length; row++)
            {
                for (int col = 0; col < tetromino[row].Length; col++)
                {
                    if (tetromino[row][col] != 0)
                    {
                        int newRow = (int)(position.Y + row);
                        int newCol = (int)(position.X + col);

                        if (newRow < 0 || newRow >= _rows || newCol < 0 || newCol >= _columns || _grid[newRow][newCol] != 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsGameOver()
        {
            // Check if any blocks in the top row are occupied
            for (int col = 0; col < _columns; col++)
            {
                if (_grid[0][col] != 0)
                {
                    return true;
                }
            }

            return false;
        }


        private void RotateTetromino()
        {
            int[][] rotatedTetromino = new int[_currentTetromino[0].Length][];
            for (int row = 0; row < _currentTetromino[0].Length; row++)
            {
                rotatedTetromino[row] = new int[_currentTetromino.Length];
                for (int col = 0; col < _currentTetromino.Length; col++)
                {
                    rotatedTetromino[row][col] = _currentTetromino[_currentTetromino.Length - col - 1][row];
                }
            }

            if (IsMovementPossible(rotatedTetromino, _tetrominoPosition))
            {
                _currentTetromino = rotatedTetromino;
            }
        }

        private void LockTetromino()
        {
            for (int row = 0; row < _currentTetromino.Length; row++)
            {
                for (int col = 0; col < _currentTetromino[row].Length; col++)
                {
                    if (_currentTetromino[row][col] != 0)
                    {
                        int newRow = (int)(_tetrominoPosition.Y + row);
                        int newCol = (int)(_tetrominoPosition.X + col);

                        _grid[newRow][newCol] = _currentTetromino[row][col];
                    }
                }
            }
        }

        private void CheckForLineClear()
        {
            int linesCompleted = 0;
            for (int row = _rows - 1; row >= 0; row--)
            {
                if (IsLineComplete(row))
                {
                    RemoveLine(row);
                    ShiftLinesDown(row);
                    linesCompleted++;

                }
            }
            if (linesCompleted > 0)
            {
                UpdateScore(linesCompleted);
            }
        }

        private void UpdateScore(int linesCompleted)
        {
            int pointsPerLine = 100;
            int additionalPoints = linesCompleted * pointsPerLine;
            _score += additionalPoints;
            _lastScore = _score;
            AddScore(_score); // Ajoute le score à la liste des scores
        }


        private bool IsLineComplete(int row)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (_grid[row][col] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void RemoveLine(int row)
        {
            for (int col = 0; col < _columns; col++)
            {
                _grid[row][col] = 0;
            }
        }

        private void ShiftLinesDown(int startRow)
        {
            for (int row = startRow - 1; row >= 0; row--)
            {
                for (int col = 0; col < _columns; col++)
                {
                    _grid[row + 1][col] = _grid[row][col];
                }
            }
        }

        private void SpawnNewTetromino()
        {
            Random random = new Random();
            int randomIndex = random.Next(_tetrominos.Count);
            _currentTetromino = _tetrominos[randomIndex];

            Color randomColor = GetRandomColor();
            ApplyColorToTetromino(randomColor);

            _tetrominoPosition = new Vector2(_columns / 2 - _currentTetromino[0].Length / 2, 0);
        }

        private Color GetRandomColor()
        {
            Random random = new Random();
            int randomColorIndex = random.Next(1, _colors.Count); // Exclude the black color (index 0)
            return _colors[randomColorIndex];
        }

        private void ApplyColorToTetromino(Color color)
        {
            for (int row = 0; row < _currentTetromino.Length; row++)
            {
                for (int col = 0; col < _currentTetromino[row].Length; col++)
                {
                    if (_currentTetromino[row][col] != 0)
                    {
                        _currentTetromino[row][col] = GetColorIndex(color);
                    }
                }
            }
        }
        public void RestartGame()
        {
            // Réinitialiser les variables du jeu
            _grid = new int[_rows][];
            for (int row = 0; row < _rows; row++)
            {
                _grid[row] = new int[_columns];
            }

            _score = 0;
            GameOver = false;

            // Réinitialiser la position du tétrmino
            _tetrominoPosition = new Vector2(_columns / 2 - _currentTetromino[0].Length / 2, 0);

            // Générer un nouveau tétrmino
            SpawnNewTetromino();
        }


        private int GetColorIndex(Color color)
        {
            foreach (KeyValuePair<int, Color> pair in _colors)
            {
                if (pair.Value == color)
                {
                    return pair.Key;
                }
            }

            return 0; // Black color index
        }
    }



    public static class Texture2DExt
    {
        public static Texture2D FromColor(GraphicsDevice graphicsDevice, Color color, int width = 1, int height = 1)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            texture.SetData(colors);
            return texture;
        }
    }



}


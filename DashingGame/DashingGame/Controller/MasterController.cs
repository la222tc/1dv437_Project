using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using DashingGame.Model;
using System.IO;
using System.Linq;

namespace DashingGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MasterController : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont defaultFont;
        Board board;

        KeyboardState prevState = Keyboard.GetState();
        Directions playerGo = Directions.None;
        Directions playerDirection = Directions.None;

        int CurrentLevelNumber = 0;
        const float frameSkip = 8f;
        int frameNumber = 0;
        const int gameStatusBorder = 50;
        bool toggleMenu = false;

        public MasterController()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GetWindowSizeX;
            graphics.PreferredBackBufferHeight = gameStatusBorder + GetWindowSizeY;
        }

        public int GetWindowSizeX
        {
            get
            {
                return Board.BLOCKSIZE * Board.BOARDSIZEX;
            }
        }

        public int GetWindowSizeY
        {
            get
            {
                return Board.BLOCKSIZE * Board.BOARDSIZEY;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            defaultFont = Content.Load<SpriteFont>("DefaultFont");

            LoadBoard();
            new Textures(GraphicsDevice);
        }

        private void LoadBoard()
        {
            Levels.Instance.Reset();
            board = Levels.Instance.TheLevels.ElementAt(CurrentLevelNumber);
            
        }
        private void MoveToNextBoard()
        {
            CurrentLevelNumber++;
            if (CurrentLevelNumber >= Levels.Instance.TheLevels.Count())
            {
                CurrentLevelNumber = 0;
            }
            LoadBoard();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            playerDirection = Directions.None;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (board.Restart)
            {
                LoadBoard();
            }
            if (board.CompletedLevel)
            {
                MoveToNextBoard();
            }
            if (state.IsKeyDown(Keys.R))
            {
                board.KillThePlayer();
            }
                // toggle Menu On/Off
            if (state.IsKeyDown(Keys.Tab) && !prevState.IsKeyDown(Keys.Tab))
            {
                toggleMenu = !toggleMenu;
            }


            if (!prevState.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Left))
            {
                playerGo = Directions.West;
            }
            if (!prevState.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Right))
            {
                playerGo = Directions.East;
            }

            if (!prevState.IsKeyDown(Keys.Up) && state.IsKeyDown(Keys.Up))
            {
                playerGo = Directions.North;
            }

            if (!prevState.IsKeyDown(Keys.Down) && state.IsKeyDown(Keys.Down))
            {
                playerGo = Directions.South;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                playerDirection = Directions.West;
            }    
            else if(state.IsKeyDown(Keys.Right))
            {
                playerDirection = Directions.East;
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                playerDirection = Directions.North;
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                playerDirection = Directions.South;
            }         

            frameNumber++;
            if (frameNumber >= frameSkip)
            {
                frameNumber = 0;

                board.UpdateBoard(gameTime, state);

                if (playerDirection != Directions.None)
                {
                    board.UpdatePlayer(playerDirection);
                    playerDirection = Directions.None;
                    playerGo = Directions.None;
                }
                else if(playerGo != Directions.None)
                    {
                        board.UpdatePlayer(playerGo);
                        playerGo = Directions.None;
                    }
            }
            prevState = state;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            board.DrawBoard(spriteBatch);
            DrawGameStatus();
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void DrawGameStatus()
        {
            var heartCompletedText = string.Format("HEARTS: {0}/{1}", board.HeartsTaken, board.HeartsToComplete);

            spriteBatch.DrawString(defaultFont, heartCompletedText, new Vector2(0.0f, GetWindowSizeY + 10), Color.Yellow);

            var toggleMenuText = string.Format("Press Tab for Menu");
            var toggleMenuTextSize = defaultFont.MeasureString(toggleMenuText);
            spriteBatch.DrawString(defaultFont, toggleMenuText, new Vector2(GetWindowSizeX - toggleMenuTextSize.X - 5, GetWindowSizeY + 10), Color.Yellow);

            if (toggleMenu)
            {
                string[] menu = new[] 
                {
                    "Dashing Game",
                    "Esc - Exit Game",
                    "Arrow keys - Moving",
                    "R - restart level"
                };

                int index = 0;
                foreach (var menuString in menu)
                {
                    spriteBatch.DrawString(defaultFont, menuString, new Vector2(GetWindowSizeX / 2 - 100, 2 + ((index) * 21)), Color.Black);
                    spriteBatch.DrawString(defaultFont, menuString, new Vector2(GetWindowSizeX / 2 - 100, (index) * 21), Color.White);
                    index++;
                }
            }
        }
    }
}

using System;
using System.Net.Mime;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class MosquitoAttackGame : Game
{
    private const int _WindowWidth = 550, _WindowHeight = 400, _NumMosquitos = 10;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;
    private SpriteFont _font;
    private string _message = "";
    private KeyboardState _kbCurrentState, _kbPreviousState;

    // enum - used to determine states of the game (datatype)
    private enum GameState { Playing, Paused, Over }
    private GameState _gameState;

    public Cannon _cannon;
    public Mosquito[] _mosquitoes;

    private Rectangle BoundingBox
    {
        get { return new Rectangle(0, 0, _WindowWidth, _WindowHeight); }
    }

    public MosquitoAttackGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();

        _cannon = new Cannon();
        _cannon.Initialize(new Vector2(50, 325), 235, BoundingBox);

        #region randomize mosquito generation
        _mosquitoes = new Mosquito[_NumMosquitos];

        for(int c = 0; c < _NumMosquitos; c++)
        {
            _mosquitoes[c] = new Mosquito();
        }

        Random random = new Random();
        foreach (Mosquito mosquito in _mosquitoes)
        {
            int direction = random.Next(1, 3) == 2 ? -1 : 1;

            int xPosition = random.Next(1, _WindowWidth - 50);
            int yPosition = random.Next(1, 151);
            int speed = random.Next(150, 251);

            mosquito.Initialize(new Vector2(xPosition, yPosition), speed, new Vector2(direction, 0), BoundingBox);
        }
        #endregion

        // Sets state of game on startup
        _gameState = GameState.Playing;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");
        _font = Content.Load<SpriteFont>("SystemArialFont");

        _cannon.LoadContent(Content);
        foreach(Mosquito mosquito in _mosquitoes)
        {
            mosquito.LoadContent(Content);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        
        _kbCurrentState = Keyboard.GetState();

        #region Update GameState
        // Update GameState
        switch (_gameState)
        {
            case GameState.Playing:
                #region Keyboard Input
                // Updates cannon direction ONLY if state is 'Playing' 
                if (_kbCurrentState.IsKeyDown(Keys.A))
                {
                    _cannon.Direction = new Vector2(-1, 0);
                }else if (_kbCurrentState.IsKeyDown(Keys.D))
                {
                    _cannon.Direction = new Vector2(1, 0);
                }
                else
                {
                    _cannon.Direction = Vector2.Zero;
                }

                if(Pressed(Keys.P))
                {
                    _gameState = GameState.Paused;
                    _message = "Game paused. Press [p] to continue.";
                }

                if (Pressed(Keys.Space))
                {
                    _cannon.Shoot();
                }

                #endregion

                _cannon.Update(gameTime);
                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Update(gameTime);
                    if(mosquito.Alive && _cannon.ProcessCollision(mosquito.BoundingBox))
                    {
                        mosquito.Die();
                    }
                }
                break;

            case GameState.Paused:
                if(Pressed(Keys.P))
                {
                    _gameState = GameState.Playing;
                    _message = "";
                }
                break;

            case GameState.Over:
                break;
        }
        #endregion
        
        _kbPreviousState = _kbCurrentState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();

        #region Update GameState
        switch (_gameState)
        {
            case GameState.Playing:
                // To tint an image, you can use a colour other than white!
                _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
                _cannon.Draw(_spriteBatch);
                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Draw(_spriteBatch);
                }
                break;

            case GameState.Paused:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.Gray);
                _cannon.Draw(_spriteBatch);

                _spriteBatch.DrawString(_font, _message, new Vector2(120, (_WindowHeight / 2) - 15), Color.White);

                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Draw(_spriteBatch);
                }
                break;

            case GameState.Over:
                break;
        }
        #endregion

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private bool Pressed(Keys key)
    {
        return _kbCurrentState.IsKeyDown(key) && _kbPreviousState.IsKeyUp(key);
                
    }
}

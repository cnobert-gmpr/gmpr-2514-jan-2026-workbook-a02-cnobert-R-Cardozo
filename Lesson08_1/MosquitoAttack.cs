using System;
using System.Data.Common;
using System.Net.Mime;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08_1;

public class MosquitoAttack : Game
{
    private const int _WindowWidth = 550, _WindowHeight = 400, _NumMosquitos = 10;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _background;
    private SpriteFont _font;
    private string _message = "";
    private string _winner = "";
    private KeyboardState _kbCurrentState, _kbPreviousState;
    private Random _random = new Random();
    private bool _CannonDyingAnimationPlaying = false;

    private enum GameState { Menu, Level01, Paused, Over }
    private GameState _gameState;

    public Cannon _cannon;
    public Mosquito[] _mosquitoes;

    private Rectangle BoundingBox
    {
        get { return new Rectangle(0, 0, _WindowWidth, _WindowHeight); }
    }

    public MosquitoAttack()
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

        #region randomize m generation

        _mosquitoes = new Mosquito[_NumMosquitos];

        for(int c = 0; c < _NumMosquitos; c++)
        {
            _mosquitoes[c] = new Mosquito();
        }
        
        foreach(Mosquito m in _mosquitoes)
        {
            int direction = _random.Next(1, 3) == 2 ? -1 : 1;

            int xPosition = _random.Next(1, _WindowWidth - 50);
            int yPosition = _random.Next(1, 151);
            int speed = _random.Next(150, 251);

            m.Initialize(new Vector2(xPosition, yPosition), speed, new Vector2(direction, 0), BoundingBox);
        }

        #endregion

        _gameState = GameState.Menu;
        _message = GetMenuMessage();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");
        _font = Content.Load<SpriteFont>("SystemArialFont");

        _cannon.LoadContent(Content);

        foreach(Mosquito m in _mosquitoes)
        {
            m.LoadContent(Content);
        }

    }

    protected override void Update(GameTime gameTime)
    {
        _kbCurrentState = Keyboard.GetState();

        #region update GameState

        switch (_gameState)
        {
            case GameState.Menu:
                if (Pressed(Keys.P))
                {
                    ResetGame();
                    _gameState = GameState.Level01;
                    _message = "";
                }

                break;

            case GameState.Level01:
            {
                #region Keyboard Input

                if (_kbCurrentState.IsKeyDown(Keys.A))
                {
                    _cannon.Direction = new Vector2(-1, 0);
                }

                else if (_kbCurrentState.IsKeyDown(Keys.D))
                {
                    _cannon.Direction = new Vector2(1, 0);
                }

                else
                {
                    _cannon.Direction = Vector2.Zero;
                }

                if (Pressed(Keys.M))
                {
                    _gameState = GameState.Menu;
                    _message = "Main Menu\nPress [p] to resume playing.";
                }

                if (Pressed(Keys.P))
                {
                    _gameState = GameState.Paused;
                    _message = "Game paused. Press [p] to continue.";
                }

                if (Pressed(Keys.Space))
                {
                    _cannon.Shoot();
                }

                if (Pressed(Keys.R))
                    {
                        _cannon.Reload();
                    }

                #endregion

                _cannon.Update(gameTime);

                foreach(Mosquito m in _mosquitoes)
                {
                    m.Update(gameTime);

                    if(m.Alive && _cannon.ProcessCollision(m.BoundingBox))
                    {
                        m.Die();
                    }
                }

                foreach(Mosquito m in _mosquitoes)
                {
                    if (m.Alive && m.ProcessFireballCollision(_cannon.BoundingBox))
                    {
                        if (!_CannonDyingAnimationPlaying)
                        {
                            _cannon.Die();
                            _CannonDyingAnimationPlaying = true;
                        }
                    }
                }

                    if (_CannonDyingAnimationPlaying)
                    {
                        if(_cannon._state == Cannon.State.Dead)
                        {
                            _gameState = GameState.Over;
                            _winner = "Mosquitoes";
                            _message = "Game Over\nMosquitoes win!\nPress [m] for menu";
                            _CannonDyingAnimationPlaying = false;
                        }
                    }

                bool allDead = true;

                foreach(Mosquito m in _mosquitoes)
                {
                    if (m.Alive)
                    {
                        allDead = false;
                        break;
                    }
                }

                if (allDead)
                {
                    _gameState = GameState.Over;
                    _winner = "Player";
                    _message = _winner == "Player" ? "Game Over\nPlayer wins!\nPress [m] for menu" : "Game Over\nMosquitoes win!\nPress [m] for menu";
                }

                foreach(Mosquito m in _mosquitoes)
                {
                    if(m.Alive && m.BoundingBox.Intersects(_cannon.BoundingBox))
                    {
                        _gameState = GameState.Over;
                        _winner = "Mosquitoes";
                        _message = "Game Over\nMosquitoes win!\nPress [m] for menu";
                    }
                }

                break;
            }
            case GameState.Paused:
            {

                if (Pressed(Keys.M))
                {
                    _gameState = GameState.Menu;
                    _message = GetMenuMessage();
                }

                if (Pressed(Keys.P))
                {
                    _gameState = GameState.Level01;
                    _message = "";
                }
                break;
            }

            case GameState.Over:
            {
                if (Pressed(Keys.M))
                {
                    _gameState = GameState.Menu;
                    _message = GetMenuMessage();
                }

                if (Pressed(Keys.P))
                {
                    ResetGame();
                    _gameState = GameState.Level01;
                    _message = "";
                }

                break;
            }
        }

        #endregion

        _kbPreviousState = _kbCurrentState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGreen);
        _spriteBatch.Begin();

        #region update GameState

        switch (_gameState)
        {
            case GameState.Menu:

                _spriteBatch.DrawString(_font, _message, GetCenteredPosition(_message),Color.White);
                
                break;

            case GameState.Level01:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
                _cannon.Draw(_spriteBatch, Color.White);

                foreach (Mosquito m in _mosquitoes)
                {
                    m.Draw(_spriteBatch);
                }

                _spriteBatch.DrawString(_font, "Press [p] to pause, and [m] to go to main menu", new Vector2(125, _WindowHeight - 20), Color.White);

                _spriteBatch.DrawString(_font, $"Ammo: {_cannon.Ammo}/{_cannon.MaxAmmo}", new Vector2(10, _WindowHeight - 40), Color.White);
                if (_cannon.IsReloading)
                {
                    _spriteBatch.DrawString(_font, "Reloading Ammo..", new Vector2(10, 10), Color.Bisque);
                }

                break;

            case GameState.Paused:
                _spriteBatch.Draw(_background, Vector2.Zero, Color.Gray);
                _cannon.Draw(_spriteBatch, Color.Gray);                

                _spriteBatch.DrawString(_font, _message, GetCenteredPosition(_message),Color.White);

                foreach (Mosquito m in _mosquitoes)
                {
                    m.Draw(_spriteBatch);
                }

                break;
            
            case GameState.Over:
                _spriteBatch.DrawString(_font, _message, GetCenteredPosition(_message),Color.White);

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

    private void ResetGame()
    {
        _cannon = new Cannon();
        _cannon.Initialize(new Vector2(50, 325), 235, BoundingBox);
        _cannon.LoadContent(Content);

        #region randomize m generation

        _mosquitoes = new Mosquito[_NumMosquitos];

        for(int c = 0; c < _NumMosquitos; c++)
        {
            _mosquitoes[c] = new Mosquito();
        }

        
        foreach(Mosquito m in _mosquitoes)
        {
            int direction = _random.Next(1, 3) == 2 ? -1 : 1;

            int xPosition = _random.Next(1, _WindowWidth - 50);
            int yPosition = _random.Next(1, 151);
            int speed = _random.Next(150, 251);

            m.Initialize(new Vector2(xPosition, yPosition), speed, new Vector2(direction, 0), BoundingBox);

            m.LoadContent(Content);
        }

        _winner = "";
        _message = "";

        #endregion
    }

    private string GetMenuMessage()
    {
        return "Main Menu\nPress [p] to play\nControl the Cannon with [a] and [d]\nHit all the mosquitoes to win!";
    }

    private Vector2 GetCenteredPosition(string text)
    {
        
        // Centers the text in the middle of the screen
        
        Vector2 size = _font.MeasureString(text);
        return new Vector2(
            (_WindowWidth - size.X) / 2,
            (_WindowHeight - size.Y) / 2
        );
    }
}
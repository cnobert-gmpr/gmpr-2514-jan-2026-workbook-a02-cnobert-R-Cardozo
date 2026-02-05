using System.ComponentModel;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07;

public class Pong : Game
{
    private const int _WindowWidth = 750, _WindowHeight = 450, _BallWidthAndHeight = 21;
    private const int _PlayAreaEdgeLineWidth = 12;
    private const int _PaddleWidth = 6, _PaddleHeight = 54;
    private const int _PaddleTwoWidth = 6, _PaddleTwoHeight = 54;
    private const float _PaddleSpeed = 240, _BallSpeed = 75;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture, _ballTexture, _paddleTexture;
    private Vector2 _ballPosition, _ballDirection;
    private float _ballSpeed;
    private Vector2 _paddlePosition, _paddleDirection, _paddleDimensions;
    private Vector2 _paddleTwoPosition, _paddleTwoDirection, _paddleTwoDimensions;
    private float _paddleSpeed, _paddleTwoSpeed;
    
    #region properties
    internal Rectangle PlayAreaBoundingBox
    {
        get
        {
            return new Rectangle(0, _PlayAreaEdgeLineWidth, _WindowWidth, _WindowHeight - (2 * _PlayAreaEdgeLineWidth));
        }
    }

    /* ALTERNATE VERSION

    internal Rectangle PlayAreaBoundingBox(){
        return new Rectangle(0, 00, _WindowWidth, _WindowHeight);
    }

    */
    #endregion

    public Pong()
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

        _ballPosition = new Vector2(150, 195);
        _ballSpeed = _BallSpeed;

        _ballDirection.X = -1;
        _ballDirection.Y = -1;

        _paddlePosition = new Vector2(690, 198);
        _paddleSpeed = _PaddleSpeed;
        _paddleDimensions = new Vector2(_PaddleWidth, _PaddleHeight);
        _paddleDirection = Vector2.Zero;

        _paddleTwoPosition = new Vector2(56, 198);
        _paddleTwoSpeed = _PaddleSpeed;
        _paddleTwoDimensions = new Vector2(_PaddleTwoWidth, _PaddleTwoHeight);
        _paddleDirection = Vector2.Zero;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundTexture = Content.Load<Texture2D>("Court");
        _ballTexture = Content.Load<Texture2D>("Ball");
        _paddleTexture = Content.Load<Texture2D>("Paddle");
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _ballPosition += _ballDirection * _ballSpeed * dt;

        #region bounce off left or right walls

        if(_ballPosition.X <= PlayAreaBoundingBox.Left || (_ballPosition.X + _BallWidthAndHeight) >= PlayAreaBoundingBox.Right)
        {
            _ballDirection.X *= -1;
        }

        #endregion

        #region bounce off the top or bottom walls

        if (_ballPosition.Y <= PlayAreaBoundingBox.Top || (_ballPosition.Y + _BallWidthAndHeight) >= PlayAreaBoundingBox.Bottom)
        {
            _ballDirection.Y *= -1;
        }

        #endregion

        #region right paddle movement

        KeyboardState kbState = Keyboard.GetState();

        if (kbState.IsKeyDown(Keys.Up))
        {
            _paddleDirection = new Vector2(0, -1);
        }
        else if(kbState.IsKeyDown(Keys.Down)){
            _paddleDirection = new Vector2(0, 1);
        }
        else
        {
            _paddleDirection = Vector2.Zero;
        }

        _paddlePosition += _paddleDirection * _paddleSpeed * dt;

        if(_paddlePosition.Y <= PlayAreaBoundingBox.Top)
        {
            _paddlePosition.Y = PlayAreaBoundingBox.Top;
        }
        else if((_paddlePosition.Y + _paddleDimensions.Y) >= PlayAreaBoundingBox.Bottom){
            _paddlePosition.Y = PlayAreaBoundingBox.Bottom - _paddleDimensions.Y;
        }

        #endregion

        #region left paddle movement

        if (kbState.IsKeyDown(Keys.W))
        {
            _paddleTwoDirection = new Vector2(0, -1);
        }
        else if (kbState.IsKeyDown(Keys.S))
        {
            _paddleTwoDirection = new Vector2(0, 1);
        }
        else
        {
            _paddleTwoDirection = Vector2.Zero;
        }

        if(_paddleTwoPosition.Y <= PlayAreaBoundingBox.Top)
        {
            _paddleTwoPosition.Y = PlayAreaBoundingBox.Top;
        }
        else if ((_paddleTwoPosition.Y + _paddleTwoDimensions.Y) >= PlayAreaBoundingBox.Bottom)
        {
            _paddleTwoPosition.Y = PlayAreaBoundingBox.Bottom - _paddleTwoDimensions.Y;
        }

        #endregion
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _WindowWidth, _WindowHeight), Color.White);

        Rectangle ballRectangle = new Rectangle((int) _ballPosition.X, (int) _ballPosition.Y, _BallWidthAndHeight, _BallWidthAndHeight);
        _spriteBatch.Draw(_ballTexture, ballRectangle, Color.White);

        Rectangle paddleRectangle = new Rectangle((int) _paddlePosition.X, (int) _paddlePosition.Y, (int) _paddleDimensions.X, (int) _paddleDimensions.Y);
        _spriteBatch.Draw(_paddleTexture, paddleRectangle, Color.White);

        Rectangle paddleTwoRectangle = new Rectangle((int) _paddleTwoPosition.X, (int) _paddleTwoPosition.Y, (int) _paddleTwoDimensions.X, (int) _paddleTwoDimensions.Y);
        _spriteBatch.Draw(_paddleTexture, paddleTwoRectangle, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

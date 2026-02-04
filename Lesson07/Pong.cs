using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07;

public class Pong : Game
{
    private const int _WindowWidth = 750, _WindowHeight = 450, _BallWidthAndHeight = 21;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture, _ballTexture;
    private Rectangle _playAreaBoundingBox;
    private Vector2 _ballPosition, _ballDirection;
    private float _ballSpeed;
    
    internal Rectangle PlayAreaBoundingBox
    {
        get
        {
            return new Rectangle(0, 0, _WindowWidth, _WindowHeight);
        }
    }

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
        _ballSpeed = 60;

        _ballDirection.X = -1;
        _ballDirection.Y = -1;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundTexture = Content.Load<Texture2D>("Court");
        _ballTexture = Content.Load<Texture2D>("Ball");
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _ballPosition += _ballDirection * _ballSpeed * dt;

        #region bounce off left or right walls
        if(_ballPosition.X <= _playAreaBoundingBox.Left || _ballPosition.X + _BallWidthAndHeight >= _playAreaBoundingBox.Right)
        {
            _ballDirection *= -1;
        }
        #endregion

        #region bounce off the top or bottom walls
        if (_ballPosition.Y <= _playAreaBoundingBox.Top || _ballPosition.Y + _BallWidthAndHeight >= _playAreaBoundingBox.Bottom)
        {
            _ballDirection *= -1;
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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson07_1;

public class Pong : Game
{
    private const int _Scale = 3;
    private const int _WindowWidth = 250 *_Scale, _WindowHeight = 150 * _Scale;
    private const int _PlayAreaEdgeLineWidth = 4 * _Scale;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture;
    private Rectangle _playAreaBoundingBox;

    private Ball _ball;
    private Paddle _leftPaddle, _rightPaddle;

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

        _playAreaBoundingBox = new Rectangle(0, _PlayAreaEdgeLineWidth, _WindowWidth, _WindowHeight - 2 * _PlayAreaEdgeLineWidth);

        _ball = new Ball();
        _ball.Initialize(new Vector2(50, 65), new Vector2(1, -2), _Scale, _playAreaBoundingBox);

        _leftPaddle = new Paddle();
        _leftPaddle.Initialize(new Vector2(10 * _Scale, 75 * _Scale), _Scale, _playAreaBoundingBox);

        _rightPaddle = new Paddle();
        _rightPaddle.Initialize(new Vector2(240 * _Scale, 75 * _Scale), _Scale, _playAreaBoundingBox);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundTexture = Content.Load<Texture2D>("Court");

        _ball.LoadContent(this.Content);
        _leftPaddle.LoadContent(this.Content);
        _rightPaddle.LoadContent(this.Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        #region left keyboard input
        KeyboardState kbState = Keyboard.GetState();

        if (kbState.IsKeyDown(Keys.W))
        {
            _leftPaddle.Direction = new Vector2(0, -1);
        }else if (kbState.IsKeyDown(Keys.S))
        {
            _leftPaddle.Direction = new Vector2(0, 1);
        }
        else
        {
            _leftPaddle.Direction = new Vector2(0, 0);
        }

        #endregion

        #region right keyboard input
        if (kbState.IsKeyDown(Keys.Up))
        {
            _rightPaddle.Direction = new Vector2(0, -1);
        }else if (kbState.IsKeyDown(Keys.Down))
        {
            _rightPaddle.Direction = new Vector2(0, 1);
        }
        else
        {
            _rightPaddle.Direction = new Vector2(0, 0);
        }

        #endregion

        _ball.Update(gameTime);
        _leftPaddle.Update(gameTime);
        _rightPaddle.Update(gameTime);

        if (_ball.ProcessCollision(_leftPaddle.BoundingBox))
        {
            //_hud.Paddle01Score++;
            //this.Glow();
        }else if (_ball.ProcessCollision(_rightPaddle.BoundingBox))
        {
            //_hud.Paddle01Score++;
            //this.Glow();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, _Scale, SpriteEffects.None, 0);
        _ball.Draw(_spriteBatch);
        _leftPaddle.Draw(_spriteBatch);
        _rightPaddle.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

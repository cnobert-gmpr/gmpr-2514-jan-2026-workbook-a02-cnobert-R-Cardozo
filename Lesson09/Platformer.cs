using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson09;

public class Platformer : Game
{
    internal const float _Gravity = 100;
    private const int _WindowWidth = 550, _WindowHeight = 400;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Rectangle _gameBoundingBox = new Rectangle(0, 0, _WindowWidth, _WindowHeight);

    private Player _player;
    private Collider _ground;

    public Platformer()
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
 
        _player = new Player(new Vector2(50, 50), _gameBoundingBox);
        _player.Initialize();

        _ground = new Collider(new Vector2(0, 300), new Vector2(_WindowWidth, 1), ColliderType.Top);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _player.LoadContent(Content);
        _ground.LoadContent(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        #region input
        KeyboardState kbState = Keyboard.GetState();
        bool left = kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A);
        bool right = kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D);

        if(left)
            _player.MoveHorizontally(-1);

        else if(right)
            _player.MoveHorizontally(1);

        else
            _player.Stop();
        
        if(kbState.IsKeyDown(Keys.Space))
            _player.Jump();
        
        #endregion
        
        _ground.ProcessCollision(_player, gameTime);

        _player.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _player.Draw(_spriteBatch);
        _ground.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

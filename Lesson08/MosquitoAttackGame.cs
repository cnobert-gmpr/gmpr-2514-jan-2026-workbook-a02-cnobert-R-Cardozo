using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class MosquitoAttackGame : Game
{
    private const int _WindowWidth = 550, _WindowHeight = 400;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;
    private SpriteFont _font;

    // enum - used to determine states of the game (datatype)
    private enum GameState { Playing, Paused, Over }
    private GameState _state;

    public Cannon _cannon;

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
        _cannon.Initialize(new Vector2(50, 325), 150);

        // Sets state of game on startup
        _state = GameState.Playing;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");
        _font = Content.Load<SpriteFont>("SystemArialFont");
        _cannon.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        
        #region Keyboard Input
        KeyboardState kbState = Keyboard.GetState();

        if (kbState.IsKeyDown(Keys.A))
        {
            _cannon.Direction = new Vector2(-1, 0);
        }else if (kbState.IsKeyDown(Keys.D))
        {
            _cannon.DDirection = new Vector2(1, 0);
        }
        else
        {
            _cannon.DDirection = Vector2.Zero;
        }
        #endregion

        _cannon.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        
        _spriteBatch.Draw(_background, Vector2.Zero, Color.White); // To tint an image, you can use a colour other than white!
        _cannon.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}

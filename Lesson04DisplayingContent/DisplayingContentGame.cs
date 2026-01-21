using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson04DisplayingContent;

public class DisplayingContentGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _spaceStation, _ship;
    private SpriteFont _font;
    private string _output = "String being outputted :)";
    private SimpleAnimation _walkingAnimation;

    public DisplayingContentGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 640;
        _graphics.PreferredBackBufferHeight = 320;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _spaceStation = Content.Load<Texture2D>("Station");
        _ship = Content.Load<Texture2D>("Beetle");

        Texture2D walkingSpriteSheet = Content.Load<Texture2D>("Walking");
        int width = walkingSpriteSheet.Width;
        int height = walkingSpriteSheet.Height;
        _walkingAnimation = new SimpleAnimation(walkingSpriteSheet, width / 8, height / 8, 8, 8);

        // _walkingAnimation = new SimpleAnimation(walkingSpriteSheet, 81, 144, 8, 8);

        _font = Content.Load<SpriteFont>("SystemArialFont");
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_spaceStation, Vector2.Zero, Color.White);
        _spriteBatch.Draw(_ship, new Vector2(200, 140), Color.White);
        _spriteBatch.DrawString(_font, _output, new Vector2(20, 20), Color.Beige);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

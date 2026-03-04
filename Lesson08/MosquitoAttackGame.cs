using System.Drawing;
using System.Net.Mime;
using System.Numerics;
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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");
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
        
        _spriteBatch.Draw(_background, Vector2.Zero, Color.White); // To tint an image, you can use a colour other than white!

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}

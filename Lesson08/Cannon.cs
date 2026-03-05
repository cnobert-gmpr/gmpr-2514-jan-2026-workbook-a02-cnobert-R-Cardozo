using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Cannon
{
    private SimpleAnimation _animation;
    private Vector2 _position, _direction;
    private Point _dimensions;
    private float _speed;

    internal Vector2 Direction
    {
        set
        {
            // Ensure cannon only moves horizontally
            value.Y = 0;
        }
    }

    internal void Initialize(Vector2 position)
    {
        _position = position;
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Cannon");
        _dimensions = new Point(texture.Width / 4, texture.Height);
        _animation = new SimpleAnimation(texture, _dimensions.X, _dimensions.Y, 4, 2);
    }

    internal void Update(GameTime gameTime)
    {
        #region Keyboard Input
        KeyboardState kbState = Keyboard.GetState();

        if (kbState.IsKeyDown(Keys.A))
        {
            Cannon.Direction = new Vector2(-1, 0);
        }else if (kbState.IsKeyDown(Keys.D))
        {
            Cannon.Direction = new Vector2(1, 0);
        }
        else
        {
            Cannon.Direction = Vector2.Zero;
        }
        #endregion

        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        _position += _direction * _speed * dt;

        _animation.Update(gameTime);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if(_animation != null)
        {
            _animation.Draw(spriteBatch, _position, SpriteEffects.None);
        }
    }
}
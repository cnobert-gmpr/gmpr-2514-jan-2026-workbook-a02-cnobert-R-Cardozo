using System.Security.Cryptography;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class Mosquito
{
    private SimpleAnimation _animation;
    private Vector2 _position, _direction;
    private Point _dimensions;
    private float _speed;

    internal Vector2 Direction
    {
        set
        {
            value.Y = 0;

            if(_direction.X < 0)
            {
                _animation.Reverse = true;
            }
            else
            {
                _animation.Reverse = false;
            }
        }
    }

    internal void Initialize(Vector2 position, float speed)
    {
        _position = position;
        _speed = speed;
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Mosquito");
        _dimensions = new Point(texture.Width / 11, texture.Height);
        _animation = new SimpleAnimation(texture, _dimensions.X, _dimensions.Y, 11, 3);
    }

    internal void Update(GameTime gameTime)
    {
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
using System.IO.Pipes;
using System.Runtime.InteropServices;
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
    private int _windowWidth;

    internal Vector2 Direction
    {
        set
        {
            value.Y = 0;
            _direction = value;

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

    internal void Initialize(Vector2 position, float speed, int windowWidth)
    {
        _position = position;
        _speed = speed;

        _direction = new Vector2(1, 0); // Moves mosquito to the right

        _windowWidth = windowWidth;
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

        if(_position.X <= 0 || _position.X + _dimensions.X >= _windowWidth)
        {
            Direction = new Vector2(-_direction.X, 0); // Changes direction of mosquito
        }

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
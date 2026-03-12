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
    private float _speed;

    private Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_animation.FrameDimensions.X,
                (int)_animation.FrameDimensions.Y
            );
        }
    }

    internal void Initialize(Vector2 position, float speed, Vector2 direction, Rectangle gameBoundingBox)
    {
        _position = position;
        _speed = speed;
        _direction = direction;
        _gameBoundingBox = gameBoundingBox;
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Mosquito");

        _animation = new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f);
        _animation.Paused = false;
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        _position += _direction * _speed * dt;

        if(BoundingBox.Left < _gameBoundingBox.Left || BoundingBox.Right > _gameBoundingBox.Right)
        {
            _direction.X *= -1;
        }

        _animation.Update(gameTime);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        _animation.Draw(spriteBatch, _position, SpriteEffects.None);
    }
}
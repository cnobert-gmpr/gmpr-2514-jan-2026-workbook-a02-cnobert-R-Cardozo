using Microsoft.Win32;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace Lesson08_1;

public class Fireball
{
    private SimpleAnimation _animation;
    private Vector2 _position, _direction;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private enum State { Flying, NotFlying };
    private State _state = State.NotFlying;

    internal Rectangle BoundingBox
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, (int)_animation.FrameDimensions.X, (int)_animation.FrameDimensions.Y);
    }

    internal bool Launchable { get => _state == State.NotFlying; }

    internal void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Fireball");

        _animation = new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 8f);
        _animation.Paused = false;
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(_state == State.Flying)
        {
            _position += _direction * _speed *  dt;
            _animation.Update(gameTime);

            if (!_gameBoundingBox.Intersects(BoundingBox))
            {
                _state = State.NotFlying;
            }
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (_state == State.Flying)
        {
            _animation.Draw(spriteBatch, _position, SpriteEffects.None);
        }
    }

    internal void Launch(Vector2 position, Vector2 direction)
    {
        if(_state == State.NotFlying)
        {
            _position = position;
            _direction = direction;
            _state = State.Flying;
        }
    }

    internal bool ProcessFireballCollision(Rectangle otherBoundingBox)
    {
        if(_state == State.Flying && BoundingBox.Intersects(otherBoundingBox))
        {
            _state = State.NotFlying;
            return true;
        }
        return false;
    }
}
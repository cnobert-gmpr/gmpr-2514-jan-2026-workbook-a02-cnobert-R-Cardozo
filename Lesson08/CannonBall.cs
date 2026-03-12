using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class CannonBall{
    private Texture2D _texture;
    private Vector2 _position, _direction;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private enum State { Flying, NotFlying };
    private State _state = State.NotFlying;

    internal Rectangle BoundingBox
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
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
        Texture2D _texture = content.Load<Texture2D>("CannonBall");
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        #region Change state
        switch (_state)
        {
            case State.Flying:
                _position += _direction * _speed * dt;
                break;
            case State.NotFlying:
                break;
        }
        #endregion

    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        #region Change state
        switch (_state)
        {
            case State.Flying:
                spriteBatch.Draw(_texture, _position, Color.White)
                break;
            case State.NotFlying:
                break;
        }
        #endregion
    }
}
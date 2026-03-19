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

    internal bool Launchable { get => _state == State.NotFlying; }

    internal void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _position = Vector2.Zero;
        _direction = Vector2.Zero;
        _speed = speed;
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
                if (!BoundingBox.Intersects(_gameBoundingBox))
                {
                    _state = State.NotFlying;
                }
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
                spriteBatch.Draw(_texture, _position, Color.White);
                break;
            case State.NotFlying:
                break;
        }
        #endregion
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
}
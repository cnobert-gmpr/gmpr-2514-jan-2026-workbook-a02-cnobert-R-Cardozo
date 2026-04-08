using System.Runtime;

namespace Lesson08;

public class Projectile
{
    protected Vector2 _position, _direction;
    protected Point _dimensions;
    protected float _speed;

    protected Rectangle _gameBoundingBox;
    protected enum State { Flying, NotFlying };
    protected State _state = State.NotFlying;

    internal Rectangle BoundingBox
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _dimensions.X, _dimensions.Y);
    }

    internal bool Launchable { get => _state == State.NotFlying; }

    // virtual -> the children of this method MAY override the method, but they don't have to
    internal virtual void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _position = Vector2.Zero;
        _direction = Vector2.Zero;
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;
    }

    internal abstract void LoadContent(ContentManager content);
    internal abstract void Update(GameTime gameTime);
    internal abstract void Draw(SpriteBatch spriteBatch);

    internal void Launch(Vector2 position, Vector2 direction)
    {
        if(_state == State.NotFlying)
        {
            _position = position;
            _direction = direction;
            _state = State.NotFlying;
        }
    }

    internal abstract bool ProcessCollision(Rectangle otherBoundingBox);
}
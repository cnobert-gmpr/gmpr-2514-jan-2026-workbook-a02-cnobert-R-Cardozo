using System.Drawing;
using System.Numerics;

namespace Lesson08;

public class Projectile
{
    protected Vector2 _position, _direction;
    protected float _speed;

    protected Rectangle _gameBoundingBox;

    protected enum State { Flying, NotFlying }
    protected State _state = State.NotFlying;

    internal bool Launchable { get => _state == State.NotFlying; }

    // virtual -> the children of this method MAY override the method, but they don't have to
    internal virtual void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _position = Vector2.Zero;
        _direction = Vector2.Zero;
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;
    }
}
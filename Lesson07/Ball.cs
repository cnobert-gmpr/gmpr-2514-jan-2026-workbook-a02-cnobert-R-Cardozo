using System.Formats.Tar;
using System.Numerics;

namespace Lesson07;

public class Ball
{
    private Vector2 _position, _dimension, _direction;
    private float _speed;

    internal void Initialize(Vector2 position, float speed, Vector2 dimensions, Vector2 direction)
    {
        _position = position;
        _dimensions = dimensions;
        _direction = direction;
        _speed = speed;
    }
}
using System.Formats.Tar;

namespace Lesson07;

public class Ball
{
    private Texture2D _texture;
    private Vector2 _position, _dimension, _direction;
    private float _speed;

    internal void Initialize(Vector2 position, Vector2 dimensions, Vector2 direction, float speed)
    {
        _position = position;
        _dimensions = dimensions;
        _direction = direction;
        _speed = speed;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = Content.Load<Texture2D>("Ball");
    }
}
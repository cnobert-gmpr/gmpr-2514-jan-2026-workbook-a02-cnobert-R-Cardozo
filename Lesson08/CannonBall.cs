

using System.Drawing;

namespace Lesson08;

public class CannonBall{
    private Texture2D _texture;
    private Vector2 _position, _direction;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private enum State { Flying, NotFlying };

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
        _position += _direction * _speed * dt;
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, Color.White);
    }
}
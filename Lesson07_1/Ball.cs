using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Lesson07_1;

public class Ball
{
    private const int _WidthAndHeight = 7;
    private const int _Speed = 45;
    private const int _CollisionTimerIntervalMillis = 400;
    private Texture2D _texture;
    private Vector2 _dimensions, _position, _direction;
    private float _speed;
    private int _collisionTimerMillis;

    private int _gameScale;
    private Rectangle _playAreaBoundingBox;
    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(_position.ToPoint(), _dimensions.ToPoint());
        }
    }
    internal void Initialize(Vector2 initialPosition, Vector2 initialDirection, int gameScale, Rectangle playAreaBoundingBox)
    {
        _position = initialPosition;
        _direction = initialDirection;
        _gameScale = gameScale;
        _playAreaBoundingBox = playAreaBoundingBox;
        _speed = _Speed * _gameScale;
        
        _dimensions = new Vector2(_WidthAndHeight * _gameScale);
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("Ball");
    }

    internal void Update(GameTime gameTime)
    {
        _collisionTimerMillis += gameTime.ElapsedGameTime.Milliseconds;

        _position += _direction * _speed * (float) gameTime.ElapsedGameTime.TotalSeconds;

        #region bounce off left and right walls

        if(_position.X <= _playAreaBoundingBox.Left || (_position.X + _dimensions.X) >= _playAreaBoundingBox.Right)
        {
            _direction.X *= -1;
        }

        #endregion

        #region bounce off top or bottom walls

        if(_position.Y <= _playAreaBoundingBox.Top || (_position.Y + _dimensions.Y) >= _playAreaBoundingBox.Bottom)
        {
            _direction.Y *= -1;
        }

        #endregion
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        Rectangle ballRectangle = new Rectangle((int) _position.X, (int) _position.Y, (int) _dimensions.X, (int) _dimensions.Y);
        spriteBatch.Draw(_texture, ballRectangle, Color.White);
    }

    internal bool ProcessCollision(Rectangle otherBoundingBox)
    {
        bool didCollide = false;
        if(_collisionTimerMillis >= _CollisionTimerIntervalMillis && BoundingBox.Intersects(otherBoundingBox))
        {
            didCollide = true;
            _collisionTimerMillis = 0;

            Rectangle intersection = Rectangle.Intersect(BoundingBox, otherBoundingBox);
            if(intersection.Width > intersection.Height)
            {
                _direction.Y *= -1;
            }
            else
            {
                _direction.X *= -1;
            }
        }
        return didCollide;
    }
}
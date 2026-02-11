using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson07;

public class Ball
{
    private Texture2D _texture;
    private Vector2 _position, _dimensions, _direction;
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
        _texture = content.Load<Texture2D>("Ball");
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        
        #region bounce off left or right walls

        if(_position.X <= PlayAreaBoundingBox.Left || (_position.X + _dimensions.X) >= PlayAreaBoundingBox.Right)
        {
            _direction.X *= -1;
        }

        #endregion

        #region bounce off the top or bottom walls

        if (_position.Y <= PlayAreaBoundingBox.Top || (_position.Y + _dimensions.Y) >= PlayAreaBoundingBox.Bottom)
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
}
using Microsoft.Win32;
using System.Collections.Generic;
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

    private List<Vector2> _trailPositions;
    private float _trailTimer;
    private const float _TrailSpawnInterval = 0.1f;
    private const int _MaxTrailPositions = 8;

    internal Rectangle BoundingBox
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
    }

    internal bool Launchable { get => _state == State.NotFlying; }

    internal override void Initialize(float speed, Rectangle gameBoundingBox)
    {
        _position = Vector2.Zero;
        _direction = Vector2.Zero;
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;

        _trailPositions = new List<Vector2>();
        _trailTimer = 0;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("CannonBall");
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        #region Change state
        switch (_state)
        {
            case State.Flying:
                _position += _direction * _speed * dt;

                _trailTimer += dt;
                if(_trailTimer >= _TrailSpawnInterval)
                {
                    _trailTimer = 0;
                    _trailPositions.Insert(0, _position);
                    if(_trailPositions.Count > _MaxTrailPositions)
                    {
                        _trailPositions.RemoveAt(_trailPositions.Count - 1);
                    }
                }

                if (!BoundingBox.Intersects(_gameBoundingBox))
                {
                    _state = State.NotFlying;
                    _trailPositions.Clear();
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
                DrawTrail(spriteBatch);
                break;
            case State.NotFlying:
                break;
        }
        #endregion
    }

    private void DrawTrail(SpriteBatch spriteBatch)
    {
        for(int c = 0; c < _trailPositions.Count; c++)
        {
            float alpha = 1f - ((float)(c + 1) / (_trailPositions.Count + 1));

            float scale = 1f - (c * 0.1f);
            if(scale < 0.2f)
            {
                scale = 0.2f;
            }

            Vector2 drawPosition = _trailPositions[c];
            Vector2 origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Vector2 centeredPosition = drawPosition + new Vector2(_texture.Width / 2f, _texture.Height / 2f);

            spriteBatch.Draw
                (
                    _texture, centeredPosition, null, Color.Gray * (alpha * 0.5f), 0f, origin, scale, SpriteEffects.None, 0f
                );
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

    internal bool ProcessCollision(Rectangle otherBoundingBox)
    {
        if(_state == State.Flying && BoundingBox.Intersects(otherBoundingBox))
        {
            _state = State.NotFlying;
            _trailPositions.Clear();
            return true;
        }
        return false;
    }
}
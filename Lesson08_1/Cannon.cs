using System.Collections;
using System.Security.Cryptography;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08_1;

public class Cannon
{
    private SimpleAnimation _animationAlive, _animationDying;
    private const int _NumCannonBalls = 10;
    private Vector2 _position, _direction;
    private Point _dimensions;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private CannonBall[] _cBalls;

    internal enum State { Alive, Dying, Dead }
    internal State _state = State.Alive;

    private int _ammo;
    private const int _maxAmmo = 10;
    private float _reloadTime = 2f; // How many seconds it takes to reload
    private float _reloadTimer = 0f;
    private bool _reloading = false;

    // The following properties 'expose' the ammo levels/status to MosquitoAttack
    internal int Ammo => _ammo;
    internal int MaxAmmo => _maxAmmo;
    internal bool IsReloading => _reloading;

    internal Vector2 Direction
    {
        set
        {
            // Ensures cannon can only move horizontally
            value.Y = 0;
            _direction = value;

            if(_direction.X < 0)
            {
                _animationAlive.Reverse = true;
            }

            else
            {
                _animationAlive.Reverse = false;
            }
        }
    }

    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_animationAlive.FrameDimensions.X,
                (int)_animationAlive.FrameDimensions.Y
            );
        }
    }

    internal void Initialize(Vector2 position, float speed, Rectangle gameBoundingBox)
    {
        _position = position;
        _speed = speed;
        _gameBoundingBox = gameBoundingBox;

        _ammo = _maxAmmo;
        _reloading = false;
        _reloadTimer = 0f;

        _cBalls = new CannonBall[_NumCannonBalls];

        for(int c = 0; c < _NumCannonBalls; c++)
        {
            _cBalls[c] = new CannonBall();
            _cBalls[c].Initialize(50, _gameBoundingBox);
        }
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D textureAlive = content.Load<Texture2D>("Cannon");
        _dimensions = new Point(textureAlive.Width / 4, textureAlive.Height);
        _animationAlive = new SimpleAnimation(textureAlive, _dimensions.X, _dimensions.Y, 4, 2);

        Texture2D textureDying = content.Load<Texture2D>("Poof");
        _animationDying = new SimpleAnimation(textureDying, textureDying.Width / 8, textureDying.Height, 8, 4);
        _animationDying.Looping = false;

        foreach(CannonBall c in _cBalls)
        {
            c.LoadContent(content);
        }
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        if (_reloading)
        {
            _reloadTimer += dt;
            if(_reloadTimer >= _reloadTime)
            {
                _ammo = _maxAmmo;
                _reloading = false;
                _reloadTimer = 0;
            }
        }

        switch (_state)
        {
            case State.Alive:
                _position += _direction * _speed * dt;

                if(_direction != Vector2.Zero)
                {
                    _animationAlive?.Update(gameTime);
                }

                if (_position.X <= _gameBoundingBox.Left)
                {
                    _position.X = _gameBoundingBox.Left;
                }
                else if (BoundingBox.Right >= _gameBoundingBox.Right)
                {
                    _position.X = _gameBoundingBox.Right - BoundingBox.Width;
                }

                break;
            
            case State.Dying:
            _animationDying.Update(gameTime);
            if (_animationDying.DonePlayingOnce)
                {
                    _state = State.Dead;
                }

                break;
            
            case State.Dead:
                break;
        }
    
        foreach(CannonBall c in _cBalls)
        {
            c.Update(gameTime);
        }

        
    }

    internal void Draw(SpriteBatch spriteBatch, Color tint)
    {
        switch (_state)
        {
            case State.Alive:
                _animationAlive.Draw(spriteBatch, _position, SpriteEffects.None, tint);

                break;
            
            case State.Dying:
                Vector2 cannonCenter = new Vector2(
                        _position.X + _animationAlive.FrameDimensions.X / 2,
                        _position.Y + _animationAlive.FrameDimensions.Y / 2
                );

                Vector2 dyingPosition = new Vector2(
                    cannonCenter.X - _animationDying.FrameDimensions.X / 2,
                    cannonCenter.Y - _animationDying.FrameDimensions.Y / 2
                );

                _animationDying.Draw(spriteBatch, dyingPosition, SpriteEffects.None, tint);

                break;

             case State.Dead:

                break;
        }

        foreach(CannonBall c in _cBalls)
        {
            c.Draw(spriteBatch);
        }
    }

    internal void Die()
    {
        if (_state == State.Alive)
        {
            _state = State.Dying;
            _animationAlive.Looping = false;
        }
    }

    internal void Shoot()
    {
        if (_state != State.Alive || _ammo <= 0 || _reloading) return; // Prevents shooting while dying/dead, if the ammo is out, or if actively reloading cannon

        foreach(CannonBall c in _cBalls)
        {
            if (c.Launchable)
            {
                float cannonBallPositionX = BoundingBox.Center.X - c.BoundingBox.Width / 2;
                float cannonBallPositionY = BoundingBox.Top - c.BoundingBox.Height;
                Vector2 cannonBallPosition = new Vector2(cannonBallPositionX, cannonBallPositionY);
                c.Launch(cannonBallPosition, new Vector2(0, -1));
                _ammo--;
                return;
            }
        }
    }

    internal void Reload()
    {
        if(!_reloading && _ammo < _maxAmmo)
        {
            _reloading = true;

            foreach(var c in _cBalls)
            {
                c.Reset();
            }
        }
    }

    internal bool ProcessCollision(Rectangle boundingBox)
    {
        foreach(CannonBall c in _cBalls)
        {
            if (c.ProcessCollision(boundingBox))
            {
                return true;
            }
        }
        return false;
    }
}
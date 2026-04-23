using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08_1;

public class Mosquito
{
    private SimpleAnimation _animationAlive, _animationDying;
    private Vector2 _position, _direction;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private Fireball[] _fireballs;
    private const int _NumFireBalls = 3;
    private Random _random = new Random();
    private float _timeSinceLastSwoop = 0f, _swoopCooldown;

    private enum State { Alive, Dying, Dead };
    private State _state;

    private enum MovementState { Still, Swooping }
    private MovementState _movementState = MovementState.Still;
    private float _swoopSpeed = 125f; // Mosquito swooping speed
    private Vector2 _swoopTarget;

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

    internal bool Alive { get => _state == State.Alive; }

    internal void Initialize(Vector2 position, float speed, Vector2 direction, Rectangle gameBoundingBox)
    {
        _position = position;
        _speed = speed;
        _direction = direction;
        _gameBoundingBox = gameBoundingBox;
        _state = State.Alive;

        _fireballs = new Fireball[_NumFireBalls];

        for(int i = 0; i < _NumFireBalls; i++)
        {
            _fireballs[i] = new Fireball();
            _fireballs[i].Initialize(200, _gameBoundingBox);
        }
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Mosquito");

        _animationAlive = new SimpleAnimation(texture, texture.Width / 11, texture.Height, 11, 8f);
        _animationAlive.Paused = false;

        texture = content.Load<Texture2D>("Poof");
        _animationDying = new SimpleAnimation(texture, texture.Width / 8, texture.Height, 8, 4);

        foreach(Fireball f in _fireballs)
        {
            f.LoadContent(content);
        }
    }

    internal void Update(GameTime gameTime)
    {

        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        switch (_state)
        {
            case State.Alive:
                _swoopCooldown = 3f + (float)_random.NextDouble() * 4f;

                if(BoundingBox.Left < _gameBoundingBox.Left || BoundingBox.Right > _gameBoundingBox.Right)
                {
                    _direction.X *= -1;
                }

                _animationAlive.Update(gameTime);

                _timeSinceLastSwoop += dt;

                if (_movementState == MovementState.Still && _timeSinceLastSwoop >= _swoopCooldown)
                {
                    float randomX = _gameBoundingBox.Left + (float)_random.NextDouble() * _gameBoundingBox.Width;
                    float swoopY = _position.Y + 100; // Swoops down by 100 pixels

                    _swoopTarget = new Vector2(randomX, swoopY);
                    _movementState = MovementState.Swooping;
                    _timeSinceLastSwoop = 0f;
                }

                if (_movementState == MovementState.Swooping)
                {
                    Vector2 direction = _swoopTarget - _position;
                    if (direction.Length() < 5f) // Checks whether the 'target' has been reached
                    {
                        direction.Normalize();
                        _movementState = MovementState.Still;
                    }
                    else
                    {
                        // Normalize() ensures the mosquitos move at a consistent speed when swooping, regardless where the target is
                        direction.Normalize();
                        _position += direction * _swoopSpeed * dt;
                    }
                }
                else
                {
                    _position += _direction * _speed * dt;
                }

                foreach(Fireball f in _fireballs)
                {
                    f.Update(gameTime);
                }

                if(_random.NextDouble() < 0.005) // Sets a 0.5% chance per frame to shoot
                {
                    ShootFireball();
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
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        switch (_state)
        {
            case State.Alive:
                _animationAlive.Draw(spriteBatch, _position, SpriteEffects.None);

                foreach(Fireball f in _fireballs){
                    f.Draw(spriteBatch);
                }

                break;
            
            case State.Dying:
                _animationDying.Draw(spriteBatch, _position, SpriteEffects.None);

                foreach(Fireball f in _fireballs){
                    f.Draw(spriteBatch);
                }

                break;
            
            case State.Dead:

                break;
        }
    }

    internal void Die()
    {
        if (Alive)
        {
            _state = State.Dying;
            _animationDying.Looping = false;
        }
    }

    internal void ShootFireball()
    {
        foreach(Fireball f in _fireballs)
        {
            if (f.Launchable)
            {
                Vector2 fireballPosition = new Vector2(
                    BoundingBox.Center.X - 10,
                    BoundingBox.Bottom
                );

                f.Launch(fireballPosition, new Vector2(0, 1)); // Moves fireballs down rather than up
                return;
            }
        }
    }

    internal bool ProcessFireballCollision(Rectangle target)
    {
        foreach(Fireball f in _fireballs)
        {
            if (f.ProcessFireballCollision(target))
            {
                return true;
            }
        }
        return false;
    }
}
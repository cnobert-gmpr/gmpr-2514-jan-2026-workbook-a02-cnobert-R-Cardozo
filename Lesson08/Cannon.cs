using System.Security.Cryptography;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lesson08;

public class Cannon
{
    private const int _NumProjectiles = 5;
    private SimpleAnimation _animationAlive, _animationDying;
    private Vector2 _position, _direction;
    private Point _dimensions;
    private float _speed;
    private Rectangle _gameBoundingBox;

    private Projectile[] _projectiles;

    internal Vector2 Direction
    {
        set
        {
            // Ensure cannon only moves horizontally
            value.Y = 0;
            _direction = value;

            // Reverses animation of cannon based on direction it's moving
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

        _projectiles = new Projectile[_NumProjectiles];
        _projectiles[0] = new CannonBall();
        _projectiles[1] = new FireBall();
        _projectiles[2] = new CannonBall();
        _projectiles[3] = new FireBall();
        _projectiles[4] = new CannonBall();

        for(int p = 0; p < _NumProjectiles; p++)
        {
            _projectiles[p].Initialize(50, _gameBoundingBox);
        }
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D texture = content.Load<Texture2D>("Cannon");
        _dimensions = new Point(texture.Width / 4, texture.Height);
        _animationAlive = new SimpleAnimation(texture, _dimensions.X, _dimensions.Y, 4, 2);

        foreach(Projectile p in _projectiles)
        {
            p.LoadContent(content);
        }
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;
        _position += _direction * _speed * dt;

        if(_direction != Vector2.Zero)
        {
            _animationAlive.Update(gameTime);
        }

        foreach(Projectile p in _projectiles)
        {
            p.Update(gameTime);
        }

    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if(_animationAlive != null)
        {
            _animationAlive.Draw(spriteBatch, _position, SpriteEffects.None);
        }

        foreach(Projectile p in _projectiles)
        {
            p.Draw(spriteBatch);
        }
    }

    internal void Shoot()
    {
        foreach(Projectile p in _projectiles)
        {
            if (p.Launchable)
            {
                float projectilePositionX = BoundingBox.Center.X - p.BoundingBox.Width / 2;
                float projectilePositionY = BoundingBox.Top - p.BoundingBox.Height;
                Vector2 projectilePosition = new Vector2(projectilePositionX, projectilePositionY);
                p.Launch(projectilePosition, new Vector2(0, -1));
                return;
            }
        }
    }

    internal bool ProcessCollision(Rectangle boundingBox)
    {
        foreach(Projectile p in _projectiles)
        {
            if (p.ProcessCollision(boundingBox))
            {
                return true;
            }
        }
        return false;
    }
}
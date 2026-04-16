using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.ContentManager;
using System.Collections;

namespace Lesson09;

public class Player
{
    private const int _Speed = 150;
    private enum State { Idle, Walking, Jumping }
    private State _state;

    private SimpleAnimation _animationIdle, _animationJump, _animationWalk, _animationCurrent;

    private Vector2 _position, _velocity, _dimensions;
    
    private Rectangle _gameBoundingBox;

    internal Rectangle BoundingBox
    {
        get {return new Rectangle((int)_position.X, (int)_position.Y, (int)_dimensions.X, (int)_dimensions.Y);}
    }

    public Player(Vector2 position, Rectangle gameBoundingBox)
    {
        _position = position;
        _gameBoundingBox = gameBoundingBox;
        _dimensions = new Vector2(46, 40);
    }

    internal void Initialize()
    {
        _state = State.Idle;
    }

    internal void LoadContent(ContentManager content)
    {
        // Idle: cells 30 px wide, 1/8 s per frame => 8 fps
        Texture2D idleTexture = content.Load<Texture2D>("Idle");
        int idleFrameWidth = 30;
        int idleFrameHeight = idleTexture.Height;
        int idleFrameCount = idleTexture.Width / idleFrameWidth;
        _animationIdle = new SimpleAnimation(idleTexture, idleFrameWidth, idleFrameHeight, idleFrameCount, 8f);

        // Walk: cells 35 px wide, 1/8 s per frame => 8 fps
        Texture2D walkTexture = content.Load<Texture2D>("Walk");
        int walkFrameWidth = 35;
        int walkFrameHeight = walkTexture.Height;
        int walkFrameCount = walkTexture.Width / walkFrameWidth;
        _animationWalk = new SimpleAnimation(walkTexture, walkFrameWidth, walkFrameHeight, walkFrameCount, 8f);

        // Jump: cells 30 px wide, 1/8 s per frame => 8 fps
        Texture2D jumpTexture = content.Load<Texture2D>("JumpOne");
        int jumpFrameWidth = 30;
        int jumpFrameHeight = jumpTexture.Height;
        int jumpFrameCount = jumpTexture.Width / jumpFrameWidth;
        _animationJump = new SimpleAnimation(jumpTexture, jumpFrameWidth, jumpFrameHeight, jumpFrameCount, 8f);

        // After loading, make sure Initialize will have something to use
        _animationCurrent = _animationIdle;
    }

    internal void Update(GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        _animationCurrent?.Update(gameTime); // Essentially checks if -> _animationCurrent != null

        _position += _velocity * dt;

        switch(_state)
        {
            case State.Jumping:

                break;
                
            case State.Idle:

                break;
                
            case State.Walking:

                break;
                
        }
    }
}
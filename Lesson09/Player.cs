using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
}
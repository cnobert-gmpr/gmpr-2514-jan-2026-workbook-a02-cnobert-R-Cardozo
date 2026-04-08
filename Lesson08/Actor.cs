using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Actor
{
    protected SimpleAnimation _animationAlive, _animationDying;
    protected Vector2 _position, _direction;
    protected Point _dimensions;
    protected float _speed;
}
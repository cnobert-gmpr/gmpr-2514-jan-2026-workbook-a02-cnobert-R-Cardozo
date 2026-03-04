using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson08;

public class Cannon
{
    private SimpleAnimation _animation;
    private Vector2 _position;
    private Point _dimensions;

    internal void Initialize(Vector2 position)
    {
        _position = position;
    }

    internal void LoadContent(ContentManager content)
    {
        Texture2D _texture = content.Load<Texture2D>("Cannon");
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson09Platformer;

public class Collider
{
    public enum ColliderType { Top, Right, Bottom, Left }
    private ColliderType _type;

    private Vector2 _position, _dimensions;
    private Texture2D _pixel;
    internal Rectangle BoundingBox
    {
        get
        {
            return new Rectangle((int)_position.X, (int)_position.Y, (int)_dimensions.X, (int)_dimensions.Y);
        }
    }
    public Collider(Vector2 position, Vector2 dimensions, ColliderType colliderType)
    {
        _position = position;
        _dimensions = dimensions;
        _type = colliderType;
    }
}
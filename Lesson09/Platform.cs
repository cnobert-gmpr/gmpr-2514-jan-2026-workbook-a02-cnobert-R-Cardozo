using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Runtime.CompilerServices;

namespace Lesson09;

public class Platform
{
    private Vector2 _position, _dimensions;
    private Collider[] _colliders;

    public Platform(Vector2 position, Vector2 dimensions)
    {
        _position = position;
        _dimensions = dimensions;

        _colliders = new Collider[4];
        _colliders[0] = new Collider(new Vector2(position.X + 3, position.Y), new Vector2(dimensions.X - 6, 1), ColliderType.Top);
        _colliders[1] = new Collider(new Vector2(position.X + dimensions.X - 1, position.Y + 1), new Vector2(1, dimensions.Y - 2), ColliderType.Right);
        _colliders[2] = new Collider(new Vector2(position.X + 3, position.Y + dimensions.Y), new Vector2(dimensions.X - 6, 1), ColliderType.Bottom);
        _colliders[3] = new Collider(new Vector2(position.X + 1, position.Y + 1), new Vector2(1, dimensions.Y - 2), ColliderType.Left);
    }
}
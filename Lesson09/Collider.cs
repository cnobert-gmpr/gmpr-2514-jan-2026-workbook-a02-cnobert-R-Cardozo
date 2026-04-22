using System.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lesson09;
public enum ColliderType { Top, Right, Bottom, Left }

public class Collider
{
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

    internal void LoadContent(GraphicsDevice graphicsDevice)
    {
        Color myColour = Color.White;
        switch(_type)
        {
            case ColliderType.Left:
                myColour = Color.Red;
                break;
            case ColliderType.Top:
                myColour = Color.Blue;
                break;
            case ColliderType.Right:
                myColour = Color.Wheat;
                break;
            case ColliderType.Bottom:
                myColour = Color.Purple;
                break;
        }

        if(_pixel == null)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { myColour });
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_pixel, BoundingBox, Color.White);
    }

    internal void ProcessCollision(Player player, GameTime gameTime)
    {
        float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        if (BoundingBox.Intersects(player.BoundingBox))
        {
            switch(_type)
            {
                case ColliderType.Left:
                    if(player.Velocity.X > 0)
                        player.MoveHorizontally(0);

                    break;

                case ColliderType.Top:
                    player.Land(BoundingBox);
                    player.StandOn(BoundingBox, dt);

                    break;

                case ColliderType.Right:
                    if(player.Velocity.X < 0)
                        player.MoveHorizontally(0);

                    break;

                case ColliderType.Bottom:


                    break;

            }
        }
    }
}
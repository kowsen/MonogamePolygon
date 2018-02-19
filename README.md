# MonogamePolygon
Draw filled convex polygons in Monogame. Also allows you to inflate / deflate polygons (useful for adding borders).

## Usage
``` csharp
var triangle = new List<Vector2>()
{
    new Vector2(50, 0),
    new Vector2(0, 100),
    new Vector2(100, 100)
};

var artist = new Artist(GraphicsDevice, GraphicsDevice.Viewport.Bounds);

// outer border
artist.DrawPolygon(triangle, Color.Blue, new Vector2(220, 275), -8);

// middle border
artist.DrawPolygon(triangle, Color.Black, new Vector2(220, 275), 0);

// inner filling
artist.DrawPolygon(triangle, Color.Red, new Vector2(220, 275), 8);
```

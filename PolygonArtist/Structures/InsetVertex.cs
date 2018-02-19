using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonArtist.Structures
{
    internal struct InsetVertex
    {
        public Vector2 Outer { get; private set; }
        public Vector2 Inner { get; private set; }
        public Vector2 Previous { get; private set; }
        public Vector2 Next { get; private set; }
        public bool IsValid { get; private set; }

        public InsetVertex(Vector2 outer, Vector2 previous, Vector2 next, double thickness)
        {
            Outer = outer;
            Previous = previous;
            Next = next;

            var toPrev = previous - outer;
            var toNext = next - outer;
            toPrev.Normalize();
            toNext.Normalize();

            var middle = toPrev + toNext;

            var magnitude = thickness / Math.Sqrt(1 - Math.Pow((toNext.X * middle.X + toNext.Y * middle.Y) / (toNext.Length() * middle.Length()), 2));

            if (double.IsNaN(magnitude) || double.IsInfinity(magnitude))
            {
                IsValid = false;
                Inner = Vector2.Zero;
                return;
            }

            middle.Normalize();
            Inner = outer + (middle * (float)magnitude);
            IsValid = true;
        }

        public InsetVertex(Vector2 inner, bool isValid = true)
        {
            Outer = Vector2.Zero;
            Inner = inner;
            Previous = Vector2.Zero;
            Next = Vector2.Zero;
            IsValid = isValid;
        }
    }
}

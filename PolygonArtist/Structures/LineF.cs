using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonArtist.Structures
{
    internal struct LineF
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }
        public float Slope { get; private set; }
        public float Intercept { get; private set; }

        public LineF(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
            Slope = (End.Y - Start.Y) / (End.X - Start.X);
            Intercept = End.Y - Slope * End.X;
        }

        public Vector2 Intersect(LineF other)
        {
            float x, y;
            if (Slope == other.Slope)
            {
                x = float.NaN;
                y = float.NaN;
            }
            else if (float.IsInfinity(Slope))
            {
                x = Start.X;
                y = other.Slope * x + other.Intercept;
            }
            else if (float.IsInfinity(other.Slope))
            {
                x = other.Start.X;
                y = Slope * x + Intercept;
            }
            else
            {
                x = (other.Intercept - Intercept) / (Slope - other.Slope);
                y = Slope * x + Intercept;
            }

            return new Vector2(x, y);
        }

        public float Distance(Vector2 point)
        {
            if (float.IsInfinity(Slope))
            {
                return Math.Abs(point.X - Start.X);
            }
            return Math.Abs((Slope * point.X - point.Y + Intercept) / (float)Math.Sqrt(Slope * Slope + 1));
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PolygonArtist.Structures;

namespace PolygonArtist
{
    public class Artist
    {
        private GraphicsDevice _device;
        private Rectangle _bounds;
        private BasicEffect _basicEffect;

        private double _doubleCheckCushion = 0.001;

        private VertexPositionColor[] _vertexStorage;
        private InsetVertex[] _polyStorage;

        public Artist(GraphicsDevice device, Rectangle bounds)
        {
            _device = device;
            _bounds = bounds;
            _basicEffect = new BasicEffect(_device);
            _basicEffect.VertexColorEnabled = true;
        }

        public List<Vector2> InsetPolygon(List<Vector2> polygon, float inset = 0f)
        {
            if (_vertexStorage == null || _vertexStorage.Length < polygon.Count)
            {
                _vertexStorage = new VertexPositionColor[polygon.Count];
            }
            var length = PopulatePolyStorage(polygon, inset);

            var insetPolygon = new List<Vector2>();
            for (var i = 0; i < length; i++)
            {
                insetPolygon.Add(_polyStorage[i].Inner);
            }

            return insetPolygon;
        }

        public void DrawPolygon(List<Vector2> polygon, Color color, float opacity = 1f, Vector2 offset = new Vector2(), float inset = 0f)
        {
            if (_vertexStorage == null || _vertexStorage.Length < polygon.Count)
            {
                _vertexStorage = new VertexPositionColor[polygon.Count];
            }
            var length = PopulatePolyStorage(polygon, inset);

            if (length >= 3)
            {
                GenerateTrianglesFromPolyStorage(length, color, _vertexStorage, offset, _bounds, inset);
                _basicEffect.Alpha = opacity;
                var passes = _basicEffect.CurrentTechnique.Passes;
                for (var i = 0; i < passes.Count; i++)
                {
                    passes[i].Apply();
                    _device.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertexStorage, 0, length - 2);
                }
            }
        }

        private int GenerateTrianglesFromPolyStorage(int storageLength, Color color, VertexPositionColor[] vertices, Vector2 offset, Rectangle bounds, float thickness)
        {
            float bbw = bounds.Width;
            float bbh = bounds.Height;

            var i = 1;
            var j = storageLength - 1;
            bool left = false;
            short index = 0;

            AddPoint(_polyStorage[0].Inner);

            while (i <= j)
            {
                if (left)
                {
                    AddPoint(_polyStorage[i].Inner);
                    i++;
                }
                else
                {
                    AddPoint(_polyStorage[j].Inner);
                    j--;
                }
                left = !left;
            }

            return storageLength;

            void AddPoint(Vector2 point)
            {
                float x = -(((bbw / 2f) - (point.X + offset.X)) / (bbw / 2f));
                float y = (((bbh / 2f) - (point.Y + offset.Y)) / (bbh / 2f));
                vertices[index] = new VertexPositionColor(new Vector3(x, y, 0f), color);
                index += 1;
            }
        }

        private int PopulatePolyStorage(List<Vector2> polygon, double thickness = 0)
        {
            if (polygon.Count < 3)
            {
                return 0;
            }

            if (_polyStorage == null || _polyStorage.Length < polygon.Count)
            {
                _polyStorage = new InsetVertex[polygon.Count];
            }

            if (Math.Abs(thickness) < _doubleCheckCushion)
            {
                for (var i = 0; i < polygon.Count; i++)
                {
                    _polyStorage[i] = new InsetVertex(polygon[i]);
                }
                return polygon.Count;
            }

            var storageLength = 0;
            for (var i = 0; i < polygon.Count; i++)
            {
                var current = polygon[i];
                var prevPoint = polygon[(i - 1 + polygon.Count) % polygon.Count];
                var nextPoint = polygon[(i + 1) % polygon.Count];

                var lineToInner = new InsetVertex(current, prevPoint, nextPoint, thickness);

                if (storageLength > 0 && thickness > 0 && DoesIntersect(_polyStorage[storageLength - 1], lineToInner))
                {
                    _polyStorage[storageLength - 1] = CombineVectorLines(_polyStorage[storageLength - 1], lineToInner, thickness);
                }
                else
                {
                    _polyStorage[storageLength] = lineToInner;
                    storageLength += 1;
                }
            }

            if (thickness > 0)
            {
                var numInvalidated = 0;
                for (var i = 0; i < storageLength; i++)
                {
                    var current = _polyStorage[i];
                    var prevIndex = (i - 1 + storageLength) % storageLength;
                    var prev = _polyStorage[prevIndex];
                    if (DoesIntersect(prev, current))
                    {
                        _polyStorage[i] = CombineVectorLines(prev, current, thickness);
                        // Optimizing for the most common case
                        if (prevIndex == 0)
                        {
                            storageLength -= 1;
                        }
                        else
                        {
                            _polyStorage[prevIndex] = new InsetVertex(Vector2.Zero, false);
                            numInvalidated += 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (storageLength - numInvalidated < 3)
                {
                    return 0;
                }

                if (numInvalidated > 0)
                {
                    for (var i = 0; i < storageLength; i++)
                    {
                        if (!_polyStorage[i].IsValid)
                        {
                            for (var j = i + 1; j < storageLength; j++)
                            {
                                _polyStorage[j - 1] = _polyStorage[j];
                            }
                            storageLength -= 1;
                            i -= 1;
                        }
                    }
                }
            }

            return storageLength;
        }

        private InsetVertex CombineVectorLines(InsetVertex prev, InsetVertex next, double thickness)
        {
            var prevLine = new LineF(prev.Previous, prev.Outer);
            var nextLine = new LineF(next.Outer, next.Next);
            var newOuter = prevLine.Intersect(nextLine);
            return new InsetVertex(newOuter, prev.Previous, next.Next, thickness);
        }

        private bool DoesIntersect(InsetVertex one, InsetVertex two)
        {
            return !IsSameSign(Cross(one.Inner - one.Outer, two.Outer - one.Inner), Cross(one.Inner - one.Outer, two.Inner - one.Inner)) &&
                   !IsSameSign(Cross(two.Inner - two.Outer, one.Outer - two.Inner), Cross(two.Inner - two.Outer, one.Inner - two.Inner));
        }

        private bool IsSameSign(float crossOne, float crossTwo)
        {
            return crossOne * crossTwo > 0;
        }

        private float Cross(Vector2 start, Vector2 end)
        {
            return start.X * end.Y - start.Y * end.X;
        }
    }
}
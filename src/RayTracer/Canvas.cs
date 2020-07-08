using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Canvas : IEnumerable<Color>
    {
        Color[,] canvas;

        public int Width { get; }
        public int Height { get; }

        public Canvas(int width, int height) : this(width, height, Color.Black) { }

        public Canvas(int width, int height, Color initializeColor)
        {
            Width = width;
            Height = height;

            canvas = new Color[height, width];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    canvas[y, x] = initializeColor;
                }
            }
        }

        public Color this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < Width &&
                    y >= 0 && y < Height)
                {
                    return canvas[y, x];
                }

                return null;
            }
            set
            {
                if (x >= 0 && x < Width &&
                    y >= 0 && y < Height)
                {
                    canvas[y, x] = value;
                }
            }
        }

        public IEnumerator<Color> GetEnumerator()
        {
            foreach (var color in canvas)
            {
                yield return color;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

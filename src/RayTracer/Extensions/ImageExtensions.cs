using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Drawing;
using System.IO;

namespace RayTracer.Extensions
{
    public static class ImageExtensions
    {
        public static System.Drawing.Image ToImage(this Canvas canvas)
        {
            var image = new Bitmap(canvas.Width, canvas.Height);

            for (var x = 0; x < canvas.Width; x++)
            {
                for (var y = 0; y < canvas.Height; y++)
                {
                    image.SetPixel(x, y, canvas[x, y].ToDrawingColor());
                }
            }

            return image;
        }

        public static string ToBase64Image(this Canvas canvas)
        {
            string base64 = null;

            using (var memoryStream = new MemoryStream())
            {
                using (var image = new Image<Rgba32>(canvas.Width, canvas.Height))
                {
                    for (var x = 0; x < canvas.Width; x++)
                    {
                        for (var y = 0; y < canvas.Height; y++)
                        {
                            var c = image[1, 2];
                            image[x, y] = canvas[x, y].ToImageSharpColor();
                        }
                    }

                    image.SaveAsPng(memoryStream);
                    base64 = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            return $"data:image/png;base64,{base64}";
        }
    }
}

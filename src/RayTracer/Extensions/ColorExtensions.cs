using SixLabors.ImageSharp.PixelFormats;

namespace RayTracer.Extensions
{
    public static class ColorExtensions
    {
        public static System.Drawing.Color ToDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.Red.To255Byte(), color.Green.To255Byte(), color.Blue.To255Byte());
        }

        public static Rgba32 ToImageSharpColor(this Color color)
        {
            return new Rgba32(color.Red.To255Byte(), color.Green.To255Byte(), color.Blue.To255Byte());
        }
    }
}

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Extensions
{
    public static class PpmExtensions
    {
        public static async Task ToPpmAsync(this Canvas canvas, Stream stream)
        {
            const int maxLineLength = 70;

            using (var streamWriter = new StreamWriter(stream))
            {
                await streamWriter.WriteLineAsync("P3");
                await streamWriter.WriteLineAsync($"{canvas.Width} {canvas.Height}");
                await streamWriter.WriteLineAsync("255");

                for (var y = 0; y < canvas.Height; y++)
                {
                    var lineLength = 0;
                    for (var x = 0; x < canvas.Width; x++)
                    {
                        var color = canvas[x, y];
                        var red = color.Red.To255Byte().ToString();
                        var green = color.Green.To255Byte().ToString();
                        var blue = color.Blue.To255Byte().ToString();

                        if (x == 0)
                        {
                            await streamWriter.WriteAsync(red);
                            lineLength += red.Length;
                        }
                        else
                        {
                            lineLength = await WriteColorComponent(streamWriter, maxLineLength, lineLength, red);
                        }
                        lineLength = await WriteColorComponent(streamWriter, maxLineLength, lineLength, green);
                        lineLength = await WriteColorComponent(streamWriter, maxLineLength, lineLength, blue);
                    }
                    await streamWriter.WriteLineAsync();
                }
            }

        }

        private static async Task<int> WriteColorComponent(StreamWriter streamWriter, int maxLineLength, int lineLength, string red)
        {
            lineLength += 1 + red.Length;
            if (lineLength > maxLineLength)
            {
                await streamWriter.WriteLineAsync();
                lineLength = red.Length;
            }
            else
            {
                await streamWriter.WriteAsync(" ");
            }
            await streamWriter.WriteAsync(red);

            return lineLength;
        }

        public static async Task<string> ToPpmAsync(this Canvas canvas)
        {
            var memoryStream = new MemoryStream();
            await canvas.ToPpmAsync(memoryStream);

            return Encoding.Default.GetString(memoryStream.ToArray());
        }
    }
}

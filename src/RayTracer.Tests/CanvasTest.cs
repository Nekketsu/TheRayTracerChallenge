using RayTracer.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RayTracer.Tests
{
    public class CanvasTest
    {
        [Fact]
        public void CreatingACanvas()
        {
            var c = new Canvas(10, 20);

            Assert.Equal(10, c.Width);
            Assert.Equal(20, c.Height);

            Assert.All(c, color => Assert.Equal(Color.Black, color));
        }

        [Fact]
        public void WritingPixelsToACanvas()
        {
            var c = new Canvas(10, 20);
            var red = new Color(1, 0, 0);

            c[2, 3] = red;

            Assert.Equal(red, c[2, 3]);
        }

        [Fact]
        public async Task ConstructingThePpmHeader()
        {
            var c = new Canvas(5, 3);
            var ppm = await c.ToPpmAsync();

            var lines = ppm.Split(Environment.NewLine);
            var actualLines1To3 = string.Join(Environment.NewLine, lines[..3]);

            var expectedLines1To3 =
@"P3
5 3
255";

            Assert.Equal(expectedLines1To3, actualLines1To3);
        }

        [Fact]
        public async Task ConstructingThePpmPixelData()
        {
            var c = new Canvas(5, 3);
            var c1 = new Color(1.5, 0, 0);
            var c2 = new Color(0, 0.5, 0);
            var c3 = new Color(-0.5, 0, 1);

            c[0, 0] = c1;
            c[2, 1] = c2;
            c[4, 2] = c3;

            var ppm = await c.ToPpmAsync();

            var lines = ppm.Split(Environment.NewLine);
            var actualLines4To6 = string.Join(Environment.NewLine, lines[3..6]);
            var expectedLines4To6 =
@"255 0 0 0 0 0 0 0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 128 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0 0 0 0 0 0 0 255";

            Assert.Equal(expectedLines4To6, actualLines4To6);
        }

        [Fact]
        public async Task SplittingLongLinesInPpmFiles()
        {
            var c = new Canvas(10, 2, new Color(1, 0.8, 0.6));
            var ppm = await c.ToPpmAsync();

            var lines = ppm.Split(Environment.NewLine);
            var actualLines4To7 = string.Join(Environment.NewLine, lines[3..7]);
            var expectedLines4To7 =
@"255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153";

            Assert.Equal(expectedLines4To7, actualLines4To7);
        }

        [Fact]
        public async Task PpmFilesAreTerminatedByANewLineCharacter()
        {
            var c = new Canvas(5, 3);
            var ppm = await c.ToPpmAsync();

            Assert.EndsWith(Environment.NewLine, ppm);
        }
    }
}

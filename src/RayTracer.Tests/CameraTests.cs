using RayTracer.Extensions;
using RayTracer.Matrices;
using RayTracer.Tuples;
using System;
using Xunit;

namespace RayTracer.Tests
{
    public class CameraTests
    {
        [Fact]
        public void ConstructingACamera()
        {
            var hSize = 160;
            var vSize = 120;
            var fieldOfView = Math.PI / 2;

            var c = new Camera(hSize, vSize, fieldOfView);

            Assert.Equal(160, c.HorizontalSize);
            Assert.Equal(120, c.VerticalSize);
            Assert.Equal(Math.PI / 2, c.FieldOfView);
            Assert.Equal(Matrix.Identity, c.Transform);
        }

        [Fact]
        public void ThePixelSizeForAHorizontalCanvas()
        {
            var c = new Camera(200, 125, Math.PI / 2);

            Assert.True(0.01.EqualsEpsilon(c.PixelSize));
        }

        [Fact]
        public void ThePixelSizeForAVerticalCanvas()
        {
            var c = new Camera(125, 200, Math.PI / 2);

            Assert.True(0.01.EqualsEpsilon(c.PixelSize));
        }

        [Fact]
        public void ConstructingARayThroughTheCenterOfTheCanvas()
        {
            var c = new Camera(201, 101, Math.PI / 2);

            var r = c.RayForPixel(100, 50);

            Assert.Equal(new Point(0, 0, 0), r.Origin);
            Assert.Equal(new Vector(0, 0, -1), r.Direction);
        }

        [Fact]
        public void ConstructingARayThroughACornerOfTheCanvas()
        {
            var c = new Camera(201, 101, Math.PI / 2);

            var r = c.RayForPixel(0, 0);

            Assert.Equal(new Point(0, 0, 0), r.Origin);
            Assert.Equal(new Vector(0.66519, 0.33259, -0.66851), r.Direction);
        }

        [Fact]
        public void ConstructingARayWhenTheCameraIsTransformed()
        {
            var c = new Camera(201, 101, Math.PI / 2);

            c.Transform = Matrix.RotationY(Math.PI / 4) * Matrix.Translation(0, -2, 5);
            var r = c.RayForPixel(100, 50);

            Assert.Equal(new Point(0, 2, -5), r.Origin);
            Assert.Equal(new Vector(Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2), r.Direction);
        }

        [Fact]
        public void RenderingAWorldWithACamera()
        {
            var w = World.CreateDefault();
            var c = new Camera(11, 11, Math.PI / 2);
            var from = new Point(0, 0, -5);
            var to = new Point(0, 0, 0);
            var up = new Vector(0, 1, 0);
            c.Transform = Matrix.View(from, to, up);

            var image = c.Render(w);

            Assert.Equal(new Color(0.38066, 0.47583, 0.2855), image[5, 5]);
        }
    }
}

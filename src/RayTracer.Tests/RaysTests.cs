using RayTracer.Matrices;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class RaysTests
    {
        [Fact]
        public void CreatingAndQueryARay()
        {
            var origin = new Point(1, 2, 3);
            var direction = new Vector(4, 5, 6);
            var ray = new Ray(origin, direction);

            Assert.Equal(origin, ray.Origin);
            Assert.Equal(direction, ray.Direction);
        }

        [Fact]
        public void ComputingAPointFromADistance()
        {
            var r = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));

            Assert.Equal(new Point(2, 3, 4), r.Position(0));
            Assert.Equal(new Point(3, 3, 4), r.Position(1));
            Assert.Equal(new Point(1, 3, 4), r.Position(-1));
            Assert.Equal(new Point(4.5, 3, 4), r.Position(2.5));
        }

        [Fact]
        public void TranslatingARay()
        {
            var r = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            var m = Matrix.Translation(3, 4, 5);

            var r2 = r.Transform(m);

            Assert.Equal(new Point(4, 6, 8), r2.Origin);
            Assert.Equal(new Vector(0, 1, 0), r2.Direction);
        }

        [Fact]
        public void ScalingARay()
        {
            var r = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            var m = Matrix.Scaling(2, 3, 4);

            var r2 = r.Transform(m);

            Assert.Equal(new Point(2, 6, 12), r2.Origin);
            Assert.Equal(new Vector(0, 3, 0), r2.Direction);
        }
    }
}

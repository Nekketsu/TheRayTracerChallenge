using RayTracer.Shapes;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class TrianglesTests
    {
        [Fact]
        public void ConstructingATriangle()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var t = new Triangle(p1, p2, p3);

            Assert.Equal(p1, t.P1);
            Assert.Equal(p2, t.P2);
            Assert.Equal(p3, t.P3);
            Assert.Equal(new Vector(-1, -1, 0), t.E1);
            Assert.Equal(new Vector(1, -1, 0), t.E2);
        }

        [Fact]
        public void FindingTheNormalOnATriangle()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            var n1 = t.LocalNormalAt(new Point(0, 0.5, 0));
            var n2 = t.LocalNormalAt(new Point(-0.5, 0.75, 0));
            var n3 = t.LocalNormalAt(new Point(0.5, 0.25, 0));

            Assert.Equal(t.Normal, n1);
            Assert.Equal(t.Normal, n2);
            Assert.Equal(t.Normal, n3);
        }

        [Fact]
        public void IntersectingARayParallelToTheTriangle()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(0, -1, -2), new Vector(0, 1, 0));

            var xs = t.LocalIntersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        public void ARayMissesTheP1P3Edge()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(1, 1, -2), new Vector(0, 0, 1));

            var xs = t.LocalIntersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        public void ARayMissesTheP1P2Edge()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(-1, 1, -2), new Vector(0, 0, 1));

            var xs = t.LocalIntersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        public void ARayMissesTheP2P3Edge()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(0, -1, -2), new Vector(0, 0, 1));

            var xs = t.LocalIntersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        public void ARayStrikesATriangle()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(0, 0.5, -2), new Vector(0, 0, 1));

            var xs = t.LocalIntersect(r);

            Assert.Equal(1, xs.Length);
            Assert.Equal(2, xs[0].T);
        }
    }
}

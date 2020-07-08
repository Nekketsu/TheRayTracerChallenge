using RayTracer.Shapes;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class PlanesTests
    {
        [Fact]
        public void TheNormalOfAPlaneIsConstantEverywhere()
        {
            var p = new Plane();

            var n1 = p.LocalNormalAt(new Point(0, 0, 0));
            var n2 = p.LocalNormalAt(new Point(10, 0, -10));
            var n3 = p.LocalNormalAt(new Point(-5, 0, 150));

            Assert.Equal(new Vector(0, 1, 0), n1);
            Assert.Equal(new Vector(0, 1, 0), n2);
            Assert.Equal(new Vector(0, 1, 0), n3);
        }

        [Fact]
        public void IntersectWithARayParallelToThePlane()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, 10, 0), new Vector(0, 0, 1));

            var xs = p.LocalIntersect(r);
            Assert.Empty(xs);
        }

        [Fact]
        public void IntersectWithACoplanarRay()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            var xs = p.LocalIntersect(r);
            Assert.Empty(xs);
        }

        [Fact]
        public void ARayIntersectingAPlaneFromAbove()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));

            var xs = p.LocalIntersect(r);

            Assert.Equal(1, xs.Length);
            Assert.Equal(1, xs[0].T);
            Assert.Equal(p, xs[0].Object);
        }

        [Fact]
        public void ARayIntersectingAPlaneFromBelow()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));

            var xs = p.LocalIntersect(r);

            Assert.Equal(1, xs.Length);
            Assert.Equal(1, xs[0].T);
            Assert.Equal(p, xs[0].Object);
        }
    }
}

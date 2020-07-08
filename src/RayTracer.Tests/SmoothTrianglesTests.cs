using RayTracer.Extensions;
using RayTracer.Intersections;
using RayTracer.Shapes;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class SmoothTrianglesTests
    {
        [Fact]
        public void SmoothTriangles()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);

            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);

            Assert.Equal(p1, tri.P1);
            Assert.Equal(p2, tri.P2);
            Assert.Equal(p3, tri.P3);
            Assert.Equal(n1, tri.N1);
            Assert.Equal(n2, tri.N2);
            Assert.Equal(n3, tri.N3);
        }

        [Fact]
        public void AnIntersectionWithASmoothTriangeStoresUV()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);

            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
            var ray = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            var xs = tri.LocalIntersect(ray);

            Assert.True(0.45.EqualsEpsilon(((IntersectionWithUV)xs[0]).U));
            Assert.True(0.25.EqualsEpsilon(((IntersectionWithUV)xs[0]).V));
        }

        [Fact]
        public void ASmoothTriangeUsesUVToInterpolateTheNormal()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);

            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);

            var i = new IntersectionWithUV(1, tri, 0.45, 0.25);
            var n = tri.NormalAt(new Point(0, 0, 0), i);

            Assert.Equal(new Vector(-0.5547, 0.83205, 0), n);
        }

        [Fact]
        public void PreparingTheNormaONASmoothTriangle()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);

            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
            var i = new IntersectionWithUV(1, tri, 0.45, 0.25);
            var r = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            var xs = new IntersectionCollection(i);
            var computations = i.PrepareComutations(r, xs);

            Assert.Equal(new Vector(-0.5547, 0.83205, 0), computations.NormalVector);
        }
    }
}

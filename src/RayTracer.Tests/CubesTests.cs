using RayTracer.Shapes;
using RayTracer.Tuples;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class CubesTests
    {
        [Theory]
        [MemberData(nameof(ARayIntersectsACubeData))]
        public void ARayIntersectsACube(Point origin, Vector direction, double t1, double t2)
        {
            var c = new Cube();
            var r = new Ray(origin, direction);

            var xs = c.LocalIntersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(t1, xs[0].T);
            Assert.Equal(t2, xs[1].T);
        }

        public static IEnumerable<object[]> ARayIntersectsACubeData
        {
            get
            {
                yield return new object[] { new Point(5, 0.5, 0), new Vector(-1, 0, 0), 4, 6 };
                yield return new object[] { new Point(-5, 0.5, 0), new Vector(1, 0, 0), 4, 6 };
                yield return new object[] { new Point(0.5, 5, 0), new Vector(0, -1, 0), 4, 6 };
                yield return new object[] { new Point(0.5, -5, 0), new Vector(0, 1, 0), 4, 6 };
                yield return new object[] { new Point(0.5, 0, 5), new Vector(0, 0, -1), 4, 6 };
                yield return new object[] { new Point(0.5, 0, -5), new Vector(0, 0, 1), 4, 6 };
                yield return new object[] { new Point(0, 0.5, 0), new Vector(0, 0, 1), -1, 1 };
            }
        }

        [Theory]
        [MemberData(nameof(ARayMissesACubeData))]
        public void ARayMissesACube(Point origin, Vector direction)
        {
            var c = new Cube();
            var r = new Ray(origin, direction);

            var xs = c.LocalIntersect(r);

            Assert.Empty(xs);
        }

        public static IEnumerable<object[]> ARayMissesACubeData
        {
            get
            {
                yield return new object[] { new Point(-2, 0, 0), new Vector(0.2673, 0.5345, 0.8018) };
                yield return new object[] { new Point(0, -2, 0), new Vector(0.8018, 0.2673, 0.5345) };
                yield return new object[] { new Point(0, 0, -2), new Vector(0.5345, 0.8018, 0.2673) };
                yield return new object[] { new Point(2, 0, 2), new Vector(0, 0, -1) };
                yield return new object[] { new Point(0, 2, 2), new Vector(0, -1, 0) };
                yield return new object[] { new Point(2, 2, 0), new Vector(-1, 0, 0) };
            }
        }

        [Theory]
        [MemberData(nameof(TheNormalOnTheSurfaceOfACubeData))]
        public void TheNormalOnTheSurfaceOfACube(Point point, Vector normal)
        {
            var c = new Cube();

            var actualNormal = c.LocalNormalAt(point);

            Assert.Equal(normal, actualNormal);
        }

        public static IEnumerable<object[]> TheNormalOnTheSurfaceOfACubeData
        {
            get
            {
                yield return new object[] { new Point(1, 0.5, -0.8), new Vector(1, 0, 0) };
                yield return new object[] { new Point(-1, -0.2, 0.9), new Vector(-1, 0, 0) };
                yield return new object[] { new Point(-0.4, 1, -0.1), new Vector(0, 1, 0) };
                yield return new object[] { new Point(0.3, -1, -0.7), new Vector(0, -1, 0) };
                yield return new object[] { new Point(-0.6, 0.3, 1), new Vector(0, 0, 1) };
                yield return new object[] { new Point(0.4, 0.4, -1), new Vector(0, 0, -1) };
                yield return new object[] { new Point(1, 1, 1), new Vector(1, 0, 0) };
                yield return new object[] { new Point(-1, -1, -1), new Vector(-1, 0, 0) };
            }
        }
    }
}

using RayTracer.Extensions;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class ConesTests
    {
        [Theory]
        [MemberData(nameof(IntersectingAConeWithARayData))]
        public void IntersectingAConeWithARay(Point origin, Vector direction, double t0, double t1)
        {
            var shape = new Cone();
            var normalizedDirection = direction.Normalize();
            var r = new Ray(origin, normalizedDirection);

            var xs = shape.LocalIntersect(r);

            Assert.Equal(2, xs.Length);
            Assert.True(t0.EqualsEpsilon(xs[0].T));
            Assert.True(t1.EqualsEpsilon(xs[1].T));
        }

        public static IEnumerable<object[]> IntersectingAConeWithARayData
        {
            get
            {
                yield return new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 5, 5 };
                yield return new object[] { new Point(0, 0, -5), new Vector(1, 1, 1), 8.66025, 8.66025 };
                yield return new object[] { new Point(1, 1, -5), new Vector(-0.5, -1, 1), 4.550006, 49.44994 };
            }
        }

        [Fact]
        public void IntersectingAConeWithARayParallelToOneOfItsHalves()
        {
            var shape = new Cone();
            var direction = new Vector(0, 1, 1).Normalize();
            var r = new Ray(new Point(0, 0, -1), direction);

            var xs = shape.LocalIntersect(r);

            Assert.Equal(1, xs.Length);
            Assert.True(0.35355.EqualsEpsilon(xs[0].T));
        }

        [Theory]
        [MemberData(nameof(IntersectingAConesEndCapsData))]
        public void IntersectingAConesEndCaps(Point origin, Vector direction, int count)
        {
            var shape = new Cone();
            shape.Minimum = -0.5;
            shape.Maximum = 0.5;
            shape.Closed = true;
            var normalizedDirection = direction.Normalize();
            var r = new Ray(origin, normalizedDirection);

            var xs = shape.LocalIntersect(r);
            Assert.Equal(count, xs.Length);
        }

        public static IEnumerable<object[]> IntersectingAConesEndCapsData
        {
            get
            {
                yield return new object[] { new Point(0, 0, -5), new Vector(0, 1, 0), 0 };
                yield return new object[] { new Point(0, 0, -0.25), new Vector(0, 1, 1), 2 };
                yield return new object[] { new Point(0, 0, -0.25), new Vector(0, 1, 0), 4 };
            }
        }

        [Theory]
        [MemberData(nameof(ComputingTheNormalVectorOnAConeData))]
        public void ComputingTheNormalVectorOnACone(Point point, Vector normal)
        {
            var shape = new Cone();

            var n = shape.LocalNormalAt(point);

            Assert.Equal(normal, n);
        }

        public static IEnumerable<object[]> ComputingTheNormalVectorOnAConeData
        {
            get
            {
                yield return new object[] { new Point(0, 0, 0), new Vector(0, 0, 0) };
                yield return new object[] { new Point(1, 1, 1), new Vector(1, -Math.Sqrt(2), 1) };
                yield return new object[] { new Point(-1, -1, 0), new Vector(-1, 1, 0) };
            }
        }
    }
}

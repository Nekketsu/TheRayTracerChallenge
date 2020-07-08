using RayTracer.Extensions;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class CylindersTests
    {
        [Theory]
        [MemberData(nameof(ARayMissesACylinderData))]
        public void ARayMissesACylinder(Point origin, Vector direction)
        {
            var cyl = new Cylinder();
            var normalizedDirection = direction.Normalize();
            var r = new Ray(origin, normalizedDirection);

            var xs = cyl.LocalIntersect(r);

            Assert.Empty(xs);
        }

        public static IEnumerable<object[]> ARayMissesACylinderData
        {
            get
            {
                yield return new object[] { new Point(1, 0, 0), new Vector(0, 1, 0) };
                yield return new object[] { new Point(0, 0, 0), new Vector(0, 1, 0) };
                yield return new object[] { new Point(0, 0, -5), new Vector(1, 1, 1) };
            }
        }

        [Theory]
        [MemberData(nameof(ARayStrikesACylinderData))]
        public void ARayStrikesACylinder(Point origin, Vector direction, double t0, double t1)
        {
            var cyl = new Cylinder();
            var normalizedDirection = direction.Normalize();
            var r = new Ray(origin, normalizedDirection);

            var xs = cyl.LocalIntersect(r);

            Assert.Equal(2, xs.Length);
            Assert.True(t0.EqualsEpsilon(xs[0].T));
            Assert.True(t1.EqualsEpsilon(xs[1].T));
        }

        public static IEnumerable<object[]> ARayStrikesACylinderData
        {
            get
            {
                yield return new object[] { new Point(1, 0, -5), new Vector(0, 0, 1), 5, 5 };
                yield return new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 4, 6 };
                yield return new object[] { new Point(0.5, 0, -5), new Vector(0.1, 1, 1), 6.80798, 7.08872 };
            }
        }

        [Theory]
        [MemberData(nameof(NormalVectorOnACylinderData))]
        public void NormalVectorOnACylinder(Point point, Vector normal)
        {
            var cyl = new Cylinder();

            var n = cyl.LocalNormalAt(point);

            Assert.Equal(normal, n);
        }

        public static IEnumerable<object[]> NormalVectorOnACylinderData
        {
            get
            {
                yield return new object[] { new Point(1, 0, 0), new Vector(1, 0, 0) };
                yield return new object[] { new Point(0, 5, -1), new Vector(0, 0, -1) };
                yield return new object[] { new Point(0, -2, 1), new Vector(0, 0, 1) };
                yield return new object[] { new Point(-1, 1, 0), new Vector(-1, 0, 0) };
            }
        }

        [Fact]
        public void TheDefaultMinimumAndMaximumForACylinder()
        {
            var cyl = new Cylinder();

            Assert.Equal(double.NegativeInfinity, cyl.Minimum);
            Assert.Equal(double.PositiveInfinity, cyl.Maximum);
        }

        [Theory]
        [MemberData(nameof(IntersectingAConstrainedCylinderData))]
        public void IntersectingAConstrainedCylinder(Point point, Vector direction, int count)
        {
            var cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            var normalizedDirection = direction.Normalize();
            var r = new Ray(point, normalizedDirection);

            var xs = cyl.LocalIntersect(r);

            Assert.Equal(count, xs.Length);
        }

        public static IEnumerable<object[]> IntersectingAConstrainedCylinderData
        {
            get
            {
                yield return new object[] { new Point(0, 1.5, 0), new Vector(0.1, 1, 0), 0 };
                yield return new object[] { new Point(0, 3, -5), new Vector(0, 0, 1), 0 };
                yield return new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 0 };
                yield return new object[] { new Point(0, 2, -5), new Vector(0, 0, 1), 0 };
                yield return new object[] { new Point(0, 1, -5), new Vector(0, 0, 1), 0 };
                yield return new object[] { new Point(0, 1.5, -2), new Vector(0, 0, 1), 2 };
            }
        }

        [Fact]
        public void TheDefaultClosedValueForACylinder()
        {
            var cyl = new Cylinder();

            Assert.False(cyl.Closed);
        }

        [Theory]
        [MemberData(nameof(IntersectingTheCapsOfAClosedCylinderData))]
        public void IntersectingTheCapsOfAClosedCylinder(Point point, Vector direction, int count)
        {
            var cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            cyl.Closed = true;
            var normalizedDirection = direction.Normalize();
            var r = new Ray(point, normalizedDirection);

            var xs = cyl.LocalIntersect(r);

            Assert.Equal(count, xs.Length);
        }

        public static IEnumerable<object[]> IntersectingTheCapsOfAClosedCylinderData
        {
            get
            {
                yield return new object[] { new Point(0, 3, 0), new Vector(0, -1, 0), 2 };
                yield return new object[] { new Point(0, 3, -2), new Vector(0, -1, 2), 2 };
                yield return new object[] { new Point(0, 4, -2), new Vector(0, -1, 1), 2 }; // Corner case
                yield return new object[] { new Point(0, 0, -2), new Vector(0, 1, 2), 2 };
                yield return new object[] { new Point(0, -1, -2), new Vector(0, 1, 1), 2 }; // Corner case
            }
        }

        [Theory]
        [MemberData(nameof(TheNormalVectorOnACylindersEndCapsData))]
        public void TheNormalVectorOnACylindersEndCaps(Point point, Vector normal)
        {
            var cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            cyl.Closed = true;

            var n = cyl.LocalNormalAt(point);

            Assert.Equal(normal, n);
        }

        public static IEnumerable<object[]> TheNormalVectorOnACylindersEndCapsData
        {
            get
            {
                yield return new object[] { new Point(0, 1, 0), new Vector(0, -1, 0) };
                yield return new object[] { new Point(0.5, 1, 0), new Vector(0, -1, 0) };
                yield return new object[] { new Point(0, 1, 0.5), new Vector(0, -1, 0) };
                yield return new object[] { new Point(0, 2, 0), new Vector(0, 1, 0) };
                yield return new object[] { new Point(0.5, 2, 0), new Vector(0, 1, 0) };
                yield return new object[] { new Point(0, 2, 0.5), new Vector(0, 1, 0) };
            }
        }
    }
}

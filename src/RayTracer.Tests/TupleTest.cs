using RayTracer.Tuples;
using System;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class TupleTest
    {
        [Fact]
        public void TupleWithW1IsAPoint()
        {
            Tuples.Tuple tuple = new Point(4.3, -4.2, 3.1);

            Assert.Equal(4.3, tuple.X);
            Assert.Equal(-4.2, tuple.Y);
            Assert.Equal(3.1, tuple.Z);
            Assert.Equal(1.0, tuple.W);

            Assert.True(tuple is Point);
            Assert.False(tuple is Vector);
        }

        [Fact]
        public void TupleWith0IsAVector()
        {
            Tuples.Tuple tuple = new Vector(4.3, -4.2, 3.1);

            Assert.Equal(4.3, tuple.X);
            Assert.Equal(-4.2, tuple.Y);
            Assert.Equal(3.1, tuple.Z);
            Assert.Equal(0.0, tuple.W);

            Assert.False(tuple is Point);
            Assert.True(tuple is Vector);
        }

        [Fact]
        public void PointCreatesTupleWithW1()
        {
            var point = new Point(4, -4, 3);

            Assert.True(point is Tuples.Tuple tuple && tuple.W == 1);
        }

        [Fact]
        public void VectorCreatesTupleWithW0()
        {
            var vector = new Vector(4, -4, 3);

            Assert.True(vector is Tuples.Tuple tuple && tuple.W == 0);
        }

        [Fact]
        public void AddingAVectorToAPoint()
        {
            var p = new Point(3, -2, 5);
            var v = new Vector(-2, 3, 1);

            Assert.Equal(new Point(1, 1, 6), p + v);
        }

        [Fact]
        public void SubstractingTwoPoints()
        {
            var p1 = new Point(3, 2, 1);
            var p2 = new Point(5, 6, 7);

            Assert.Equal(new Vector(-2, -4, -6), p1 - p2);
        }

        [Fact]
        public void SubstractingAVectorFromAPoint()
        {
            var p = new Point(3, 2, 1);
            var v = new Vector(5, 6, 7);

            Assert.Equal(new Point(-2, -4, -6), p - v);
        }

        [Fact]
        public void SubstractingTwoVectors()
        {
            var v1 = new Vector(3, 2, 1);
            var v2 = new Vector(5, 6, 7);

            Assert.Equal(new Vector(-2, -4, -6), v1 - v2);
        }

        [Fact]
        public void SubstractingAVectorFromTheZeroVector()
        {
            var v = new Vector(1, -2, 3);

            Assert.Equal(new Vector(-1, 2, -3), Vector.Zero - v);
        }

        [Fact]
        public void NegatingAVector()
        {
            var v = new Vector(1, -2, 3);

            Assert.Equal(new Vector(-1, 2, -3), -v);
        }

        [Fact]
        public void MultiplyingAVectorByAScalar()
        {
            var v = new Vector(1, -2, 3);

            Assert.Equal(new Vector(3.5, -7, 10.5), v * 3.5);
            Assert.Equal(new Vector(3.5, -7, 10.5), 3.5 * v);
        }

        [Fact]
        public void MultiplyingAVectorByAFraction()
        {
            var v = new Vector(1, -2, 3);

            Assert.Equal(new Vector(0.5, -1, 1.5), v * 0.5);
            Assert.Equal(new Vector(0.5, -1, 1.5), 0.5 * v);
        }

        [Fact]
        public void DividingAVectorByAScalar()
        {
            var v = new Vector(1, -2, 3);

            Assert.Equal(new Vector(0.5, -1, 1.5), v / 2);
        }

        [Theory]
        [MemberData(nameof(MagnitudeData))]
        public void ComputingTheMagnitudeOfVector(Vector vector, double length)
        {
            Assert.Equal(vector.Length, length);
        }

        public static IEnumerable<object[]> MagnitudeData
        {
            get
            {
                yield return new object[] { new Vector(1, 0, 0), 1 };
                yield return new object[] { new Vector(0, 1, 0), 1 };
                yield return new object[] { new Vector(0, 0, 1), 1 };
                yield return new object[] { new Vector(1, 2, 3), Math.Sqrt(14) };
                yield return new object[] { new Vector(-1, -2, -3), Math.Sqrt(14) };
            }
        }

        [Theory]
        [MemberData(nameof(NormalizingData))]
        public void NormalizingVector(Vector vector, Vector result)
        {
            Assert.Equal(vector.Normalize(), result);
        }

        public static IEnumerable<object[]> NormalizingData
        {
            get
            {
                yield return new object[] { new Vector(4, 0, 0), new Vector(1, 0, 0) };
                yield return new object[] { new Vector(1, 2, 3), new Vector(1 / Math.Sqrt(14), 2 / Math.Sqrt(14), 3 / Math.Sqrt(14)) };
            }
        }

        [Fact]
        public void TheMagnitudeOfANormalizedVector()
        {
            var v = new Vector(1, 2, 3);
            var normalized = v.Normalize();

            Assert.Equal(1, normalized.Length);
        }

        [Fact]
        public void TheDotProductOfTwoVectors()
        {
            var a = new Vector(1, 2, 3);
            var b = new Vector(2, 3, 4);

            Assert.Equal(20, a * b);
        }

        [Fact]
        public void TheCrossProductOfTwoVectors()
        {
            var a = new Vector(1, 2, 3);
            var b = new Vector(2, 3, 4);

            Assert.Equal(new Vector(-1, 2, -1), a.Cross(b));
            Assert.Equal(new Vector(1, -2, 1), b.Cross(a));
        }

        [Fact]
        public void ColorsAreTuples()
        {
            var c = new Color(-0.5, 0.4, 1.7);

            Assert.Equal(-0.5, c.Red);
            Assert.Equal(0.4, c.Green);
            Assert.Equal(1.7, c.Blue);
        }

        [Fact]
        public void AddingColors()
        {
            var c1 = new Color(0.9, 0.6, 0.75);
            var c2 = new Color(0.7, 0.1, 0.25);

            Assert.Equal(new Color(1.6, 0.7, 1.0), c1 + c2);
        }

        [Fact]
        public void SubstractingColors()
        {
            var c1 = new Color(0.9, 0.6, 0.75);
            var c2 = new Color(0.7, 0.1, 0.25);

            Assert.Equal(new Color(0.2, 0.5, 0.5), c1 - c2);
        }

        [Fact]
        public void MultiplyingAColorByAScalar()
        {
            var c = new Color(0.2, 0.3, 0.4);

            Assert.Equal(new Color(0.4, 0.6, 0.8), c * 2);
        }

        [Fact]
        public void MuiltiplyingColors()
        {
            var c1 = new Color(1, 0.2, 0.4);
            var c2 = new Color(0.9, 1, 0.1);

            Assert.Equal(new Color(0.9, 0.2, 0.04), c1 * c2);
        }

        [Fact]
        public void ReflectingAVectorApproachingAt45Degrees()
        {
            var v = new Vector(1, -1, 0);
            var n = new Vector(0, 1, 0);

            var r = v.Reflect(n);

            Assert.Equal(new Vector(1, 1, 0), r);
        }

        [Fact]
        public void ReflectingAVectorOffASlantedSurface()
        {
            var v = new Vector(0, -1, 0);
            var n = new Vector(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0);

            var r = v.Reflect(n);

            Assert.Equal(new Vector(1, 0, 0), r);
        }
    }
}

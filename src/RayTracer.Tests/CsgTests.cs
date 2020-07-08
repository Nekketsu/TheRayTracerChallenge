using RayTracer.Intersections;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class CsgTests
    {
        [Fact]
        public void CsgIsCreatedWithAnOperationAndTwoShapes()
        {
            var s1 = new Sphere();
            var s2 = new Cube();

            var c = Csg.Union(s1, s2);

            Assert.True(c is CsgUnion);
            Assert.Equal(s1, c.Left);
            Assert.Equal(s2, c.Right);
            Assert.Equal(c, s1.Parent);
            Assert.Equal(c, s2.Parent);
        }

        [Theory]
        [MemberData(nameof(EvaluatingTheRulForACsgUnionData))]
        public void EvaluatingTheRulForACsgUnion(bool leftHit, bool inLeft, bool inRight, bool result)
        {
            var s1 = new Sphere();
            var s2 = new Cube();

            var c = Csg.Union(s1, s2);

            var actualResult = c.IntersectionAllowed(leftHit, inLeft, inRight);

            Assert.Equal(result, actualResult);
        }

        public static IEnumerable<object[]> EvaluatingTheRulForACsgUnionData
        {
            get
            {
                yield return new object[] { true, true, true, false };
                yield return new object[] { true, true, false, true };
                yield return new object[] { true, false, true, false };
                yield return new object[] { true, false, false, true };
                yield return new object[] { false, true, true, false };
                yield return new object[] { false, true, false, false };
                yield return new object[] { false, false, true, true };
                yield return new object[] { false, false, false, true };
            }
        }

        [Theory]
        [MemberData(nameof(EvaluatingTheRulForACsgIntersectionData))]
        public void EvaluatingTheRulForACsgIntersection(bool leftHit, bool inLeft, bool inRight, bool result)
        {
            var s1 = new Sphere();
            var s2 = new Cube();

            var c = Csg.Intersection(s1, s2);

            var actualResult = c.IntersectionAllowed(leftHit, inLeft, inRight);

            Assert.Equal(result, actualResult);
        }

        public static IEnumerable<object[]> EvaluatingTheRulForACsgIntersectionData
        {
            get
            {
                yield return new object[] { true, true, true, true };
                yield return new object[] { true, true, false, false };
                yield return new object[] { true, false, true, true };
                yield return new object[] { true, false, false, false };
                yield return new object[] { false, true, true, true };
                yield return new object[] { false, true, false, true };
                yield return new object[] { false, false, true, false };
                yield return new object[] { false, false, false, false };
            }
        }

        [Theory]
        [MemberData(nameof(EvaluatingTheRulForACsgDifferenceData))]
        public void EvaluatingTheRulForACsgDifference(bool leftHit, bool inLeft, bool inRight, bool result)
        {
            var s1 = new Sphere();
            var s2 = new Cube();

            var c = Csg.Difference(s1, s2);

            var actualResult = c.IntersectionAllowed(leftHit, inLeft, inRight);

            Assert.Equal(result, actualResult);
        }

        public static IEnumerable<object[]> EvaluatingTheRulForACsgDifferenceData
        {
            get
            {
                yield return new object[] { true, true, true, false };
                yield return new object[] { true, true, false, true };
                yield return new object[] { true, false, true, false };
                yield return new object[] { true, false, false, true };
                yield return new object[] { false, true, true, true };
                yield return new object[] { false, true, false, true };
                yield return new object[] { false, false, true, false };
                yield return new object[] { false, false, false, false };
            }
        }

        [Fact]
        public void FilteringAListOfIntersectionsUnion()
        {
            var s1 = new Sphere();
            var s2 = new Cube();
            var c = Csg.Union(s1, s2);
            var xs = new IntersectionCollection(
                new Intersection(1, s1),
                new Intersection(2, s2),
                new Intersection(3, s1),
                new Intersection(4, s2)
            );

            var result = c.FilterIntersections(xs);

            Assert.Equal(2, result.Length);
            Assert.Equal(xs[0], result[0]);
            Assert.Equal(xs[3], result[1]);
        }

        [Fact]
        public void FilteringAListOfIntersectionsIntersection()
        {
            var s1 = new Sphere();
            var s2 = new Cube();
            var c = Csg.Intersection(s1, s2);
            var xs = new IntersectionCollection(
                new Intersection(1, s1),
                new Intersection(2, s2),
                new Intersection(3, s1),
                new Intersection(4, s2)
            );

            var result = c.FilterIntersections(xs);

            Assert.Equal(2, result.Length);
            Assert.Equal(xs[1], result[0]);
            Assert.Equal(xs[2], result[1]);
        }

        [Fact]
        public void FilteringAListOfIntersectionsDifference()
        {
            var s1 = new Sphere();
            var s2 = new Cube();
            var c = Csg.Difference(s1, s2);
            var xs = new IntersectionCollection(
                new Intersection(1, s1),
                new Intersection(2, s2),
                new Intersection(3, s1),
                new Intersection(4, s2)
            );

            var result = c.FilterIntersections(xs);

            Assert.Equal(2, result.Length);
            Assert.Equal(xs[0], result[0]);
            Assert.Equal(xs[1], result[1]);
        }

        [Fact]
        public void ARayMissesACsgObject()
        {
            var c = Csg.Union(new Sphere(), new Cube());
            var r = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));

            var xs = c.LocalIntersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        public void ARayHitsACsgObject()
        {
            var s1 = new Sphere();
            var s2 = new Sphere();
            s2.Transform = Matrix.Translation(0, 0, 0.5);
            var c = Csg.Union(s1, s2);
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            var xs = c.LocalIntersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(s1, xs[0].Object);
            Assert.Equal(6.5, xs[1].T);
            Assert.Equal(s2, xs[1].Object);
        }
    }
}

using RayTracer.Extensions;
using RayTracer.Intersections;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;
using Xunit;

namespace RayTracer.Tests
{
    public class IntersectionsTests
    {
        [Fact]
        public void AnInteresctionEncapsulatesTAndObject()
        {
            var s = new Sphere();

            var i = new Intersection(3.5, s);

            Assert.Equal(3.5, i.T);
            Assert.Equal(s, i.Object);
        }

        [Fact]
        public void AggregatingIntersections()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);

            var xs = new IntersectionCollection(i1, i2);

            Assert.Equal(2, xs.Length);
            Assert.Equal(1, xs[0].T);
            Assert.Equal(2, xs[1].T);
        }

        [Fact]
        public void TheHitWhenAllIntersectionsHavePositiveT()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = new IntersectionCollection(i2, i1);

            var i = xs.Hit;

            Assert.Equal(i1, i);
        }

        [Fact]
        public void TheHitWhenSomeIntersectionsHaveNegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(1, s);
            var xs = new IntersectionCollection(i2, i1);

            var i = xs.Hit;

            Assert.Equal(i, i2);
        }

        [Fact]
        public void TheHitwhenAllIntersectionsHaveNegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(-2, s);
            var i2 = new Intersection(-1, s);
            var xs = new IntersectionCollection(i2, i1);

            var i = xs.Hit;

            Assert.Null(i);
        }

        [Fact]
        public void TheHitIsAlwaysTheLowestNonNegativeIntersection()
        {
            var s = new Sphere();
            var i1 = new Intersection(5, s);
            var i2 = new Intersection(7, s);
            var i3 = new Intersection(-3, s);
            var i4 = new Intersection(2, s);
            var xs = new IntersectionCollection(i1, i2, i3, i4);

            var i = xs.Hit;

            Assert.Equal(i4, i);
        }

        [Fact]
        public void PrecomputingTheStateOfAnIntersection()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);

            var computations = i.PrepareComutations(r);

            Assert.Equal(i.T, computations.T);
            Assert.Equal(i.Object, computations.Object);
            Assert.Equal(new Point(0, 0, -1), computations.Point);
            Assert.Equal(new Vector(0, 0, -1), computations.EyeVector);
            Assert.Equal(new Vector(0, 0, -1), computations.NormalVector);
        }

        [Fact]
        public void TheHitWhenAnIntersectionOccursOnTheOutside()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);

            var computations = i.PrepareComutations(r);

            Assert.False(computations.Inside);
        }

        [Fact]
        public void TheHitWhenAnIntersectionOccursOnTheInside()
        {
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(1, shape);

            var computations = i.PrepareComutations(r);

            Assert.Equal(new Point(0, 0, 1), computations.Point);
            Assert.Equal(new Vector(0, 0, -1), computations.EyeVector);
            Assert.True(computations.Inside);
            // Normal would have been (0, 0, 1) but is inverted!
            Assert.Equal(new Vector(0, 0, -1), computations.NormalVector);
        }

        [Fact]
        public void TheHitShouldOffsetThePoint()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere { Transform = Matrix.Translation(0, 0, 1) };
            var i = new Intersection(5, shape);

            var computations = i.PrepareComutations(r);

            Assert.True(computations.OverPoint.Z < -DoubleExtensions.Epsilon / 2);
            Assert.True(computations.Point.Z > computations.OverPoint.Z);
        }

        [Fact]
        public void TheUnderPointIsOffsetBelowTheSurface()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new GlassSphere();
            shape.Transform = Matrix.Translation(0, 0, 1);
            var i = new Intersection(5, shape);
            var xs = new IntersectionCollection(i);

            var computations = i.PrepareComutations(r, xs);

            Assert.True(DoubleExtensions.Epsilon / 2 < computations.UnderPoint.Z);
            Assert.True(computations.Point.Z < computations.UnderPoint.Z);
        }

        [Fact]
        public void TheSchlickApproximationUnderTotalInternalReflection()
        {
            var shape = new GlassSphere();
            var r = new Ray(new Point(0, 0, Math.Sqrt(2) / 2), new Vector(0, 1, 0));
            var xs = new IntersectionCollection(
                new Intersection(-Math.Sqrt(2) / 2, shape),
                new Intersection(Math.Sqrt(2) / 2, shape)
            );

            var computations = xs[1].PrepareComutations(r, xs);
            var reflectance = computations.Schlick();

            Assert.Equal(1, reflectance);
        }

        [Fact]
        public void TheSchlickApproximationWithAPerpendicularViewingAngle()
        {
            var shape = new GlassSphere();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));
            var xs = new IntersectionCollection(
                new Intersection(-1, shape),
                new Intersection(1, shape)
            );

            var computations = xs[1].PrepareComutations(r, xs);
            var reflectance = computations.Schlick();

            Assert.True(0.04.EqualsEpsilon(reflectance));
        }

        [Fact]
        public void TheSchlickApproximationWithSmallAngleAndN2BiggerThanN1()
        {
            var shape = new GlassSphere();
            var r = new Ray(new Point(0, 0.99, -2), new Vector(0, 0, 1));
            var xs = new IntersectionCollection(new Intersection(1.8589, shape));

            var computations = xs[0].PrepareComutations(r, xs);
            var reflectance = computations.Schlick();

            Assert.True(0.48873.EqualsEpsilon(reflectance));
        }

        [Fact]
        public void AnIntersectionCanEncapsulateUAndV()
        {
            var s = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            var i = new IntersectionWithUV(3.5, s, 0.2, 0.4);

            Assert.Equal(0.2, i.U);
            Assert.Equal(0.4, i.V);
        }
    }
}

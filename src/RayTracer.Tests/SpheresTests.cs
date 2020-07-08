using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;
using Xunit;

namespace RayTracer.Tests
{
    public class SpheresTests
    {
        [Fact]
        public void ARayIntersectsASphereAtTwoPoints()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(4.0, xs[0].T);
            Assert.Equal(6.0, xs[1].T);
        }

        [Fact]
        public void ARayIntersectsASphereAtATangent()
        {
            var r = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(5.0, xs[0].T);
            Assert.Equal(5.0, xs[1].T);
        }

        [Fact]
        public void ARayMissesASphere()
        {
            var r = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(0, xs.Length);
        }

        [Fact]
        public void ARayOriginatesInsideASphere()
        {
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(-1.0, xs[0].T);
            Assert.Equal(1.0, xs[1].T);
        }

        [Fact]
        public void ASphereIsBehindARay()
        {
            var r = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(-6.0, xs[0].T);
            Assert.Equal(-4.0, xs[1].T);
        }

        [Fact]
        public void IntersectSetsTheObjectOnTheIntersection()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(s, xs[0].Object);
            Assert.Equal(s, xs[1].Object);
        }

        [Fact]
        public void AShperesDefaultTransformation()
        {
            var s = new Sphere();

            Assert.Equal(Matrix.Identity, s.Transform);
        }

        [Fact]
        public void ChangingASpheresTransformation()
        {
            var s = new Sphere();
            var t = Matrix.Translation(2, 3, 4);

            s.Transform = t;

            Assert.Equal(t, s.Transform);
        }

        [Fact]
        public void IntersectingAScaledSphereWithARay()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var s = new Sphere();

            s.Transform = Matrix.Scaling(2, 2, 2);
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Length);
            Assert.Equal(3, xs[0].T);
            Assert.Equal(7, xs[1].T);
        }

        [Fact]
        public void IntersectingATranslatedSphereWithARay()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var s = new Sphere();

            s.Transform = Matrix.Translation(5, 0, 0);
            var xs = s.Intersect(r);

            Assert.Equal(0, xs.Length);
        }

        [Fact]
        public void TheNormalOnASphereAtAPointOnTheXAxis()
        {
            var s = new Sphere();

            var n = s.NormalAt(new Point(1, 0, 0));

            Assert.Equal(new Vector(1, 0, 0), n);
        }

        [Fact]
        public void TheNormalOnASphereAtAPointOnTheYAxis()
        {
            var s = new Sphere();

            var n = s.NormalAt(new Point(0, 1, 0));

            Assert.Equal(new Vector(0, 1, 0), n);
        }

        [Fact]
        public void TheNormalOnASphereAtAPointOnTheZAxis()
        {
            var s = new Sphere();

            var n = s.NormalAt(new Point(0, 0, 1));

            Assert.Equal(new Vector(0, 0, 1), n);
        }

        [Fact]
        public void TheNormalOnASphereAtANonAxialPoint()
        {
            var s = new Sphere();

            var n = s.NormalAt(new Point(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));

            Assert.Equal(new Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3), n);
        }

        [Fact]
        public void TheNormalIsANormalizedVector()
        {
            var s = new Sphere();

            var n = s.NormalAt(new Point(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));

            Assert.Equal(n.Normalize(), n);
        }

        [Fact]
        public void ComputingTheNormalOnATranslatedSphere()
        {
            var s = new Sphere();
            s.Transform = Matrix.Translation(0, 1, 0);

            var n = s.NormalAt(new Point(0, 1.70711, -0.70711));

            Assert.Equal(new Vector(0, 0.70711, -0.70711), n);
        }

        [Fact]
        public void ComputingTheNormalOnATransformedSphere()
        {
            var s = new Sphere();
            var m = Matrix.Scaling(1, 0.5, 1) * Matrix.RotationZ(Math.PI / 5);
            s.Transform = m;

            var n = s.NormalAt(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));

            Assert.Equal(new Vector(0, 0.97014, -0.24254), n);
        }

        [Fact]
        public void ASphereHasADefaultMaterial()
        {
            var s = new Sphere();
            var m = s.Material;

            Assert.Equal(new Material(), m);
        }

        [Fact]
        public void ASphereMayBeAssignedAMaterial()
        {
            var s = new Sphere();
            var m = new Material();
            m.Ambient = 1;

            s.Material = m;

            Assert.Equal(m, s.Material);
        }
    }
}

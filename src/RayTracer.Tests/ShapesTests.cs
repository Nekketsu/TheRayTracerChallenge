using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Tests.Entities;
using RayTracer.Tuples;
using System;
using Xunit;

namespace RayTracer.Tests
{
    public class ShapesTests
    {
        [Fact]
        public void TheDefaultTransofrmation()
        {
            var s = new TestShape();

            Assert.Equal(Matrix.Identity, s.Transform);
        }

        [Fact]
        public void AssigningATransformation()
        {
            var s = new TestShape();

            s.Transform = Matrix.Translation(2, 3, 4);

            Assert.Equal(Matrix.Translation(2, 3, 4), s.Transform);
        }

        [Fact]
        public void TheDefaultMaterial()
        {
            var s = new TestShape();

            var m = s.Material;

            Assert.Equal(new Material(), m);
        }

        [Fact]
        public void AssigningAMaterial()
        {
            var s = new TestShape();
            var m = new Material();
            m.Ambient = 1;

            s.Material = m;

            Assert.Equal(m, s.Material);
        }

        [Fact]
        public void IntersectingAScaledShapeWithARay()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var s = new TestShape();

            s.Transform = Matrix.Scaling(2, 2, 2);
            var xs = s.Intersect(r);

            Assert.Equal(new Point(0, 0, -2.5), s.SavedRay.Origin);
            Assert.Equal(new Vector(0, 0, 0.5), s.SavedRay.Direction);
        }

        [Fact]
        public void IntersectingATranslatedShapeWithARay()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var s = new TestShape();

            s.Transform = Matrix.Translation(5, 0, 0);
            var xs = s.Intersect(r);

            Assert.Equal(new Point(-5, 0, -5), s.SavedRay.Origin);
            Assert.Equal(new Vector(0, 0, 1), s.SavedRay.Direction);
        }

        [Fact]
        public void ComputingTheNormalOnATranslatedShape()
        {
            var s = new TestShape();

            s.Transform = Matrix.Translation(0, 1, 0);
            var n = s.NormalAt(new Point(0, 1.70711, -0.70711));

            Assert.Equal(new Vector(0, 0.70711, -0.70711), n);
        }

        [Fact]
        public void ComputingTheNormalOnATransformedShape()
        {
            var s = new TestShape();
            var m = Matrix.Scaling(1, 0.5, 1) * Matrix.RotationZ(Math.PI / 5);

            s.Transform = m;
            var n = s.NormalAt(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));

            Assert.Equal(new Vector(0, 0.97014, -0.24254), n);
        }
    }
}

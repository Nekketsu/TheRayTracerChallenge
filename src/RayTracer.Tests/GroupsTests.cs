using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tests.Entities;
using RayTracer.Tuples;
using System;
using Xunit;

namespace RayTracer.Tests
{
    public class GroupsTests
    {
        [Fact]
        public void CreatingANewGroup()
        {
            var g = new Group();

            Assert.Equal(Matrix.Identity, g.Transform);
            Assert.Empty(g);
        }

        [Fact]
        public void AShapeHasAParentAttribute()
        {
            var s = new TestShape();

            Assert.Null(s.Parent);
        }

        [Fact]
        public void AddingAChildToAGroup()
        {
            var g = new Group();
            var s = new TestShape();

            g.AddChild(s);

            Assert.NotEmpty(g);
            Assert.Contains(s, g);
            Assert.Equal(g, s.Parent);
        }

        [Fact]
        public void IntersectingARayWithAnEmptyGroup()
        {
            var g = new Group();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            var xs = g.LocalIntersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        public void IntersectingARayWithANonEmptyGroup()
        {
            var g = new Group();
            var s1 = new Sphere();
            var s2 = new Sphere();
            s2.Transform = Matrix.Translation(0, 0, -3);
            var s3 = new Sphere();
            s3.Transform = Matrix.Translation(5, 0, 0);
            g.AddChild(s1);
            g.AddChild(s2);
            g.AddChild(s3);

            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var xs = g.LocalIntersect(r);

            Assert.Equal(4, xs.Length);
            Assert.Equal(s2, xs[0].Object);
            Assert.Equal(s2, xs[1].Object);
            Assert.Equal(s1, xs[2].Object);
            Assert.Equal(s1, xs[3].Object);
        }

        [Fact]
        public void IntersectingATransformedGroup()
        {
            var g = new Group();
            g.Transform = Matrix.Scaling(2, 2, 2);
            var s = new Sphere();
            s.Transform = Matrix.Translation(5, 0, 0);
            g.AddChild(s);

            var r = new Ray(new Point(10, 0, -10), new Vector(0, 0, 1));
            var xs = g.Intersect(r);

            Assert.Equal(2, xs.Length);
        }

        [Fact]
        public void ConvertingAPointFromWorldToObjectSpace()
        {
            var g1 = new Group();
            g1.Transform = Matrix.RotationY(Math.PI / 2);
            var g2 = new Group();
            g2.Transform = Matrix.Scaling(2, 2, 2);
            g1.AddChild(g2);
            var s = new Sphere();
            s.Transform = Matrix.Translation(5, 0, 0);
            g2.AddChild(s);

            var p = s.WorldToObject(new Point(-2, 0, -10));

            Assert.Equal(new Point(0, 0, -1), p);
        }

        [Fact]
        public void ConvertingANormalFromObjectoWorldSpace()
        {
            var g1 = new Group();
            g1.Transform = Matrix.RotationY(Math.PI / 2);
            var g2 = new Group();
            g2.Transform = Matrix.Scaling(1, 2, 3);
            g1.AddChild(g2);
            var s = new Sphere();
            s.Transform = Matrix.Translation(5, 0, 0);
            g2.AddChild(s);

            var n = s.NormalToWorld(new Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));

            Assert.Equal(new Vector(0.2857, 0.4286, -0.8571), n);
        }

        [Fact]
        public void FindingTheNormalOnAChildObject()
        {
            var g1 = new Group();
            g1.Transform = Matrix.RotationY(Math.PI / 2);
            var g2 = new Group();
            g2.Transform = Matrix.Scaling(1, 2, 3);
            g1.AddChild(g2);
            var s = new Sphere();
            s.Transform = Matrix.Translation(5, 0, 0);
            g2.AddChild(s);

            var n = s.NormalAt(new Point(1.7321, 1.1547, -5.5774));

            Assert.Equal(new Vector(0.2857, 0.4286, -0.8571), n);
        }
    }
}

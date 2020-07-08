using RayTracer.Intersections;
using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tests.Entities;
using RayTracer.Tuples;
using System;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{
    public class MaterialsTests
    {
        [Fact]
        public void TheDefaultMaterial()
        {
            var m = new Material();

            Assert.Equal(Color.White, m.Color);
            Assert.Equal(0.1, m.Ambient);
            Assert.Equal(0.9, m.Diffuse);
            Assert.Equal(0.9, m.Specular);
            Assert.Equal(200, m.Shininess);
        }

        [Fact]
        public void LightingWithTheEyeBetweenTheLightAndTheSurface()
        {
            var m = new Material();
            var position = new Point(0, 0, 0);

            var eyeVector = new Vector(0, 0, -1);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));

            var result = m.Lighting(new TestShape(), light, position, eyeVector, normalVector);

            Assert.Equal(new Color(1.9, 1.9, 1.9), result);
        }

        [Fact]
        public void LightingWitTheEyeBetweenLightAndSurfaceEyeOffset45Degrees()
        {
            var m = new Material();
            var position = new Point(0, 0, 0);

            var eyeVector = new Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));

            var result = m.Lighting(new TestShape(), light, position, eyeVector, normalVector);

            Assert.Equal(new Color(1.0, 1.0, 1.0), result);
        }

        [Fact]
        public void LightingWithEyeOppositeSurfaceLightOffset45Degrees()
        {
            var m = new Material();
            var position = new Point(0, 0, 0);

            var eyeVector = new Vector(0, 0, -1);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 10, -10), new Color(1, 1, 1));

            var result = m.Lighting(new TestShape(), light, position, eyeVector, normalVector);

            Assert.Equal(new Color(0.7364, 0.7364, 0.7364), result);
        }

        [Fact]
        public void LightingWithEyeInThePathOfTheReflectionVector()
        {
            var m = new Material();
            var position = new Point(0, 0, 0);

            var eyeVector = new Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 10, -10), new Color(1, 1, 1));

            var result = m.Lighting(new TestShape(), light, position, eyeVector, normalVector);

            Assert.Equal(new Color(1.6364, 1.6364, 1.6364), result);
        }

        [Fact]
        public void LightingWithTheLightBehindTheSurface()
        {
            var m = new Material();
            var position = new Point(0, 0, 0);

            var eyeVector = new Vector(0, 0, -1);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, 10), new Color(1, 1, 1));

            var result = m.Lighting(new TestShape(), light, position, eyeVector, normalVector);

            Assert.Equal(new Color(0.1, 0.1, 0.1), result);
        }

        [Fact]
        public void LightingWithTheSurfaceInShadow()
        {
            var m = new Material();
            var position = new Point(0, 0, 0);

            var eyeVector = new Vector(0, 0, -1);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var inShadow = true;

            var result = m.Lighting(new TestShape(), light, position, eyeVector, normalVector, inShadow);

            Assert.Equal(new Color(0.1, 0.1, 0.1), result);
        }

        [Fact]
        public void ThereIsNoShadowWhenNothingIsCollinearWithPointAndLight()
        {
            var w = World.CreateDefault();
            var p = new Point(0, 10, 0);

            Assert.False(w.IsShadowed(p, w.Lights.First()));
        }

        [Fact]
        public void TheShadowWhenAnObjectIsBetweenThePointAndTheLight()
        {
            var w = World.CreateDefault();
            var p = new Point(10, -10, 10);

            Assert.True(w.IsShadowed(p, w.Lights.First()));
        }

        [Fact]
        public void ThereIsNoShadowWhenAnObjectIsBehindTheLight()
        {
            var w = World.CreateDefault();
            var p = new Point(-20, 20, -20);

            Assert.False(w.IsShadowed(p, w.Lights.First()));
        }

        [Fact]
        public void ThereIsNoShadowWhenAndObjectIsBehindThePoint()
        {
            var w = World.CreateDefault();
            var p = new Point(-2, 2, -2);

            Assert.False(w.IsShadowed(p, w.Lights.First()));
        }

        [Fact]
        public void ReflectivityForTheDefaultMaterial()
        {
            var m = new Material();

            Assert.Equal(0.0, m.Reflective);
        }

        [Fact]
        public void PrecomputingTheReflectionVector()
        {
            var shape = new Plane();
            var r = new Ray(new Point(0, 1, -1), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), shape);

            var computations = i.PrepareComutations(r);

            Assert.Equal(new Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), computations.ReflectVector);
        }

        [Fact]
        public void TransparencyAndRefractiveIndexForTheDefaultMaterial()
        {
            var m = new Material();

            Assert.Equal(0.0, m.Transparency);
            Assert.Equal(1.0, m.RefractiveIndex);
        }

        [Fact]
        public void AHelperForProducingASphereWithAGlassyMaterial()
        {
            var s = new GlassSphere();

            Assert.Equal(Matrix.Identity, s.Transform);
            Assert.Equal(1.0, s.Material.Transparency);
            Assert.Equal(1.5, s.Material.RefractiveIndex);
        }

        [Theory]
        [InlineData(0, 1.0, 1.5)]
        [InlineData(1, 1.5, 2.0)]
        [InlineData(2, 2.0, 2.5)]
        [InlineData(3, 2.5, 2.5)]
        [InlineData(4, 2.5, 1.5)]
        [InlineData(5, 1.5, 1.0)]
        public void FindingN1AndN2AtVariousIntersections(int index, double n1, double n2)
        {
            var a = new GlassSphere();
            a.Transform = Matrix.Scaling(2, 2, 2);
            a.Material.RefractiveIndex = 1.5;
            var b = new GlassSphere();
            b.Transform = Matrix.Translation(0, 0, -0.25);
            b.Material.RefractiveIndex = 2.0;
            var c = new GlassSphere();
            c.Transform = Matrix.Translation(0, 0, 0.25);
            c.Material.RefractiveIndex = 2.5;
            var r = new Ray(new Point(0, 0, -4), new Vector(0, 0, 1));
            var xs = new IntersectionCollection(
                new Intersection(2, a),
                new Intersection(2.75, b),
                new Intersection(3.25, c),
                new Intersection(4.75, b),
                new Intersection(5.25, c),
                new Intersection(6, a)
            );

            var components = xs[index].PrepareComutations(r, xs);

            Assert.Equal(n1, components.N1);
            Assert.Equal(n2, components.N2);
        }
    }
}

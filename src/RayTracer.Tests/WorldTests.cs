using RayTracer.Intersections;
using RayTracer.Lights;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tests.Entities;
using RayTracer.Tuples;
using System;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{
    public class WorldTests
    {
        [Fact]
        public void CreatingAWorld()
        {
            var w = new World();

            Assert.Empty(w.Objects);
            Assert.Empty(w.Lights);
        }

        [Fact]
        public void TheDefaultWorld()
        {
            var light = new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1));
            var s1 = new Sphere();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = 0.7;
            s1.Material.Specular = 0.2;
            var s2 = new Sphere();
            s2.Transform = Matrix.Scaling(0.5, 0.5, 0.5);

            var w = World.CreateDefault();

            Assert.Contains(w.Lights, l => l.Position == light.Position && l.Intensity == light.Intensity);
            Assert.Contains(w.Objects, o => o.Transform == s1.Transform && o.Material == s1.Material);
            Assert.Contains(w.Objects, o => o.Transform == s2.Transform && o.Material == s2.Material);
        }

        [Fact]
        public void IntersectAWorldWithARay()
        {
            var w = World.CreateDefault();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            var xs = w.Intersect(r);

            Assert.Equal(4, xs.Length);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(4.5, xs[1].T);
            Assert.Equal(5.5, xs[2].T);
            Assert.Equal(6, xs[3].T);
        }

        [Fact]
        public void ShadingAnIntersection()
        {
            var w = World.CreateDefault();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = w.Objects.First();
            var i = new Intersection(4, shape);

            var computations = i.PrepareComutations(r);
            var c = w.ShadeHit(computations);

            Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c);
        }

        [Fact]
        public void ShadingAnIntersectionFromTheInside()
        {
            var w = World.CreateDefault();
            w.Lights.Clear();
            w.Lights.Add(new PointLight(new Point(0, 0.25, 0), new Color(1, 1, 1)));
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = w.Objects[1];
            var i = new Intersection(0.5, shape);

            var computations = i.PrepareComutations(r);
            var c = w.ShadeHit(computations);

            Assert.Equal(new Color(0.90498, 0.90498, 0.90498), c);
        }

        [Fact]
        public void TheColorWhenARayMisses()
        {
            var w = World.CreateDefault();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));

            var c = w.ColorAt(r);

            Assert.Equal(Color.Black, c);
        }

        [Fact]
        public void TheColorWhenARayHits()
        {
            var w = World.CreateDefault();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            var c = w.ColorAt(r);

            Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c);
        }

        [Fact]
        public void TheColorWithAnIntersectionBehindTheRay()
        {
            var w = World.CreateDefault();
            var outer = w.Objects.First();
            outer.Material.Ambient = 1;
            var inner = w.Objects[1];
            inner.Material.Ambient = 1;
            var r = new Ray(new Point(0, 0, 0.75), new Vector(0, 0, -1));

            var c = w.ColorAt(r);

            Assert.Equal(inner.Material.Color, c);
        }

        [Fact]
        public void ShadeHitIsGivenAnIntersectionInShadow()
        {
            var w = new World();
            w.Lights.Add(new PointLight(new Point(0, 0, -10), new Color(1, 1, 1)));
            var s1 = new Sphere();
            w.Objects.Add(s1);
            var s2 = new Sphere { Transform = Matrix.Translation(0, 0, 10) };
            w.Objects.Add(s2);
            var r = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var i = new Intersection(4, s2);

            var computations = i.PrepareComutations(r);
            var c = w.ShadeHit(computations);

            Assert.Equal(new Color(0.1, 0.1, 0.1), c);
        }

        [Fact]
        public void TheReflectedColorForANonReflectiveMaterial()
        {
            var w = World.CreateDefault();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = w.Objects[1];
            shape.Material.Ambient = 1;
            var i = new Intersection(1, shape);

            var computations = i.PrepareComutations(r);
            var color = w.ReflectedColor(computations);

            Assert.Equal(Color.Black, color);
        }

        [Fact]
        public void TheReflectedColorForAReflectiveMaterial()
        {
            var w = World.CreateDefault();
            var shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Matrix.Translation(0, -1, 0);
            w.Objects.Add(shape);
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), shape);

            var computations = i.PrepareComutations(r);
            var color = w.ReflectedColor(computations);

            Assert.Equal(new Color(0.19032, 0.2379, 0.14274), color);
        }

        [Fact]
        public void ShadeHitWithAReflectiveMaterial()
        {
            var w = World.CreateDefault();
            var shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Matrix.Translation(0, -1, 0);
            w.Objects.Add(shape);
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), shape);

            var computations = i.PrepareComutations(r);
            var color = w.ShadeHit(computations);

            Assert.Equal(new Color(0.87677, 0.92436, 0.82918), color);
        }

        [Fact(Timeout = 60 * 1000)]
        public void ColorAtWithMutuallyReflectiveSurfaces()
        {
            var w = new World();
            w.Lights.Add(new PointLight(new Point(0, 0, 0), new Color(1, 1, 1)));
            var lower = new Plane();
            lower.Material.Reflective = 1;
            lower.Transform = Matrix.Translation(0, -1, 0);
            w.Objects.Add(lower);
            var upper = new Plane();
            upper.Material.Reflective = 1;
            upper.Transform = Matrix.Translation(0, 1, 0);
            w.Objects.Add(upper);
            try
            {
                var r = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));

                Assert.True(w.ColorAt(r) is Color);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TheReflectedColorAtTheMaximumRecursivePath()
        {
            var w = World.CreateDefault();
            var shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Matrix.Translation(0, -1, 0);
            w.Objects.Add(shape);
            var r = new Ray(new Point(0, 0, 3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), shape);

            var computations = i.PrepareComutations(r);
            var color = w.ReflectedColor(computations, 0);

            Assert.Equal(new Color(0, 0, 0), color);
        }

        [Fact]
        public void TheRefractedColorWithAnOpaqueSurface()
        {
            var w = World.CreateDefault();
            var shape = w.Objects[0];
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var xs = new IntersectionCollection(
                new Intersection(4, shape),
                new Intersection(6, shape)
            );

            var computations = xs[0].PrepareComutations(r, xs);
            var c = w.RefractedColor(computations, 5);

            Assert.Equal(Color.Black, c);
        }

        [Fact]
        public void TheRefradctedColorATheMaximumRecursiveDepth()
        {
            var w = World.CreateDefault();
            var shape = w.Objects[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var xs = new IntersectionCollection(
                new Intersection(4, shape),
                new Intersection(6, shape)
            );

            var computations = xs[0].PrepareComutations(r, xs);
            var c = w.RefractedColor(computations, 0);

            Assert.Equal(Color.Black, c);
        }

        [Fact]
        public void TheRefractedColorUnderTotalInteralReflection()
        {
            var w = World.CreateDefault();
            var shape = w.Objects[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            var r = new Ray(new Point(0, 0, Math.Sqrt(2) / 2), new Vector(0, 1, 0));
            var xs = new IntersectionCollection(
                new Intersection(-Math.Sqrt(2) / 2, shape),
                new Intersection(Math.Sqrt(2) / 2, shape)
            );

            // NOTE: this time you're inside the sphere, so you need
            // to look at the second intersection, xs[1], not xs[2]
            var computations = xs[1].PrepareComutations(r, xs);
            var c = w.ReflectedColor(computations, 5);

            Assert.Equal(Color.Black, c);
        }

        [Fact]
        public void TheRefractedColorWithRefractedRay()
        {
            var w = World.CreateDefault();
            var a = w.Objects[0];
            a.Material.Ambient = 1.0;
            a.Material.Pattern = new TestPattern();
            var b = w.Objects[1];
            b.Material.Transparency = 1.0;
            b.Material.RefractiveIndex = 1.5;
            var r = new Ray(new Point(0, 0, 0.1), new Vector(0, 1, 0));
            var xs = new IntersectionCollection(
                new Intersection(-0.9899, a),
                new Intersection(-0.4899, b),
                new Intersection(0.4899, b),
                new Intersection(0.9899, a)
            );

            var computations = xs[2].PrepareComutations(r, xs);
            var c = w.RefractedColor(computations, 5);

            Assert.Equal(new Color(0, 0.99888, 0.04725), c);
        }

        [Fact]
        public void ShadeHitWithATransparentMaterial()
        {
            var w = World.CreateDefault();
            var floor = new Plane();
            floor.Transform = Matrix.Translation(0, -1, 0);
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            w.Objects.Add(floor);
            var ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Matrix.Translation(0, -3.5, -0.5);
            w.Objects.Add(ball);
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var xs = new IntersectionCollection(new Intersection(Math.Sqrt(2), floor));

            var computations = xs[0].PrepareComutations(r, xs);
            var color = w.ShadeHit(computations, 5);

            Assert.Equal(new Color(0.93642, 0.68642, 0.68642), color);
        }

        [Fact]
        public void ShadeHitWithAReflectiveTransparentMaterial()
        {
            var w = World.CreateDefault();
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var floor = new Plane();
            floor.Transform = Matrix.Translation(0, -1, 0);
            floor.Material.Reflective = 0.5;
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            w.Objects.Add(floor);
            var ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Matrix.Translation(0, -3.5, -0.5);
            w.Objects.Add(ball);
            var xs = new IntersectionCollection(new Intersection(Math.Sqrt(2), floor));

            var computations = xs[0].PrepareComutations(r, xs);
            var color = w.ShadeHit(computations, 5);

            Assert.Equal(new Color(0.93391, 0.69643, 0.69243), color);
        }
    }
}

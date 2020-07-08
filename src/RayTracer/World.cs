using RayTracer.Intersections;
using RayTracer.Lights;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class World
    {
        const int DefaultRemaining = 5;

        public static World CreateDefault()
        {
            var light = new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1));

            var s1 = new Sphere();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = 0.7;
            s1.Material.Specular = 0.2;

            var s2 = new Sphere();
            s2.Transform = Matrix.Scaling(0.5, 0.5, 0.5);

            var world = new World();
            world.Lights.Add(light);
            world.Objects.Add(s1);
            world.Objects.Add(s2);

            return world;
        }

        public List<Shape> Objects { get; }
        public List<PointLight> Lights { get; }
        public bool HasShadows { get; set; }
        public bool HasReflections { get; set; }
        public bool HasRefractions { get; set; }

        public World()
        {
            Objects = new List<Shape>();
            Lights = new List<PointLight>();
            HasShadows = true;
            HasReflections = true;
            HasRefractions = true;
        }

        public IntersectionCollection Intersect(Ray ray)
        {
            var intersections = Objects.SelectMany(o => o.Intersect(ray)).ToArray();

            return new IntersectionCollection(intersections);
        }

        public Color ShadeHit(Computations computations, int remaining = DefaultRemaining)
        {
            var color = Color.Black;

            foreach (var light in Lights)
            {
                var isShadowed = HasShadows && IsShadowed(computations.OverPoint, light);

                var surface = computations.Object.Material.Lighting(
                    computations.Object, light, computations.OverPoint, computations.EyeVector, computations.NormalVector, isShadowed);

                var reflected = HasReflections ? ReflectedColor(computations, remaining) : Color.Black;
                var refracted = HasRefractions ? RefractedColor(computations, remaining) : Color.Black;

                var material = computations.Object.Material;
                if (material.Reflective > 0 && material.Transparency > 0)
                {
                    var reflectance = computations.Schlick();
                    color += surface + reflected * reflectance + refracted * (1 - reflectance);
                }
                else
                {
                    color += surface + reflected + refracted;
                }
            }

            return color;
        }

        public Color ColorAt(Ray ray, int remaining = DefaultRemaining)
        {
            var xs = Intersect(ray);
            if (xs.Hit == null)
            {
                return Color.Black;
            }

            var computations = xs.Hit.PrepareComutations(ray);

            return ShadeHit(computations, remaining);
        }

        public bool IsShadowed(Point point, PointLight light)
        {
            var v = light.Position - point;
            var distance = v.Length;
            var direction = v.Normalize();

            var r = new Ray(point, direction);
            var intersections = Intersect(r);

            var hit = intersections.Hit;
            return hit != null && hit.T < distance;
        }

        public Color ReflectedColor(Computations computations, int remainig = DefaultRemaining)
        {
            if (computations.Object.Material.Reflective == 0)
            {
                return Color.Black;
            }
            if (remainig <= 0)
            {
                return Color.Black;
            }

            var reflectiveRay = new Ray(computations.OverPoint, computations.ReflectVector);
            var color = ColorAt(reflectiveRay, remainig - 1);

            return color * computations.Object.Material.Reflective;
        }

        public Color RefractedColor(Computations computations, int remaining = DefaultRemaining)
        {
            if (computations.Object.Material.Transparency == 0)
            {
                return Color.Black;
            }
            if (remaining <= 0)
            {
                return Color.Black;
            }

            // Find the ratio of first index of refraction to the second.
            // (Yup, this is inverted from the definition of Snell's Law.)
            var nRatio = computations.N1 / computations.N2;

            // cos(theta_i) is the same as the dot product of the two vectors
            var cosI = computations.EyeVector * computations.NormalVector;

            // Find sin(theta_t)^2 via trigonometric indentity
            var sin2T = nRatio * nRatio * (1 - cosI * cosI);

            if (sin2T > 1)
            {
                return Color.Black;
            }

            // Find cos(theta_t) via trigonometric identity
            var cosT = Math.Sqrt(1 - sin2T);

            // Compute the direction of the refracted ray
            var direction = computations.NormalVector * (nRatio * cosI - cosT) - computations.EyeVector * nRatio;

            // Create the refracted ray
            var refractedRay = new Ray(computations.UnderPoint, direction);

            // Find the color of the refracted ray, making sure to multiply
            // by the transparency value to account for any opacity
            var color = ColorAt(refractedRay, remaining - 1) * computations.Object.Material.Transparency;

            return color;
        }
    }
}

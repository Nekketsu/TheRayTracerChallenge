using RayTracer.Extensions;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Intersections
{
    public class Intersection
    {
        public double T { get; }
        public Shape Object { get; }

        public Intersection(double t, Shape @object)
        {
            T = t;
            Object = @object;
        }

        public Computations PrepareComutations(Ray ray, IntersectionCollection intersections = null)
        {
            // Instantiate a data structure for storing some precomputed values
            //var computations = new Computations();

            // Copy the intersection's properties, for convenience
            //computations.T = T;
            //computations.Shape = Shape;

            // Precompute some useful values
            var point = ray.Position(T);
            var eyeVector = -ray.Direction;
            var normalVector = NormalAt(point);
            var inside = normalVector * eyeVector < 0;
            normalVector = inside ? -normalVector : normalVector;
            var reflectVector = ray.Direction.Reflect(normalVector);
            var overPoint = point + normalVector * DoubleExtensions.Epsilon;
            var underPoint = point - normalVector * DoubleExtensions.Epsilon;

            if (intersections == null)
            {
                intersections = new IntersectionCollection(this);
            }
            var n1 = 0.0;
            var n2 = 0.0;
            var container = new List<Shape>();
            foreach (var intersection in intersections)
            {
                if (intersection == this)
                {
                    if (!container.Any())
                    {
                        n1 = 1.0;
                    }
                    else
                    {
                        n1 = container.Last().Material.RefractiveIndex;
                    }
                }

                if (container.Contains(intersection.Object))
                {
                    container.Remove(intersection.Object);
                }
                else
                {
                    container.Add(intersection.Object);
                }

                if (intersection == this)
                {
                    if (!container.Any())
                    {
                        n2 = 1.0;
                    }
                    else
                    {
                        n2 = container.Last().Material.RefractiveIndex;
                    }
                }
            }

            return new Computations(T, Object, point, eyeVector, normalVector, inside, overPoint, underPoint, reflectVector, n1, n2);
        }

        protected virtual Vector NormalAt(Point point)
        {
            return Object.NormalAt(point);
        }
    }
}

using RayTracer.Extensions;
using RayTracer.Intersections;
using RayTracer.Tuples;
using System;
using System.Collections.Generic;

namespace RayTracer.Shapes
{
    public class Cone : Shape
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public bool Closed { get; set; }

        public Cone()
        {
            Minimum = double.NegativeInfinity;
            Maximum = double.PositiveInfinity;
            Closed = false;
        }

        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            var intersections = new List<Intersection>();

            var a = ray.Direction.X * ray.Direction.X - ray.Direction.Y * ray.Direction.Y + ray.Direction.Z * ray.Direction.Z;

            var b = 2 * ray.Origin.X * ray.Direction.X -
                    2 * ray.Origin.Y * ray.Direction.Y +
                    2 * ray.Origin.Z * ray.Direction.Z;

            var c = ray.Origin.X * ray.Origin.X - ray.Origin.Y * ray.Origin.Y + ray.Origin.Z * ray.Origin.Z;

            // Ray is parallel to the y axis
            if (!a.EqualsEpsilon(0))
            {

                var disc = b * b - 4 * a * c;

                // Ray does not intersect the cylinder
                if (disc < 0)
                {
                    return new IntersectionCollection();
                }
                var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
                var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

                if (t0 > t1)
                {
                    (t0, t1) = (t1, t0);
                }

                var y0 = ray.Origin.Y + t0 * ray.Direction.Y;
                if (Minimum < y0 && y0 < Maximum)
                {
                    intersections.Add(new Intersection(t0, this));
                }

                var y1 = ray.Origin.Y + t1 * ray.Direction.Y;
                if (Minimum < y1 && y1 < Maximum)
                {
                    intersections.Add(new Intersection(t1, this));
                }
            }
            else if (b != 0)
            {
                var t = -c / (2 * b);
                intersections.Add(new Intersection(t, this));
            }

            IntersectCaps(ray, intersections);


            return new IntersectionCollection(intersections.ToArray());
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            // Compute the square of the distance from the y axis
            var distance = point.X * point.X + point.Z * point.Z;

            if (distance < 1 && point.Y >= Maximum - DoubleExtensions.Epsilon)
            {
                return new Vector(0, 1, 0);
            }
            else if (distance < 1 && point.Y <= Minimum + DoubleExtensions.Epsilon)
            {
                return new Vector(0, -1, 0);
            }
            else
            {
                var y = Math.Sqrt(point.X * point.X + point.Z * point.Z);
                if (point.Y > 0)
                {
                    y = -y;
                }
                return new Vector(point.X, y, point.Z);
            }
        }

        // A helper function to reduce duplication.
        // Checks to see if the intersection at 't' is within a radius
        // of 1 (the radius of your cylinders) from the y axis.
        private bool CheckCap(Ray ray, double t, double radius)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            return x * x + z * z <= radius;
        }

        private void IntersectCaps(Ray ray, List<Intersection> intersections)
        {
            // Caps only matter if the cylinder is closed and might possibly be
            // intersected by the ray.
            if (!Closed || ray.Direction.Y.EqualsEpsilon(0))
            {
                return;
            }

            // Check for an intersection with the lower end cap by intersecting
            // the ray with the plane at y = cyl.Minimum
            var t = (Minimum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t, Math.Abs(Minimum)))
            {
                intersections.Add(new Intersection(t, this));
            }

            // Check for an intersection with the upper end cap by intersecting
            // the ray with the plane at y = cyl.Maximum
            t = (Maximum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t, Math.Abs(Maximum)))
            {
                intersections.Add(new Intersection(t, this));
            }
        }

    }
}

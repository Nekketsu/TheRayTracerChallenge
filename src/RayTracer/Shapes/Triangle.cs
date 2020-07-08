using RayTracer.Extensions;
using RayTracer.Intersections;
using RayTracer.Tuples;
using System;

namespace RayTracer.Shapes
{
    public class Triangle : Shape
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Point P3 { get; }

        public Vector E1 { get; }
        public Vector E2 { get; }

        public Vector Normal { get; }

        public Triangle(Point p1, Point p2, Point p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            E1 = p2 - p1;
            E2 = p3 - p1;

            Normal = E2.Cross(E1).Normalize();
        }

        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            var directionCrossE2 = ray.Direction.Cross(E2);
            var determinant = E1 * directionCrossE2;

            if (Math.Abs(determinant) < DoubleExtensions.Epsilon)
            {
                return new IntersectionCollection();
            }

            var f = 1.0 / determinant;

            var p1ToOrigin = ray.Origin - P1;
            var u = f * p1ToOrigin * directionCrossE2;

            if (u < 0 || u > 1)
            {
                return new IntersectionCollection();
            }

            var originCrossE1 = p1ToOrigin.Cross(E1);
            var v = f * ray.Direction * originCrossE1;

            if (v < 0 || (u + v) > 1)
            {
                return new IntersectionCollection();
            }

            var t = f * E2 * originCrossE1;

            return new IntersectionCollection(new IntersectionWithUV(t, this, u, v));
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            return Normal;
        }

        public override bool Equals(object obj)
        {
            if (obj is Triangle triangle)
            {
                return base.Equals(obj) || (
                       P1.Equals(triangle.P1) &&
                       P2.Equals(triangle.P2) &&
                       P3.Equals(triangle.P3));
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(P1, P2, P3);
        }
    }
}

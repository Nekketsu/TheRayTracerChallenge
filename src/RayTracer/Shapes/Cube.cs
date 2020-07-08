using RayTracer.Intersections;
using RayTracer.Tuples;
using System;

namespace RayTracer.Shapes
{
    public class Cube : Shape
    {
        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            var (xTMin, xTMax) = CheckAxis(ray.Origin.X, ray.Direction.X);
            var (yTMin, yTMax) = CheckAxis(ray.Origin.Y, ray.Direction.Y);
            var (zTMin, zTMax) = CheckAxis(ray.Origin.Z, ray.Direction.Z);

            var tMin = Math.Max(xTMin, Math.Max(yTMin, zTMin));
            var tMax = Math.Min(xTMax, Math.Min(yTMax, zTMax));

            if (tMin > tMax)
            {
                return new IntersectionCollection();
            }

            return new IntersectionCollection(
                new Intersection(tMin, this),
                new Intersection(tMax, this)
            );
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            var maxComponent = Math.Max(Math.Abs(point.X), Math.Max(Math.Abs(point.Y), Math.Abs(point.Z)));

            if (maxComponent == Math.Abs(point.X))
            {
                return new Vector(point.X, 0, 0);
            }
            else if (maxComponent == Math.Abs(point.Y))
            {
                return new Vector(0, point.Y, 0);
            }
            else // if (maxComponent == Math.Abs(localPoint.Z))
            {
                return new Vector(0, 0, point.Z);
            }
        }

        private (double tMin, double tMax) CheckAxis(double origin, double direction)
        {
            var tMinNumerator = -1 - origin;
            var tMaxNumerator = 1 - origin;

            double tMin;
            double tMax;

            tMin = tMinNumerator / direction;
            tMax = tMaxNumerator / direction;

            //if (Math.Abs(direction) >= DoubleExtensions.Epsilon)
            //{
            //    tMin = tMinNumerator / direction;
            //    tMax = tMaxNumerator / direction;
            //}
            //else
            //{
            //    tMin = tMinNumerator * double.PositiveInfinity;
            //    tMax = tMaxNumerator * double.PositiveInfinity;
            //}

            if (tMin > tMax)
            {
                (tMin, tMax) = (tMax, tMin);
            }

            return (tMin, tMax);
        }
    }
}

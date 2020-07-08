using RayTracer.Intersections;
using RayTracer.Tuples;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Shapes
{
    public abstract class Csg : Shape
    {
        public Shape Left { get; }
        public Shape Right { get; }

        protected Csg(Shape left, Shape right)
        {
            Left = left;
            Right = right;

            left.Parent = this;
            right.Parent = this;
        }

        public static CsgUnion Union(Shape left, Shape right)
        {
            return new CsgUnion(left, right);
        }

        public static CsgIntersection Intersection(Shape left, Shape right)
        {
            return new CsgIntersection(left, right);
        }

        public static CsgDifference Difference(Shape left, Shape right)
        {
            return new CsgDifference(left, right);
        }

        public abstract bool IntersectionAllowed(bool leftHit, bool InLeft, bool inRight);

        public IntersectionCollection FilterIntersections(IntersectionCollection xs)
        {
            // Begin outside of both children
            var inLeft = false;
            var inRight = false;

            // Prepare a list to receive the filtered intersections
            var result = new List<Intersection>();

            foreach (var intersection in xs)
            {
                // If intersection.Object is part of the "left" child, then leftHit is true
                var leftHit = Left.Includes(intersection.Object);

                if (IntersectionAllowed(leftHit, inLeft, inRight))
                {
                    result.Add(intersection);
                }

                // Depending on which object was hit, toggle either inLeft or inRight
                if (leftHit)
                {
                    inLeft = !inLeft;
                }
                else
                {
                    inRight = !inRight;
                }
            }

            return new IntersectionCollection(result.ToArray());
        }


        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            var leftXs = Left.Intersect(ray);
            var rightXs = Right.Intersect(ray);

            var xs = new IntersectionCollection(leftXs.Union(rightXs).ToArray());

            return FilterIntersections(xs);
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            throw new System.NotImplementedException();
        }

        public override bool Includes(Shape shape)
        {
            return Left.Includes(shape) ||
                   Right.Includes(shape);
        }
    }
}

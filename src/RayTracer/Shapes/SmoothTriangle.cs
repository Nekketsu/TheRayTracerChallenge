using RayTracer.Intersections;
using RayTracer.Tuples;
using System;

namespace RayTracer.Shapes
{
    public class SmoothTriangle : Triangle
    {
        public Vector N1 { get; }
        public Vector N2 { get; }
        public Vector N3 { get; }

        public SmoothTriangle(Point p1, Point p2, Point p3, Vector n1, Vector n2, Vector n3) : base(p1, p2, p3)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            if (hit != null)
            {
                return N2 * hit.U +
                       N3 * hit.V +
                       N1 * (1 - hit.U - hit.V);
            }

            return Normal;
        }

        public override bool Equals(object obj)
        {
            if (obj is SmoothTriangle smoothTriangle)
            {
                return base.Equals(smoothTriangle) &&
                       N1.Equals(smoothTriangle.N1) &&
                       N2.Equals(smoothTriangle.N2) &&
                       N3.Equals(smoothTriangle.N3);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(P1, P2, P3, N1, N2, N3);
        }
    }
}

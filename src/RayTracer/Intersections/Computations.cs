using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Intersections
{
    public class Computations
    {
        public double T { get; }
        public Shape Object { get; }
        public Point Point { get; }
        public Vector EyeVector { get; }
        public Vector NormalVector { get; }
        public bool Inside { get; }
        public Point OverPoint { get; }
        public Point UnderPoint { get; }
        public Vector ReflectVector { get; }
        public double N1 { get; }
        public double N2 { get; }

        public Computations(double t, Shape @object, Point point, Vector eyeVector, Vector normalVector, bool inside, Point overPoint, Point underPoint, Vector reflectVector, double n1, double n2)
        {
            T = t;
            Object = @object;
            Point = point;
            EyeVector = eyeVector;
            NormalVector = normalVector;
            Inside = inside;
            OverPoint = overPoint;
            UnderPoint = underPoint;
            ReflectVector = reflectVector;
            N1 = n1;
            N2 = n2;
        }

        public double Schlick()
        {
            // Find the cosine of the angle between the eye and the normal vectors
            var cos = EyeVector * NormalVector;

            // Total internal reflectaion ccan only occur if n1 > n2
            if (N1 > N2)
            {
                var n = N1 / N2;
                var sin2T = n * n * (1.0 - cos * cos);

                if (sin2T > 1.0)
                {
                    return 1.0;
                }

                // Compute cosine of theta_t using trigonometry identity
                var cosT = Math.Sqrt(1.0 - sin2T);

                // When n1 > n2, use cos(theta_t) instead
                cos = cosT;
            }

            var r0 = Math.Pow((N1 - N2) / (N1 + N2), 2);

            return r0 + (1 - r0) * Math.Pow((1 - cos), 5);
        }
    }
}

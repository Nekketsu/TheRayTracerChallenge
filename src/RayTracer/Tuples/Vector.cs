using RayTracer.Matrices;
using System;

namespace RayTracer.Tuples
{
    public class Vector : Tuple
    {
        public static Vector Zero => new Vector(0, 0, 0);

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public Vector(double x, double y, double z) : base(x, y, z, 0) { }

        public Vector Normalize()
        {
            var length = Length;

            return new Vector(X / length, Y / length, Z / length);
        }

        public Vector Add(Vector v)
        {
            return new Vector(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Vector Substract(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y, Z - v.Z);
        }

        public Vector Negate()
        {
            return new Vector(-X, -Y, -Z);
        }

        public Vector Multiply(double escalar)
        {
            return new Vector(X * escalar, Y * escalar, Z * escalar);
        }

        public Vector Divide(double escalar)
        {
            return new Vector(X / escalar, Y / escalar, Z / escalar);
        }

        public double Dot(Vector v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public Vector Cross(Vector v)
        {
            return new Vector(Y * v.Z - Z * v.Y,
                              Z * v.X - X * v.Z,
                              X * v.Y - Y * v.X);
        }

        public Vector Reflect(Vector normal)
        {
            return Substract(normal * 2 * Dot(normal));
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static explicit operator Vector(Matrix matrix)
        {
            if (matrix.Rows != 4 || matrix.Columns != 1 ||
                matrix[3, 0] != 0)
            {
                throw new InvalidOperationException();
            }

            return new Vector(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        }


        public static Vector operator +(Vector v1, Vector v2)
        {
            return v1.Add(v2);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1.Substract(v2);
        }

        public static Vector operator -(Vector v)
        {
            return v.Negate();
        }

        public static Vector operator *(Vector v, double escalar)
        {
            return v.Multiply(escalar);
        }

        public static Vector operator *(double escalar, Vector v)
        {
            return v.Multiply(escalar);
        }

        public static Vector operator /(Vector v, double escalar)
        {
            return v.Divide(escalar);
        }

        public static double operator *(Vector v1, Vector v2)
        {
            return v1.Dot(v2);
        }
    }
}

using RayTracer.Matrices;
using System;

namespace RayTracer.Tuples
{
    public class Point : Tuple
    {
        public static Point Origin => new Point(0, 0, 0);

        public Point(double x, double y, double z) : base(x, y, z, 1) { }

        public Point Add(Vector v)
        {
            return new Point(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Point Substract(Vector v)
        {
            return new Point(X - v.X, Y - v.Y, Z - v.Z);
        }

        public Vector Substract(Point p)
        {
            return new Vector(X - p.X, Y - p.Y, Z - p.Z);
        }

        public static explicit operator Point(Matrix matrix)
        {
            if (matrix.Rows != 4 || matrix.Columns != 1 ||
                matrix[3, 0] != 1)
            {
                throw new InvalidOperationException();
            }

            return new Point(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }


        public static Point operator +(Point p, Vector v)
        {
            return p.Add(v);
        }

        public static Point operator -(Point p, Vector v)
        {
            return p.Substract(v);
        }

        public static Vector operator -(Point p1, Point p2)
        {
            return p1.Substract(p2);
        }
    }
}

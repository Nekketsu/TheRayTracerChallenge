using RayTracer.Intersections;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Tuples;

namespace RayTracer.Shapes
{
    public abstract class Shape
    {
        public Matrix Transform { get; set; }
        public Material Material { get; set; }
        public Shape Parent { get; set; }

        public Shape()
        {
            Transform = Matrix.Identity;
            Material = new Material();
            Parent = null;
        }

        public IntersectionCollection Intersect(Ray ray)
        {
            var localRay = ray.Transform(Transform.Inverse());

            return LocalIntersect(localRay);
        }

        public Vector NormalAt(Point worldPoint, IntersectionWithUV hit = null)
        {
            var localPoint = WorldToObject(worldPoint);
            var localNormal = LocalNormalAt(localPoint, hit);

            return NormalToWorld(localNormal);
        }

        public Point WorldToObject(Point point)
        {
            if (Parent != null)
            {
                point = Parent.WorldToObject(point);
            }

            return (Point)(Transform.Inverse() * point);
        }

        public Vector NormalToWorld(Vector normal)
        {
            var normalMatrix = Transform.Inverse().Transpose() * normal;

            normal = new Vector(normalMatrix[0, 0], normalMatrix[1, 0], normalMatrix[2, 0]);
            normal = normal.Normalize();

            if (Parent != null)
            {
                normal = Parent.NormalToWorld(normal);
            }

            return normal;
        }

        public virtual bool Includes(Shape shape)
        {
            return this == shape;
        }

        public abstract IntersectionCollection LocalIntersect(Ray ray);

        public abstract Vector LocalNormalAt(Point point, IntersectionWithUV hit = null);


        public CsgUnion Union(Shape shape)
        {
            return Csg.Union(this, shape);
        }

        public CsgIntersection Intersection(Shape shape)
        {
            return Csg.Intersection(this, shape);
        }

        public CsgDifference Difference(Shape shape)
        {
            return Csg.Difference(this, shape);
        }

        public static CsgUnion operator +(Shape left, Shape right)
        {
            return left.Union(right);
        }

        public static CsgIntersection operator *(Shape left, Shape right)
        {
            return left.Intersection(right);
        }

        public static CsgDifference operator -(Shape left, Shape right)
        {
            return left.Difference(right);
        }
    }
}

using RayTracer.Intersections;
using RayTracer.Tuples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Shapes
{
    public class Group : Shape, IEnumerable<Shape>
    {
        private List<Shape> shapes;

        public Group(IEnumerable<Shape> shapes) : this()
        {
            AddChildren(shapes);
        }

        public Group()
        {
            shapes = new List<Shape>();
        }

        public void AddChild(Shape shape)
        {
            shapes.Add(shape);
            shape.Parent = this;
        }

        public void AddChildren(IEnumerable<Shape> shapes)
        {
            foreach (var shape in shapes)
            {
                AddChild(shape);
            }
        }

        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            var intersects = shapes.SelectMany(shape => shape.Intersect(ray)).ToArray();

            return new IntersectionCollection(intersects);
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            throw new NotImplementedException();
        }

        public Shape this[int i] => shapes[i];

        public IEnumerator<Shape> GetEnumerator()
        {
            return shapes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Includes(Shape shape)
        {
            return shapes.Any(s => s.Includes(shape));
        }
    }
}

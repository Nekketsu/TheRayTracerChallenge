using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Intersections
{
    public class IntersectionCollection : IEnumerable<Intersection>
    {
        private Intersection[] Intersections { get; }

        public Intersection Hit { get; }
        public int Length => Intersections.Length;

        public IntersectionCollection(params Intersection[] intersections)
        {
            Intersections = intersections.OrderBy(i => i.T).ToArray();
            Hit = Intersections.FirstOrDefault(i => i.T >= 0);
        }

        public Intersection this[int i] => Intersections[i];

        public IEnumerator<Intersection> GetEnumerator()
        {
            return Intersections.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

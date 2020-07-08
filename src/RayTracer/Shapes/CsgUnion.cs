namespace RayTracer.Shapes
{
    public class CsgUnion : Csg
    {

        public CsgUnion(Shape left, Shape right) : base(left, right) { }

        public override bool IntersectionAllowed(bool leftHit, bool InLeft, bool inRight)
        {
            return (leftHit && !inRight) || (!leftHit && !InLeft);
        }
    }
}

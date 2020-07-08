using RayTracer.Tuples;

namespace RayTracer.Lights
{
    public class PointLight
    {
        public Point Position { get; }
        public Color Intensity { get; }

        public PointLight(Point position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}

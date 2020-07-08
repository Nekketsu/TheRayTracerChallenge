namespace RayTracer.Shapes
{
    public class GlassSphere : Sphere
    {
        public GlassSphere()
        {
            Material.Transparency = 1.0;
            Material.RefractiveIndex = 1.5;
        }
    }
}

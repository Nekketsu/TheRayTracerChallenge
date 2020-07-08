using RayTracer.Lights;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class LightsTests
    {
        [Fact]
        public void APointLightHasAPositionAndIntensity()
        {
            var intensity = new Color(1, 1, 1);
            var position = new Point(0, 0, 0);

            var light = new PointLight(position, intensity);

            Assert.Equal(position, light.Position);
            Assert.Equal(intensity, light.Intensity);
        }
    }
}

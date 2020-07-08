using System.Linq;
using Xunit;

namespace RayTracer.Demos.Tests
{
    public class DemosTests
    {
        [Fact]
        public void EnsureDemosHaveNameAndOrder()
        {
            var iDemo = typeof(IDemo);

            var demos = iDemo.Assembly.GetTypes()
                .Where(t => iDemo.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var demoType in demos)
            {
                var nameProperty = demoType.GetProperty(nameof(IDemo.Name));
                var orderProperty = demoType.GetProperty(nameof(IDemo.Order));

                Assert.NotNull(nameProperty);
                Assert.NotNull(orderProperty);

                Assert.True(nameProperty.GetValue(null) is string name && name != null);
                Assert.True(orderProperty.GetValue(null) is int order && order > 0);
            }
        }
    }
}

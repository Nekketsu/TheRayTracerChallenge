using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Demos.Services
{
    public class DemoService
    {
        public IEnumerable<string> GetDemos()
        {
            var iDemo = typeof(IDemo);

            var demos = iDemo.Assembly.GetTypes()
                .Where(t => iDemo.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => new
                {
                    Name = (string)t.GetProperty(nameof(IDemo.Name)).GetValue(null),
                    Order = (int)t.GetProperty(nameof(IDemo.Order)).GetValue(null)
                })
                .OrderBy(t => t.Order)
                .Select(t => t.Name);

            return demos;
        }

        public IDemo CreateDemo(string name)
        {
            var iDemo = typeof(IDemo);

            var demoType = iDemo.Assembly.GetTypes()
                .SingleOrDefault(t => iDemo.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract &&
                                      (string)t.GetProperty(nameof(IDemo.Name)).GetValue(null) == name);

            if (demoType == null)
            {
                return null;
            }

            var demo = (IDemo)Activator.CreateInstance(demoType);

            return demo;
        }
    }
}

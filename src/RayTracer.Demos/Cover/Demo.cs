using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Cover
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 18;
        public static string Name { get; } = "Cover";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var world = new World();

            // The camera
            var camera = new Camera(100, 100, 0.785)
            {
                Transform = Matrix.View(new Point(-6, 6, -10),
                                        new Point(6, 0, 6),
                                        new Vector(-0.45, 1, 0))
            };

            // Lights sources
            world.Lights.Add(new PointLight(new Point(50, 100, -50), Color.White));

            // An optiona second light for additional illumination
            world.Lights.Add(new PointLight(new Point(-400, 50, -10), new Color(0.2, 0.2, 0.2)));


            // Define some constants to avoid dupplication
            var whiteMaterial = new Material
            {
                Color = Color.White,
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1
            };

            var blueMaterial = new Material
            {
                Color = new Color(0.537, 0.831, 0.914),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1
            };

            var redMaterial = new Material
            {
                Color = new Color(0.941, 0.322, 0.388),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1
            };

            var purpleMaterial = new Material
            {
                Color = new Color(0.373, 0.404, 0.550),
                Diffuse = 0.7,
                Ambient = 0.1,
                Specular = 0.0,
                Reflective = 0.1
            };


            var standardTransform = Matrix.Translation(1, -1, 1).Scale(0.5, 0.5, 0.5);
            var largeObject = standardTransform.Scale(3.5, 3.5, 3.5);
            var mediumObject = standardTransform.Scale(3, 3, 3);
            var smallObject = standardTransform.Scale(2, 2, 2);


            // A white backdrop for the scene
            world.Objects.Add(new Plane
            {
                Material = new Material
                {
                    Color = Color.White,
                    Ambient = 1,
                    Diffuse = 0,
                    Specular = 0,
                },
                Transform = Matrix.RotationX(Math.PI / 2).Translate(0, 0, 500)
            });


            // Describe the elements of the scene
            world.Objects.Add(new Sphere
            {
                Material = new Material
                {
                    Color = new Color(0.373, 0.404, 0.550),
                    Diffuse = 0.2,
                    Ambient = 0.0,
                    Specular = 1.0,
                    Shininess = 200,
                    Reflective = 0.7,
                    Transparency = 0.7,
                    RefractiveIndex = 1.5
                },
                Transform = largeObject
            });


            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = mediumObject.Translate(4, 0, 0)
            });

            world.Objects.Add(new Cube
            {
                Material = blueMaterial,
                Transform = largeObject.Translate(8.5, 1.5, -0.5)
            });

            world.Objects.Add(new Cube
            {
                Material = redMaterial,
                Transform = largeObject.Translate(0, 0, 4)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = smallObject.Translate(4, 0, 4)
            });

            world.Objects.Add(new Cube
            {
                Material = purpleMaterial,
                Transform = mediumObject.Translate(7.5, 0.5, 4)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = mediumObject.Translate(-0.25, 0.25, 8)
            });

            world.Objects.Add(new Cube
            {
                Material = blueMaterial,
                Transform = largeObject.Translate(4, 1, 7.5)
            });

            world.Objects.Add(new Cube
            {
                Material = redMaterial,
                Transform = mediumObject.Translate(10, 2, 7.5)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = smallObject.Translate(8, 2, 12)
            });


            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = smallObject.Translate(20, 1, 9)
            });

            world.Objects.Add(new Cube
            {
                Material = blueMaterial,
                Transform = largeObject.Translate(-0.5, -5, 0.25)
            });

            world.Objects.Add(new Cube
            {
                Material = redMaterial,
                Transform = largeObject.Translate(4, -4, 0)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = largeObject.Translate(8.5, -4, 0)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = largeObject.Translate(0, -4, 4)
            });

            world.Objects.Add(new Cube
            {
                Material = purpleMaterial,
                Transform = largeObject.Translate(-0.5, -4.5, 8)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = largeObject.Translate(0, -8, 4)
            });

            world.Objects.Add(new Cube
            {
                Material = whiteMaterial,
                Transform = largeObject.Translate(-0.5, -8.5, 8)
            });

            var canvas = camera.Render(world);

            return canvas;
        }
    }
}

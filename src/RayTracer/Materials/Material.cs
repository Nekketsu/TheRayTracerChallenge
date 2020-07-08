using RayTracer.Lights;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Materials
{
    public class Material
    {
        public Color Color { get; set; }
        public double Ambient { get; set; }
        public double Diffuse { get; set; }
        public double Specular { get; set; }
        public double Shininess { get; set; }
        public Pattern Pattern { get; set; }
        public double Reflective { get; set; }
        public double Transparency { get; set; }
        public double RefractiveIndex { get; set; }

        public Material()
        {
            Color = Color.White;
            Ambient = 0.1;
            Diffuse = 0.9;
            Specular = 0.9;
            Shininess = 200;
            Pattern = null;
            Reflective = 0;
            Transparency = 0.0;
            RefractiveIndex = 1.0;
        }

        public Color Lighting(Shape shape, PointLight light, Point point, Vector eyeVector, Vector normalVector, bool isShadowed = false)
        {
            var color = Pattern != null ? Pattern.PatternAtShape(shape, point) : Color;

            // Combine the surface color with the light's color/intensity
            var effectiveColor = color * light.Intensity;

            // Find the direction of the light source
            var lightVector = (light.Position - point).Normalize();

            // Compute the ambient contribution
            var ambient = effectiveColor * Ambient;

            if (isShadowed)
            {
                return ambient;
            }

            // lightDotNormal represents the cosine of the angle between the
            // light vector and the normal vector. A negative number means the
            // light is on the other side of the surface.
            var lightDotNormal = lightVector * normalVector;
            Color diffuse;
            Color specular;
            if (lightDotNormal < 0)
            {
                diffuse = Color.Black;
                specular = Color.Black;
            }
            else
            {
                // Compute the diffuse contribution
                diffuse = effectiveColor * Diffuse * lightDotNormal;

                // reflectDotEye represents the cosine of the angle between the
                // reflection vctor and the eye vector. A negative number means the
                // light reflects away from the eye.
                var reflectVector = (-lightVector).Reflect(normalVector);
                var reflectDotEye = reflectVector * eyeVector;

                if (reflectDotEye <= 0)
                {
                    specular = Color.Black;
                }
                else
                {
                    // Compute the specular contribution
                    var factor = Math.Pow(reflectDotEye, Shininess);
                    specular = light.Intensity * Specular * factor;
                }
            }

            // Add the three contributions together to get the final shading
            return ambient + diffuse + specular;
        }

        public override bool Equals(object obj)
        {
            if (obj is Material material)
            {
                return Color == material.Color &&
                       Ambient == material.Ambient &&
                       Diffuse == material.Diffuse &&
                       Specular == material.Specular &&
                       Shininess == material.Shininess;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Ambient, Diffuse, Specular, Shininess);
        }

        public static bool operator ==(Material m1, Material m2)
        {
            return m1.Equals(m2);
        }
        public static bool operator !=(Material m1, Material m2)
        {
            return !m1.Equals(m2);
        }
    }
}

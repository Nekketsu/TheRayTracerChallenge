using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Tests.Entities;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class PatternsTests
    {
        [Fact]
        public void CreatingAStripePattern()
        {
            var pattern = new StripePattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.ColorA);
            Assert.Equal(Color.Black, pattern.ColorB);
        }

        [Fact]
        public void AStripePatternIsConstantInY()
        {
            var pattern = new StripePattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 1, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 2, 0)));
        }

        [Fact]
        public void AStripePatternIsConstantInZ()
        {
            var pattern = new StripePattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 1)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 2)));
        }

        [Fact]
        public void AStripePatternAlternatesInX()
        {
            var pattern = new StripePattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0.9, 0, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(1, 0, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(-0.1, 0, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(-1, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(-1.1, 0, 0)));
        }

        [Fact]
        public void LightingWithAPatternApplied()
        {
            var m = new Material();
            m.Pattern = new StripePattern(Color.White, Color.Black);
            m.Ambient = 1;
            m.Diffuse = 0;
            m.Specular = 0;
            var eyeVector = new Vector(0, 0, -1);
            var normalVector = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));

            var c1 = m.Lighting(new TestShape(), light, new Point(0.9, 0, 0), eyeVector, normalVector, false);
            var c2 = m.Lighting(new TestShape(), light, new Point(1.1, 0, 0), eyeVector, normalVector, false);

            Assert.Equal(Color.White, c1);
            Assert.Equal(Color.Black, c2);
        }

        [Fact]
        public void StripesWithAnObjectTransformation()
        {
            var shape = new Sphere();
            shape.Transform = Matrix.Scaling(2, 2, 2);
            var pattern = new StripePattern(Color.White, Color.Black);

            var c = pattern.PatternAtShape(shape, new Point(1.5, 0, 0));

            Assert.Equal(Color.White, c);
        }

        [Fact]
        public void StripesWithAPatternTransformatino()
        {
            Sphere shape = new Sphere();
            var pattern = new StripePattern(Color.White, Color.Black);
            pattern.Transform = Matrix.Scaling(2, 2, 2);

            var c = pattern.PatternAtShape(shape, new Point(1.5, 0, 0));

            Assert.Equal(Color.White, c);
        }

        [Fact]
        public void StripesWithBothAnObjectAndAPatternTransformation()
        {
            var shape = new Sphere();
            shape.Transform = Matrix.Scaling(2, 2, 2);
            var pattern = new StripePattern(Color.White, Color.Black);
            pattern.Transform = Matrix.Translation(0.5, 0, 0);

            var c = pattern.PatternAtShape(shape, new Point(2.5, 0, 0));

            Assert.Equal(Color.White, c);
        }

        [Fact]
        public void TheDefaultPatternTranformation()
        {
            var pattern = new TestPattern();

            Assert.Equal(Matrix.Identity, pattern.Transform);
        }

        [Fact]
        public void AssigningATranformation()
        {
            var pattern = new TestPattern();

            pattern.Transform = Matrix.Translation(1, 2, 3);

            Assert.Equal(Matrix.Translation(1, 2, 3), pattern.Transform);
        }

        [Fact]
        public void APatternWithAnObjectTranformation()
        {
            var shape = new Sphere();
            shape.Transform = Matrix.Scaling(2, 2, 2);
            var pattern = new TestPattern();

            var c = pattern.PatternAtShape(shape, new Point(2, 3, 4));

            Assert.Equal(new Color(1, 1.5, 2), c);
        }

        [Fact]
        public void APatternWithAPatternTranformation()
        {
            var shape = new Sphere();
            var pattern = new TestPattern();
            pattern.Transform = Matrix.Scaling(2, 2, 2);

            var c = pattern.PatternAtShape(shape, new Point(2, 3, 4));

            Assert.Equal(new Color(1, 1.5, 2), c);
        }

        [Fact]
        public void APatternWithBothAnObjectAndAPatternTranformation()
        {
            var shape = new Sphere();
            shape.Transform = Matrix.Scaling(2, 2, 2);
            var pattern = new TestPattern();
            pattern.Transform = Matrix.Translation(0.5, 1, 1.5);

            var c = pattern.PatternAtShape(shape, new Point(2.5, 3, 3.5));

            Assert.Equal(new Color(0.75, 0.5, 0.25), c);
        }

        [Fact]
        public void AGradientLinearlyInterpolatesBetweenColors()
        {
            var pattern = new GradientPattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(new Color(0.75, 0.75, 0.75), pattern.PatternAt(new Point(0.25, 0, 0)));
            Assert.Equal(new Color(0.5, 0.5, 0.5), pattern.PatternAt(new Point(0.5, 0, 0)));
            Assert.Equal(new Color(0.25, 0.25, 0.25), pattern.PatternAt(new Point(0.75, 0, 0)));
        }

        [Fact]
        public void ARingShouldExtendInBothXAndZ()
        {
            var pattern = new RingPattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(1, 0, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(0, 0, 1)));
            // 0.78 = just slighly more than Math.Sqrt(2) / 2
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(0.708, 0, 0.708)));
        }

        [Fact]
        public void CheckersShouldRepeatInX()
        {
            var pattern = new CheckersPattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0.99, 0, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(1.01, 0, 0)));
        }

        [Fact]
        public void CheckersShouldRepeatInY()
        {
            var pattern = new CheckersPattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0.99, 0)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(0, 1.01, 0)));
        }

        [Fact]
        public void CheckersShouldRepeatInZ()
        {
            var pattern = new CheckersPattern(Color.White, Color.Black);

            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(Color.White, pattern.PatternAt(new Point(0, 0, 0.99)));
            Assert.Equal(Color.Black, pattern.PatternAt(new Point(0, 0, 1.01)));
        }
    }
}

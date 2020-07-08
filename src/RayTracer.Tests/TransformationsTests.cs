using RayTracer.Matrices;
using RayTracer.Tuples;
using System;
using Xunit;

namespace RayTracer.Tests
{
    public class TransformationsTests
    {
        [Fact]
        public void MultiplyingByATranlationMatrix()
        {
            var transform = Matrix.Translation(5, -3, 2);
            var p = new Point(-3, 4, 5);

            Assert.Equal(new Point(2, 1, 7), transform * p);
        }

        [Fact]
        public void MultiplyingByTheInverseOfATranslationMatrix()
        {
            var transform = Matrix.Translation(5, -3, 2);
            var inverse = transform.Inverse();
            var p = new Point(-3, 4, 5);

            Assert.Equal(new Point(-8, 7, 3), inverse * p);
        }

        [Fact]
        public void TranslationDoesNotAffectVectors()
        {
            var transform = Matrix.Translation(5, -3, 2);
            var v = new Vector(-3, 4, 5);

            Assert.Equal(v, transform * v);
        }

        [Fact]
        public void AScaleingMatrixAppliedToAPoint()
        {
            var transform = Matrix.Scaling(2, 3, 4);
            var p = new Point(-4, 6, 8);

            Assert.Equal(new Point(-8, 18, 32), transform * p);
        }

        [Fact]
        public void AScalingMatrixAppliedToAVector()
        {
            var transform = Matrix.Scaling(2, 3, 4);
            var v = new Vector(-4, 6, 8);

            Assert.Equal(new Vector(-8, 18, 32), transform * v);
        }

        [Fact]
        public void MultiplyingByTheInverseOfAScalingMatrix()
        {
            var transform = Matrix.Scaling(2, 3, 4);
            var inverse = transform.Inverse();
            var v = new Vector(-4, 6, 8);

            Assert.Equal(new Vector(-2, 2, 2), inverse * v);
        }

        [Fact]
        public void ReflectionIsScalingByANegativeValue()
        {
            var transform = Matrix.Scaling(-1, 1, 1);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(-2, 3, 4), transform * p);
        }

        [Fact]
        public void RotatingAPointAroundTheXAxis()
        {
            var p = new Point(0, 1, 0);
            var halfQuarter = Matrix.RotationX(Math.PI / 4);
            var fullQuarter = Matrix.RotationX(Math.PI / 2);

            Assert.Equal(new Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), halfQuarter * p);
            Assert.Equal(new Point(0, 0, 1), fullQuarter * p);
        }

        [Fact]
        public void TheInverseOfAnXRotationRotatesInTheOppositeDirection()
        {
            var p = new Point(0, 1, 0);
            var halfQuarter = Matrix.RotationX(Math.PI / 4);
            var inverse = halfQuarter.Inverse();

            Assert.Equal(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2), inverse * p);
        }

        [Fact]
        public void RotatingAPointAroundTheYAxis()
        {
            var p = new Point(0, 0, 1);
            var halfQuarter = Matrix.RotationY(Math.PI / 4);
            var fullQuarter = Matrix.RotationY(Math.PI / 2);

            Assert.Equal(new Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2), halfQuarter * p);
            Assert.Equal(new Point(1, 0, 0), fullQuarter * p);
        }

        [Fact]
        public void RotatingAPointAroundTheZAxis()
        {
            var p = new Point(0, 1, 0);
            var halfQuarter = Matrix.RotationZ(Math.PI / 4);
            var fullQuarter = Matrix.RotationZ(Math.PI / 2);

            Assert.Equal(new Point(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0), halfQuarter * p);
            Assert.Equal(new Point(-1, 0, 0), fullQuarter * p);
        }

        [Fact]
        public void AShearingTransformationMovesXInProportionToY()
        {
            var transform = Matrix.Shearing(1, 0, 0, 0, 0, 0);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(5, 3, 4), transform * p);
        }

        [Fact]
        public void AShearingTransformationMovesXInProportionToZ()
        {
            var transform = Matrix.Shearing(0, 1, 0, 0, 0, 0);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(6, 3, 4), transform * p);
        }

        [Fact]
        public void AShearingTransformationMovesYInProportionToX()
        {
            var transform = Matrix.Shearing(0, 0, 1, 0, 0, 0);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(2, 5, 4), transform * p);
        }

        [Fact]
        public void AShearingTransformationMovesYInProportionToZ()
        {
            var transform = Matrix.Shearing(0, 0, 0, 1, 0, 0);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(2, 7, 4), transform * p);
        }

        [Fact]
        public void AShearingTransformationMovesZInProportionToX()
        {
            var transform = Matrix.Shearing(0, 0, 0, 0, 1, 0);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(2, 3, 6), transform * p);
        }

        [Fact]
        public void AShearingTransformationMovesZInProportionToY()
        {
            var transform = Matrix.Shearing(0, 0, 0, 0, 0, 1);
            var p = new Point(2, 3, 4);

            Assert.Equal(new Point(2, 3, 7), transform * p);
        }

        [Fact]
        public void IndividualTransformationsAreAppliedInSequence()
        {
            var p = new Point(1, 0, 1);
            var a = Matrix.RotationX(Math.PI / 2);
            var b = Matrix.Scaling(5, 5, 5);
            var c = Matrix.Translation(10, 5, 7);

            var p2 = a * p;
            var p3 = b * p2;
            var p4 = c * p3;

            Assert.Equal(new Point(1, -1, 0), p2);
            Assert.Equal(new Point(5, -5, 0), p3);
            Assert.Equal(new Point(15, 0, 7), p4);
        }

        [Fact]
        public void ChainedTransformationsMustBeAppliedInReverseOrder()
        {
            var p = new Point(1, 0, 1);
            var a = Matrix.RotationX(Math.PI / 2);
            var b = Matrix.Scaling(5, 5, 5);
            var c = Matrix.Translation(10, 5, 7);

            var t = c * b * a;

            Assert.Equal(new Point(15, 0, 7), t * p);
        }

        [Fact]
        public void TheTransformationMatrixForTheDefaultOrientation()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, -1);
            var up = new Vector(0, 1, 0);

            var t = Matrix.View(from, to, up);

            Assert.Equal(Matrix.Identity, t);
        }

        [Fact]
        public void AViewTransformationMatrixLookingInPositiveZDirection()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, 1);
            var up = new Vector(0, 1, 0);

            var t = Matrix.View(from, to, up);

            Assert.Equal(Matrix.Scaling(-1, 1, -1), t);
        }

        [Fact]
        public void TheViewTransformationMovesTheWorld()
        {
            var from = new Point(0, 0, 8);
            var to = new Point(0, 0, 0);
            var up = new Vector(0, 1, 0);

            var t = Matrix.View(from, to, up);

            Assert.Equal(Matrix.Translation(0, 0, -8), t);
        }

        [Fact]
        public void AnArbitraryViewTransformation()
        {
            var from = new Point(1, 3, 2);
            var to = new Point(4, -2, 8);
            var up = new Vector(1, 1, 0);

            var t = Matrix.View(from, to, up);

            var expected = new Matrix(new double[,]
            {
                { -0.50709, 0.50709, 0.67612, -2.36643 },
                { 0.76772, 0.60609, 0.12122, -2.82843 },
                { -0.35857, 0.59761, -0.71714, 0.00000 },
                { 0.00000, 0.00000, 0.00000, 1.00000 }
            });

            Assert.Equal(expected, t);
        }
    }
}

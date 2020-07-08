using RayTracer.Matrices;
using RayTracer.Tuples;
using Xunit;

namespace RayTracer.Tests
{
    public class MatrixTests
    {
        [Fact]
        public void ConstructingAndInspectingA4x4Matrix()
        {
            var matrix = new Matrix(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5.5, 6.5, 7.5, 8.5 },
                { 9, 10, 11, 12 },
                { 13.5, 14.5, 15.5, 16.5 }
            });

            Assert.Equal(1, matrix[0, 0]);
            Assert.Equal(4, matrix[0, 3]);
            Assert.Equal(5.5, matrix[1, 0]);
            Assert.Equal(7.5, matrix[1, 2]);
            Assert.Equal(11, matrix[2, 2]);
            Assert.Equal(13.5, matrix[3, 0]);
            Assert.Equal(15.5, matrix[3, 2]);
        }

        [Fact]
        public void A2x2MatrixOughtToBeRepresentable()
        {
            var matrix = new Matrix(new double[,]
            {
                { -3, 5 },
                { 1, -2 }
            });

            Assert.Equal(-3, matrix[0, 0]);
            Assert.Equal(5, matrix[0, 1]);
            Assert.Equal(1, matrix[1, 0]);
            Assert.Equal(-2, matrix[1, 1]);
        }

        [Fact]
        public void A3x3MatrixOughtToBeRepresentable()
        {
            var matrix = new Matrix(new double[,]
            {
                { -3, 5, 0 },
                { 1, -2, -7 },
                { 0, 1, 1 }
            });

            Assert.Equal(-3, matrix[0, 0]);
            Assert.Equal(-2, matrix[1, 1]);
            Assert.Equal(1, matrix[2, 2]);
        }

        [Fact]
        public void MatrixEqualityWithIdenticalMatrices()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 8, 7, 6 },
                { 5, 4, 3, 2 }
            });

            var b = new Matrix(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 8, 7, 6 },
                { 5, 4, 3, 2 }
            });

            Assert.Equal(a, b);
        }

        [Fact]
        public void MatrixEqualityWithDifferentMatrices()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 8, 7, 6 },
                { 5, 4, 3, 2 }
            });

            var b = new Matrix(new double[,]
            {
                { 2, 3, 4, 5 },
                { 6, 7, 8, 9 },
                { 8, 7, 6, 5 },
                { 4, 3, 2, 1 }
            });

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void MultiplyingTwoMatrices()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 8, 7, 6 },
                { 5, 4, 3, 2 }
            });

            var b = new Matrix(new double[,]
            {
                { -2, 1, 2, 3 },
                { 3, 2, 1, -1 },
                { 4, 3, 6, 5 },
                { 1, 2, 7 ,8 }
            });

            var expected = new Matrix(new double[,]
            {
                { 20, 22, 50, 48 },
                { 44, 54, 114, 108 },
                { 40, 58, 110, 102 },
                { 16, 26, 46, 42 }
            });

            Assert.Equal(expected, a * b);
        }

        [Fact]
        public void AMatrixMultipliedByATuple()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 2, 3, 4 },
                { 2, 4, 4, 2 },
                { 8, 6, 4, 1 },
                { 0, 0, 0, 1 }
            });

            var b = new Point(1, 2, 3);

            Assert.Equal(new Point(18, 24, 33), a * b);
        }

        [Fact]
        public void MultiplyingAMatrixByTheIdentityMatrix()
        {
            var a = new Matrix(new double[,]
            {
                { 0, 1, 2, 4 },
                { 1, 2, 4, 8 },
                { 2, 4, 8, 16 },
                { 4, 8, 16, 32 }
            });

            Assert.Equal(a, a * Matrix.Identity);
        }

        [Fact]
        public void MultiplyingTheIdentityMatrixByATuple()
        {
            var a = new Vector(1, 2, 3);

            Assert.Equal(a, Matrix.Identity * a);
        }

        [Fact]
        public void TransposingAMatrix()
        {
            var a = new Matrix(new double[,]
            {
                { 0, 9, 3, 0 },
                { 9, 8, 0, 8 },
                { 1, 8, 5, 3 },
                { 0, 0, 5, 8 }
            });

            var expected = new Matrix(new double[,]
            {
                { 0, 9, 1, 0 },
                { 9, 8, 8, 0 },
                { 3, 0, 5, 5 },
                { 0, 8, 3, 8 }
            });

            Assert.Equal(expected, a.Transpose());
        }

        [Fact]
        public void TransposingTheIdentityMatrix()
        {
            var a = Matrix.Identity.Transpose();

            Assert.Equal(Matrix.Identity, a);
        }

        [Fact]
        public void CalculatingTheDeterminantOfA2x2Mawtrix()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 5 },
                { -3, 2 }
            });

            Assert.Equal(17, a.Determinant());
        }

        [Fact]
        public void ASubmatrixOfA3x3MatrixIsA2x2Matrix()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 5, 0 },
                { -3, 2, 7 },
                { 0, 6, -3 }
            });

            var expected = new Matrix(new double[,]
            {
                { -3, 2 },
                { 0, 6 }
            });

            Assert.Equal(expected, a.SubMatrix(0, 2));
        }

        [Fact]
        public void ASubmatrixOfA4x4MatrixIsA3x3Matrix()
        {
            var a = new Matrix(new double[,]
            {
                { -6, 1, 1, 6 },
                { -8, 5, 8, 6 },
                { -1, 0, 8, 2 },
                { -7, 1, -1, 1 }
            });

            var expected = new Matrix(new double[,]
            {
                { -6, 1, 6 },
                { -8, 8, 6 },
                { -7, -1, 1 }
            });

            Assert.Equal(expected, a.SubMatrix(2, 1));
        }

        [Fact]
        public void CalculatingAMinorOfA3x3Matrix()
        {
            var a = new Matrix(new double[,]
            {
                { 3, 5, 0 },
                { 2, -1, -7 },
                { 6, -1, 5 }
            });

            var b = a.SubMatrix(1, 0);

            Assert.Equal(25, b.Determinant());
            Assert.Equal(25, a.Minor(1, 0));
        }

        [Fact]
        public void CalculatingACofactorOfA3x3Matrix()
        {
            var a = new Matrix(new double[,]
            {
                { 3, 5, 0 },
                { 2, -1, -7 },
                { 6, -1, 5 }
            });

            Assert.Equal(-12, a.Minor(0, 0));
            Assert.Equal(-12, a.Cofactor(0, 0));
            Assert.Equal(25, a.Minor(1, 0));
            Assert.Equal(-25, a.Cofactor(1, 0));
        }

        [Fact]
        public void CalculatingTheDeterminantOfA3x3Matrix()
        {
            var a = new Matrix(new double[,]
            {
                { 1, 2, 6 },
                { -5, 8, -4 },
                { 2,  6, 4 }
            });

            Assert.Equal(56, a.Cofactor(0, 0));
            Assert.Equal(12, a.Cofactor(0, 1));
            Assert.Equal(-46, a.Cofactor(0, 2));
            Assert.Equal(-196, a.Determinant());
        }

        [Fact]
        public void CalculatingTheDeterminantOfA4x4Matrix()
        {
            var a = new Matrix(new double[,]
            {
                { -2, -8, 3, 5 },
                { -3, 1, 7, 3 },
                { 1, 2, -9, 6 },
                { -6, 7, 7, -9 }
            });

            Assert.Equal(690, a.Cofactor(0, 0));
            Assert.Equal(447, a.Cofactor(0, 1));
            Assert.Equal(210, a.Cofactor(0, 2));
            Assert.Equal(51, a.Cofactor(0, 3));
            Assert.Equal(-4071, a.Determinant());
        }

        [Fact]
        public void TestingAnInvertibleMatrixForInvertibility()
        {
            var a = new Matrix(new double[,]
            {
                { 6, 4, 4, 4 },
                { 5, 5, 7, 6 },
                { 4, -9, 3, -7 },
                { 9, 1, 7, -6 }
            });

            Assert.Equal(-2120, a.Determinant());
            Assert.True(a.IsInvertible());
        }

        [Fact]
        public void TestingANonInvertibleMatrixForInvertibility()
        {
            var a = new Matrix(new double[,]
            {
                { -4, 2, -2, -3 },
                { 9, 6, 2, 6 },
                { 0, -5, 1, -5 },
                { 0, 0, 0, 0 }
            });

            Assert.Equal(0, a.Determinant());
            Assert.False(a.IsInvertible());
        }

        [Fact]
        public void CalculatingTheInverseOfAMatrix()
        {
            var a = new Matrix(new double[,]
            {
                { -5, 2, 6, -8 },
                { 1, -5, 1, 8 },
                { 7, 7, -6, -7 },
                { 1, -3, 7, 4 }
            });

            var b = a.Inverse();

            var expected = new Matrix(new double[,]
            {
                { 0.21805, 0.45113, 0.24060, -0.04511 },
                { -0.80827, -1.45677, -0.44361, 0.52068 },
                { -0.07895, -0.22368, -0.05263, 0.19737 },
                { -0.52256, -0.81391, -0.30075, 0.30639 }
            });

            Assert.Equal(532, a.Determinant());
            Assert.Equal(-160, a.Cofactor(2, 3));
            Assert.Equal(-160.0 / 532.0, b[3, 2]);
            Assert.Equal(105.0, a.Cofactor(3, 2));
            Assert.Equal(105.0 / 532.0, b[2, 3]);
            Assert.Equal(expected, b);
        }

        [Fact]
        public void CalculatingTheInverseOfAnotherMatrix()
        {
            var a = new Matrix(new double[,]
            {
                { 8, -5, 9, 2 },
                { 7, 5, 6, 1 },
                { -6, 0, 9, 6 },
                { -3, 0, -9, -4 }
            });

            var expected = new Matrix(new double[,]
            {
                { -0.15385, -0.15385, -0.28205, -0.53846 },
                { -0.07692, 0.12308, 0.02564, 0.03077 },
                { 0.35897, 0.35897, 0.43590, 0.92308 },
                { -0.69231, -0.69231, -0.76923, -1.92308 }
            });

            Assert.Equal(expected, a.Inverse());
        }

        [Fact]
        public void CalculatingTheInverseOfAThirdMatrix()
        {
            var a = new Matrix(new double[,]
            {
                { 9, 3, 0, 9 },
                { -5,-2, -6, -3 },
                { -4, 9, 6, 4 },
                { -7, 6, 6, 2 }
            });

            var expected = new Matrix(new double[,]
            {
                { -0.04074, -0.07778, 0.14444, -0.22222 },
                { -0.07778, 0.03333, 0.36667, -0.33333 },
                { -0.02901, -0.14630, -0.10926, 0.12963 },
                { 0.17778, 0.06667, -0.26667, 0.33333 }
            });

            Assert.Equal(expected, a.Inverse());
        }

        [Fact]
        public void MultiplyingAProductByItsInverse()
        {
            var a = new Matrix(new double[,]
            {
                { 3, -9, 7, 3 },
                { 3, -8, 2, -9 },
                { -4, 4, 4, 1 },
                { -6, 5, -1, 1 }
            });

            var b = new Matrix(new double[,]
            {
                { 8, 2, 2, 2 },
                { 3, -1, 7, 0 },
                { 7, 0, 5, 4 },
                { 6, -2, 0, 5 }
            });

            var c = a * b;

            Assert.Equal(a, c * b.Inverse());
        }
    }
}

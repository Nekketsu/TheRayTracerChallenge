using RayTracer.Extensions;
using RayTracer.Tuples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Matrices
{
    public class Matrix
    {
        public static Matrix Identity = new Matrix(new double[,]
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        });

        public static Matrix Translation(double x, double y, double z)
        {
            return new Matrix(new double[,]
            {
                { 1, 0, 0, x },
                { 0, 1, 0, y },
                { 0, 0, 1, z },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix Scaling(double x, double y, double z)
        {
            return new Matrix(new double[,]
            {
                { x, 0, 0, 0 },
                { 0, y, 0, 0 },
                { 0, 0, z, 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix RotationX(double radians)
        {
            return new Matrix(new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, Math.Cos(radians), -Math.Sin(radians), 0 },
                { 0, Math.Sin(radians), Math.Cos(radians), 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix RotationY(double radians)
        {
            return new Matrix(new double[,]
            {
                { Math.Cos(radians), 0, Math.Sin(radians), 0 },
                { 0, 1, 0 , 0 },
                { -Math.Sin(radians), 0, Math.Cos(radians), 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix RotationZ(double radians)
        {
            return new Matrix(new double[,]
            {
                { Math.Cos(radians), -Math.Sin(radians), 0, 0 },
                { Math.Sin(radians), Math.Cos(radians), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix Shearing(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            return new Matrix(new double[,]
            {
                { 1, xy, xz, 0 },
                { yx, 1, yz, 0 },
                { zx, zy, 1, 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix View(Point from, Point to, Vector up)
        {
            var forward = (to - from).Normalize();
            var upNormalized = up.Normalize();
            var left = forward.Cross(upNormalized);
            var trueUp = left.Cross(forward);

            var orientation = new Matrix(new double[,]
            {
                { left.X, left.Y, left.Z, 0 },
                { trueUp.X, trueUp.Y, trueUp.Z, 0 },
                { -forward.X, -forward.Y, -forward.Z, 0 },
                { 0, 0, 0, 1 }
            });

            return orientation * Translation(-from.X, -from.Y, -from.Z);
        }

        public Matrix Translate(double x, double y, double z)
        {
            return Translation(x, y, z).Multiply(this);
        }

        public Matrix Scale(double x, double y, double z)
        {
            return Scaling(x, y, z).Multiply(this);
        }

        public Matrix RotateX(double radians)
        {
            return RotationX(radians).Multiply(this);
        }

        public Matrix RotateY(double radians)
        {
            return RotationY(radians).Multiply(this);
        }

        public Matrix RotateZ(double radians)
        {
            return RotationZ(radians).Multiply(this);
        }

        public Matrix Shear(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            return Shearing(xy, xz, yx, yz, zx, zy).Multiply(this);
        }

        public int Rows { get; }
        public int Columns { get; }

        double[,] values;

        public Matrix(double[,] values)
        {
            this.values = values;

            Rows = values.GetLength(0);
            Columns = values.GetLength(1);
        }

        public double this[int row, int column] => values[row, column];

        public Matrix Multiply(Matrix matrix)
        {
            if (Columns != matrix.Rows)
            {
                throw new InvalidOperationException();
            }

            var size = Columns;
            var result = new double[Rows, matrix.Columns];
            for (var i = 0; i < Rows; i++)
            {
                var row = Row(i);
                for (var j = 0; j < matrix.Columns; j++)
                {
                    var column = matrix.Column(j);

                    result[i, j] = row.Zip(column, (r, c) => r * c).Sum();
                }
            }

            return new Matrix(result);
        }

        public Matrix Transpose()
        {
            var values = new double[Columns, Rows];

            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    values[j, i] = this[i, j];
                }
            }

            return new Matrix(values);
        }

        public double Determinant()
        {
            if (Rows != Columns)
            {
                throw new InvalidOperationException();
            }
            var size = Rows;

            if (size < 1)
            {
                throw new InvalidOperationException();
            }

            double determinant = 0;

            if (size == 1)
            {
                return this[0, 0];
            }
            //else if (size == 2)
            //{
            //    determinant = this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            //}
            else
            {
                var i = 0; // Fix any row or column
                for (var j = 0; j < size; j++)
                {
                    determinant += this[i, j] * Cofactor(i, j);
                }
            }

            return determinant;
        }

        public Matrix Inverse()
        {
            if (Rows != Columns)
            {
                throw new InvalidOperationException();
            }

            var size = Rows;
            if (size < 1)
            {
                throw new InvalidOperationException();
            }

            var determinant = Determinant();
            if (determinant == 0)
            {
                throw new InvalidOperationException();
            }

            var inverseValues = new double[Columns, Rows];

            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    // Cofactors -> Divide by deterimant -> Transpose
                    inverseValues[j, i] = Cofactor(i, j) / determinant;
                }
            }

            var inverse = new Matrix(inverseValues);

            return inverse;
        }



        public bool IsInvertible()
        {
            if (Rows != Columns)
            {
                return false;
            }
            var size = Rows;

            if (size < 1)
            {
                return false;
            }

            return Determinant() != 0;
        }

        public Matrix SubMatrix(int row, int column)
        {
            var values = new double[Rows - 1, Columns - 1];

            var destinationRow = 0;
            for (var sourceRow = 0; sourceRow < Rows; sourceRow++)
            {
                if (sourceRow == row) { continue; }

                var destinationColumn = 0;
                for (var sourceColumn = 0; sourceColumn < Columns; sourceColumn++)
                {
                    if (sourceColumn == column) { continue; }

                    values[destinationRow, destinationColumn] = this[sourceRow, sourceColumn];

                    destinationColumn++;
                }

                destinationRow++;
            }

            return new Matrix(values);
        }

        public double Minor(int row, int column)
        {
            return SubMatrix(row, column).Determinant();
        }

        public double Cofactor(int row, int column)
        {
            var minor = Minor(row, column);
            var cofactor = (row + column) % 2 == 0 ? minor : -minor;

            return cofactor;
        }

        internal IEnumerable<double> Row(int row)
        {
            for (var column = 0; column < Columns; column++)
            {
                yield return this[row, column];
            }
        }

        internal IEnumerable<double> Column(int column)
        {
            for (var row = 0; row < Rows; row++)
            {
                yield return this[row, column];
            }
        }


        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return m1.Multiply(m2);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Matrix matrix))
            {
                return false;
            }
            if (Rows != matrix.Rows || Columns != matrix.Columns)
            {
                return false;
            }

            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    if (!this[row, column].EqualsEpsilon(matrix[row, column]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(values);
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return m1.Equals(m2);
        }
        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !m1.Equals(m2);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < Rows; i++)
            {
                stringBuilder.AppendLine($"| {string.Join(" | ", Row(i))} |");
            }

            return stringBuilder.ToString();
        }
    }
}

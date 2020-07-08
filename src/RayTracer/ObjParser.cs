using RayTracer.Shapes;
using RayTracer.Tuples;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace RayTracer
{
    public class ObjParser
    {
        private static readonly NumberFormatInfo numberFormat = CultureInfo.GetCultureInfo("en-US").NumberFormat;

        public int IgnoredLineCount { get; }
        public ReadOnlyCollection<Point> Vertices { get; }
        public Group DefaultGroup { get; }
        public ReadOnlyDictionary<string, Group> Groups { get; }
        public ReadOnlyCollection<Vector> Normals { get; set; }

        public Point Minimum { get; private set; }
        public Point Maximum { get; private set; }
        public Point Middle { get; set; }


        public ObjParser(string objData) : this(objData.Split(Environment.NewLine)) { }

        public ObjParser(IEnumerable<string> objLines)
        {
            var vertices = new List<Point>();
            var defaultGroup = new Group();
            var currentGroup = defaultGroup;
            var groups = new Dictionary<string, Group>();
            var normals = new List<Vector>();

            var ignoredLineCount = 0;

            foreach (var line in objLines)
            {
                var lineSplit = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (lineSplit.Any())
                {
                    switch (lineSplit[0])
                    {
                        case "v":
                            ParseVertex(vertices, lineSplit, ref ignoredLineCount);
                            break;
                        case "f":
                            ParseFace(vertices, normals, currentGroup, lineSplit, ref ignoredLineCount);
                            break;
                        case "g":
                            ParseGroup(groups, ref currentGroup, lineSplit, ref ignoredLineCount);
                            break;
                        case "vn":
                            ParseNormal(normals, lineSplit, ref ignoredLineCount);
                            break;
                        default:
                            ignoredLineCount++;
                            break;
                    }
                }
            }

            Vertices = vertices.AsReadOnly();
            DefaultGroup = new Group(defaultGroup);
            Groups = new ReadOnlyDictionary<string, Group>(groups.ToDictionary(k => k.Key, v => new Group(v.Value)));
            Normals = normals.AsReadOnly();

            IgnoredLineCount = ignoredLineCount;

            (Minimum, Maximum, Middle) = GetBounds(vertices);
        }

        private void ParseVertex(List<Point> vertices, string[] lineSplit, ref int ignoredLineCount)
        {
            if (lineSplit.Length == 4)
            {
                double x = 0;
                double y = 0;
                double z = 0;

                var valid = double.TryParse(lineSplit[1], NumberStyles.Float, numberFormat, out x);
                valid = valid && double.TryParse(lineSplit[2], NumberStyles.Float, numberFormat, out y);
                valid = valid && double.TryParse(lineSplit[3], NumberStyles.Float, numberFormat, out z);

                if (valid)
                {
                    var vertex = new Point(x, y, z);
                    vertices.Add(vertex);

                    return;
                }
            }

            ignoredLineCount++;
        }

        private void ParseFace(List<Point> vertices, List<Vector> normals, Group triangles, string[] lineSplit, ref int ignoredLineCount)
        {
            bool isValid = false;

            if (lineSplit.Length >= 4)
            {
                isValid = true;

                var (vertex1, normal1) = GetVertextInfo(lineSplit[1], vertices, normals);
                var hasNormal1 = normal1 is { };
                if (vertex1 is { })
                {
                    for (var i = 2; i < lineSplit.Length - 1; i++)
                    {
                        var (vertex2, normal2) = GetVertextInfo(lineSplit[i], vertices, normals);
                        var (vertex3, normal3) = GetVertextInfo(lineSplit[i + 1], vertices, normals);

                        if (vertex2 == null & vertex3 == null)
                        {
                            isValid = false;
                            break;
                        }
                        var triangle = hasNormal1 && normal2 is { } && normal3 is { }
                            ? new SmoothTriangle(vertex1, vertex2, vertex3, normal1, normal2, normal3)
                            : new Triangle(vertex1, vertex2, vertex3);
                        triangles.AddChild(triangle);
                    }
                }
            }

            if (!isValid)
            {
                ignoredLineCount++;
            }
        }

        private (Point vertex, Vector normal) GetVertextInfo(string text, List<Point> vertices, List<Vector> normals)
        {
            Point vertex = null;
            Vector normal = null;

            var vertexInfo = text.Split('/');
            if ((vertexInfo.Length >= 1) &&
                (int.TryParse(vertexInfo[0], NumberStyles.Integer, numberFormat, out var vertexIndex)) &&
                (vertexIndex >= 1 && vertexIndex <= vertices.Count))
            {
                vertex = vertices[vertexIndex - 1];
            }

            if ((vertexInfo.Length >= 2) &&
                (int.TryParse(vertexInfo[2], NumberStyles.Integer, numberFormat, out var normalIndex)) &&
                (normalIndex >= 1 && normalIndex <= normals.Count))
            {
                normal = normals[normalIndex - 1];
            }

            return (vertex, normal);
        }

        public Group ToGroup()
        {
            var group = new Group(DefaultGroup);
            group.AddChildren(Groups.Values);

            return group;
        }

        private void ParseGroup(Dictionary<string, Group> groups, ref Group currentGroup, string[] lineSplit, ref int ignoredLineCount)
        {
            if (lineSplit.Length == 2)
            {
                var groupName = lineSplit[1];
                currentGroup = new Group();
                groups.Add(groupName, currentGroup);
            }

            ignoredLineCount++;
        }

        private void ParseNormal(List<Vector> normals, string[] lineSplit, ref int ignoredLineCount)
        {
            if (lineSplit.Length == 4)
            {
                double x = 0;
                double y = 0;
                double z = 0;

                var numberFormat = CultureInfo.GetCultureInfo("en-US").NumberFormat;

                var valid = double.TryParse(lineSplit[1], NumberStyles.Float, numberFormat, out x);
                valid = valid && double.TryParse(lineSplit[2], NumberStyles.Float, numberFormat, out y);
                valid = valid && double.TryParse(lineSplit[3], NumberStyles.Float, numberFormat, out z);

                if (valid)
                {
                    var normal = new Vector(x, y, z);
                    normals.Add(normal);

                    return;
                }
            }

            ignoredLineCount++;
        }

        private (Point minimum, Point maximum, Point middle) GetBounds(List<Point> vertices)
        {
            if (!vertices.Any())
            {
                return (null, null, null);
            }

            var minimumX = vertices.First().X;
            var minimumY = vertices.First().Y;
            var minimumZ = vertices.First().Z;

            var maximumX = vertices.First().X;
            var maximumY = vertices.First().Y;
            var maximumZ = vertices.First().Z;

            foreach (var vertex in vertices.Skip(1))
            {
                if (vertex.X < minimumX)
                {
                    minimumX = vertex.X;
                }
                if (vertex.Y < minimumY)
                {
                    minimumY = vertex.Y;
                }
                if (vertex.Z < minimumZ)
                {
                    minimumZ = vertex.Z;
                }

                if (vertex.X > maximumX)
                {
                    maximumX = vertex.X;
                }
                if (vertex.Y > maximumY)
                {
                    maximumY = vertex.Y;
                }
                if (vertex.Z > maximumZ)
                {
                    maximumZ = vertex.Z;
                }
            }

            var minimum = new Point(minimumX, minimumY, minimumZ);
            var maximum = new Point(maximumX, maximumY, maximumZ);

            var middle = new Point((minimumX + maximumX) / 2, (minimumY + maximumY) / 2, (minimumZ + maximumZ) / 2);

            return (minimum, maximum, middle);
        }
    }
}

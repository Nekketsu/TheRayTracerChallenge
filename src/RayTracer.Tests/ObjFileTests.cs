using RayTracer.Shapes;
using RayTracer.Tuples;
using System.IO;
using Xunit;

namespace RayTracer.Tests
{
    public class ObjFileTests
    {
        [Fact]
        public void IgnoringUnrecognizedLines()
        {
            var gibberish =
@"There was a young lady named Bright
who traveled much faster than light.
She set out one day
in a relative way,
and came back the previous night.";

            var parser = new ObjParser(gibberish);

            Assert.Equal(5, parser.IgnoredLineCount);
        }

        [Fact]
        public void VertexRecords()
        {
            var file =
@"v -1 1 0
v -1.0000 0.5000 0.0000
v 1 0 0
v 1 1 0";

            var parser = new ObjParser(file);

            Assert.Equal(new Point(-1, 1, 0), parser.Vertices[0]);
            Assert.Equal(new Point(-1, 0.5, 0), parser.Vertices[1]);
            Assert.Equal(new Point(1, 0, 0), parser.Vertices[2]);
            Assert.Equal(new Point(1, 1, 0), parser.Vertices[3]);
        }

        [Fact]
        public void ParsingTriangleFaces()
        {
            var file =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

f 1 2 3
f 1 3 4
";

            var parser = new ObjParser(file);
            var g = parser.DefaultGroup;
            var t1 = (Triangle)g[0];
            var t2 = (Triangle)g[1];

            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
        }

        [Fact]
        public void TriangulatingPolygons()
        {
            var file =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
v 0 2 0

f 1 2 3 4 5";

            var parser = new ObjParser(file);
            var g = parser.DefaultGroup;
            var t1 = (Triangle)g[0];
            var t2 = (Triangle)g[1];
            var t3 = (Triangle)g[2];

            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
            Assert.Equal(parser.Vertices[0], t3.P1);
            Assert.Equal(parser.Vertices[3], t3.P2);
            Assert.Equal(parser.Vertices[4], t3.P3);
        }

        [Fact]
        public void TrianglesInGroups()
        {
            var file = File.ReadAllText("Assets/triangles.obj");

            var parser = new ObjParser(file);
            var g1 = parser.Groups["FirstGroup"];
            var g2 = parser.Groups["SecondGroup"];
            var t1 = (Triangle)g1[0];
            var t2 = (Triangle)g2[0];

            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
        }

        [Fact]
        public void ConvertingAndObjFileToAGroup()
        {
            var file = File.ReadAllText("Assets/triangles.obj");
            var parser = new ObjParser(file);

            var g = parser.ToGroup();

            Assert.Contains(parser.Groups["FirstGroup"], g);
            Assert.Contains(parser.Groups["SecondGroup"], g);
        }

        [Fact]
        public void VertextNormalRecords()
        {
            var file =
@"vn 0 0 1
vn 0.707 0 -0.707
vn 1 2 3";

            var parser = new ObjParser(file);

            Assert.Equal(new Vector(0, 0, 1), parser.Normals[0]);
            Assert.Equal(new Vector(0.707, 0, -0.707), parser.Normals[1]);
            Assert.Equal(new Vector(1, 2, 3), parser.Normals[2]);
        }

        [Fact]
        public void FacesWithNormals()
        {
            var file =
@"v 0 1 0
v -1 0 0
v 1 0 0

vn -1 0 0
vn 1 0 0
vn 0 1 0

f 1//3 2//1 3//2
f 1/0/3 2/102/1 3/14/2";

            var parser = new ObjParser(file);
            var g = parser.DefaultGroup;
            var t1 = (SmoothTriangle)g[0];
            var t2 = (SmoothTriangle)g[1];

            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Normals[2], t1.N1);
            Assert.Equal(parser.Normals[0], t1.N2);
            Assert.Equal(parser.Normals[1], t1.N3);
            Assert.Equal(t1, t2);
        }
    }
}

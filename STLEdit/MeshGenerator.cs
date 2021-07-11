using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STLEdit
{
    public static class MeshGenerator
    {
        public static List<Face> GetCube()
        {
            List<Face> faces = new List<Face>();
            Vertex[] bottom = new Vertex[] { new Vertex(0, 0, 0), new Vertex(0, 1, 0), new Vertex(0, 1, 1), new Vertex(0, 0, 1) };
            Vertex[] top = new Vertex[] { new Vertex(1, 0, 0), new Vertex(1, 0, 1), new Vertex(1, 1, 1), new Vertex(1, 1, 0) };
            Vertex[] right = new Vertex[4];
            Vertex[] left = new Vertex[4];
            Vertex[] front = new Vertex[4];
            Vertex[] back = new Vertex[4];

            for(int i = 0; i < 4; i++)
            {
                left[i] = new Vertex(bottom[i].Z, bottom[i].X, bottom[i].Y);
                right[i] = new Vertex(top[i].Z, top[i].X, top[i].Y);
                front[i] = new Vertex(bottom[i].Y, bottom[i].Z, bottom[i].X);
                back[i] = new Vertex(top[i].Y, top[i].Z, top[i].X);
            }

            Vertex[][] planes = new Vertex[][]
            {
                bottom, top, front, back, right, left
            };

            foreach(Vertex[] plane in planes)
            {
                Face face1 = new Face(plane[1], plane[0], plane[3]);
                Face face2 = new Face(plane[2], plane[1], plane[3]);
                faces.Add(face1);
                faces.Add(face2);
            }

            return faces;
        }

        public static List<Face> GetCube(Vertex p1, Vertex p2)
        {
            Vertex scale = p2 - p1;
            var cube = GetCube();
            for (int i = 0; i < cube.Count; i++)
            {
                Face f = cube[i];
                for (int j = 0; j < 3; j++)
                {
                    Vertex v = f[j];
                    v.X = (v.X * scale.X) + p1.X;
                    v.Y = (v.Y * scale.Y) + p1.Y;
                    v.Z = (v.Z * scale.Z) + p1.Z;
                    f[j] = v;
                }
                cube[i] = f;
            }
            return cube;
        }

        public static List<Face> GetCylinder(float rad, float z1, float z2)
        {
            int steps = 64;
            PointF[] circle = new PointF[steps];
            for(int i = 0; i < steps; i++)
            {
                float angle = i / (float)steps * (float)Math.PI * 2f;
                float x = (float)Math.Cos(angle) * rad;
                float y = (float)Math.Sin(angle) * rad;
                circle[i] = new PointF(x, y);
            }

            return GetExtrutedCurve(circle, z1, z2);
        }

        public static List<Face> GetExtrutedCurve(PointF[] points, float z1, float z2)
        {
            List<Face> faces = new List<Face>();
            if (points.Length == 0)
                return faces;
            Vertex start1 = new Vertex(points[0].X, points[0].Y, z1);
            Vertex start2 = new Vertex(points[0].X, points[0].Y, z2);
            for(int i = 0; i < points.Length; i++)
            {
                PointF vCurrent = points[i];
                PointF vNext = points[(i + 1) % points.Length];

                if (i >= 1 && i < points.Length - 1)
                {
                    // Bottom
                    faces.Add(new Face(start1, new Vertex(vNext.X, vNext.Y, z1), new Vertex(vCurrent.X, vCurrent.Y, z1)));
                    // Top
                    faces.Add(new Face(start2, new Vertex(vCurrent.X, vCurrent.Y, z2), new Vertex(vNext.X, vNext.Y, z2)));
                }
                // Wall
                faces.Add(
                    new Face(new Vertex(vCurrent.X, vCurrent.Y,z1), new Vertex(vNext.X,vNext.Y,z1), new Vertex(vCurrent.X, vCurrent.Y,z2))
                    );
                faces.Add(
                    new Face(new Vertex(vNext.X, vNext.Y, z1), new Vertex(vNext.X, vNext.Y, z2), new Vertex(vCurrent.X, vCurrent.Y, z2))
                    );
            }
            return faces;
        }
    }
}

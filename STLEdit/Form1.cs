using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STLEdit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.Faces.AddRange(MeshGenerator.GetCylinder(1, -1, 1));
            textBox1.Text = mesh.ToAsciiSTL();

            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                string stl = mesh.ToAsciiSTL();
                File.WriteAllText(sfd.FileName, stl);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "STL|*.stl";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Mesh mesh = Mesh.LoadFromStl(ofd.FileName);
                Mesh mesh2 = mesh.Clone();
                mesh2.TransformTranslate(1, 0, 0);
                Mesh mesh3 = mesh2.Clone();//Mesh.Merge(mesh, mesh2);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "STL|*.stl";
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    string txt = mesh3.ToAsciiSTL();
                    File.WriteAllText(sfd.FileName, txt);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Vertex p = new Vertex(2, 4, 6);
            Matrix4x4 mat = Matrix4x4.GetScaleMatrix(1, 6, 2);
            textBox1.Text = (p * mat).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.Faces.AddRange(MeshGenerator.GetCube());
            Matrix4x4 mat = Matrix4x4.GetRotationX((float)Math.PI / 4).AtPoint(.5f, .5f, .5f);
            mesh.Transform(mat);
            textBox1.Text = mesh.ToAsciiSTL();
        }

        private void btnCylinder_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.Faces.AddRange(MeshGenerator.GetCylinder(1, 0, 1));
            textBox1.Text = mesh.ToAsciiSTL();
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            PointF[] curve = new PointF[]
            {
                new PointF(0,0),
                new PointF(2,0),
                new PointF(2,2),
                new PointF(1,2),
                new PointF(0,1)
            };

            Mesh mesh = new Mesh();
            mesh.Faces.AddRange(MeshGenerator.GetExtrutedCurve(curve, 0, 1));
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "STL|*.stl";
            if(ofd.ShowDialog()== DialogResult.OK)
            {
                File.WriteAllText(ofd.FileName, mesh.ToAsciiSTL());
            }
        }
    }
}

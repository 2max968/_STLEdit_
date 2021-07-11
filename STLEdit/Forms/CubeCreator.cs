using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STLEdit.Forms
{
    public partial class CubeCreator : Form
    {
        public CubeCreator()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.Faces.AddRange(MeshGenerator.GetCube());
            mesh.Transform(Matrix4x4.GetScaleMatrix(1, 1, 2));
            textBox1.Text = mesh.ToAsciiSTL();
        }
    }
}

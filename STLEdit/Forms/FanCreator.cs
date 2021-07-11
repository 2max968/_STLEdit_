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

namespace STLEdit.Forms
{
    public partial class FanCreator : Form
    {
        public FanCreator()
        {
            InitializeComponent();

            tbRad.Text = 100.ToString();
            tbAngle.Text = 30.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int bladeNum = (int)tbNum.Value;
            if (!float.TryParse(tbRad.Text, out float rad)
                || !float.TryParse(tbAngle.Text, out float ang))
            {
                MessageBox.Show("Invalid Input", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ang *= (float)Math.PI / 180f;
            float innerRad = rad * .1f;
            float height = 20;

            Mesh mesh = new Mesh();
            mesh.Faces.AddRange(MeshGenerator.GetCylinder(innerRad, 0, height));
            Mesh blade = new Mesh();
            blade.Faces.AddRange(MeshGenerator.GetCube(new Vertex(-1, innerRad, 0), new Vertex(1, rad, height)));
            blade.Transform(Matrix4x4.GetRotationY(ang).AtPoint(0, 0, height / 2));
            for(int i = 0; i < bladeNum; i++)
            {
                float currentAngle = i / (float)bladeNum * (float)Math.PI * 2f;
                Mesh currentBlade = blade.Clone();
                currentBlade.Transform(Matrix4x4.GetRotationZ(currentAngle));
                mesh.Faces.AddRange(currentBlade.Faces);
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "STL|*.stl";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                string stl = mesh.ToAsciiSTL();
                File.WriteAllText(sfd.FileName, stl);
            }
        }
    }
}

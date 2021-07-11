using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STLEdit
{
    public struct Face
    {
        public Vertex A, B, C;
        public Vertex Normal;

        public Vertex this[int ind]
        {
            get
            {
                switch (ind)
                {
                    case 0: return A;
                    case 1: return B;
                    case 2: return C;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (ind)
                {
                    case 0:
                        A = value;
                        return;
                    case 1:
                        B = value;
                        return;
                    case 2:
                        C = value;
                        return;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Face(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
            Normal = Vertex.Cross(b - a, c - a).GetNormalized();
        }
    }
}

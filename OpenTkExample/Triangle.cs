using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkExample
{
	class Triangle
	{

		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }

		public Vector3 Scale { get; set; }

		public Matrix4 ModelMatrix = Matrix4.Identity;
		public Matrix4 ViewMatrix = Matrix4.Identity;
		public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
		public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

		private Vector3[] _vertex;
		private Vector3[] _colors;
        private Vector3[] _normals;


		public Triangle() { }
		public Triangle(Vector3[] vert,Vector3[] col)
		{
			SetVertex(vert);
			SetColors(col);
		}

        public void SetNormals(Vector3[] v)
        {
            int l = v.Length;
            _normals = new Vector3[l];
            for (int i = 0; i < l; i++)
                _normals[i] = v[i];
        }


		public void SetVertex(Vector3[] v)
		{
			int l = v.Length;
			_vertex = new Vector3[l];
			for(int i = 0; i < l; i++)
				_vertex[i] = v[i];
		}

		public Vector3[] GetVertex()
		{
			return this._vertex;
		}

        public Vector3[] GetNormals()
        {
            return this._normals;
        }

		public void SetColors(Vector3[] v)
		{
			int l = v.Length;
			_colors = new Vector3[l];
			for (int i = 0; i < l; i++)
				_colors[i] = v[i];
		}

		public Vector3[] GetColors()
		{
			return this._colors;
		}


		public void CalculateModelMatrix()
		{
			ModelMatrix = Matrix4.Scale(Scale) *
				Matrix4.CreateRotationX(Rotation.X) *
				Matrix4.CreateRotationY(Rotation.Y) *
				Matrix4.CreateRotationZ(Rotation.Z) *
				Matrix4.CreateTranslation(Position);
		}


		public void SetValueX(Matrix4 xVal)
		{
			Vector3 pos = Position;
			ModelMatrix *= Matrix4.CreateTranslation(new Vector3(-pos.X, -pos.Y, -pos.Z));
			ModelMatrix = ModelMatrix * xVal;
			ModelMatrix *= Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z));
		}
		public void SetValueY(Matrix4 yVal)
		{
			Vector3 pos = Position;
			ModelMatrix *= Matrix4.CreateTranslation(new Vector3(-pos.X, -pos.Y, -pos.Z));
			ModelMatrix = ModelMatrix * yVal;
			ModelMatrix *= Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z));

		}
		public void SetTranslation(float trans, char c)
		{

			switch (c)
			{
				case 'X':
				case 'x':
					Vector3 px = new Vector3(trans, 0, 0);
					Position += px;
					ModelMatrix *= Matrix4.CreateTranslation(px);
					break;
				case 'Y':
				case 'y':
					Vector3 py = new Vector3(0, trans, 0);
					Position += py;
					ModelMatrix *= Matrix4.CreateTranslation(py);
					break;
				case 'Z':
				case 'z':
					Vector3 pz = new Vector3(0, 0, trans);
					Position += pz;
					ModelMatrix *= Matrix4.CreateTranslation(pz);
					break;
			}
		}

	}
}

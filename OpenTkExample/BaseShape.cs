using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkExample
{
	public abstract class BaseShape
	{

		public Vector3 Position = Vector3.Zero;
		public Vector3 Rotation = Vector3.Zero;
		public Vector3 Scale = Vector3.One;

		public int VerticesCount { get; set; }
		public int IndicesCount;
		public int ColorDataCount;

		public Matrix4 ModelMatrix = Matrix4.Identity;
		public Matrix4 ViewMatrix = Matrix4.Identity;
		public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
		public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

		public abstract Vector3[] GetVertex();
		public abstract int[] GetIndices(int offset = 0);
		public abstract int[] GetNormalIndices(int offset = 0);
		//public abstract Vector3[] GetNormals();
		public abstract Vector3[] GetColorData();

		public Vector3[] Normals = new Vector3[0];


		public abstract void CalculateModelMatrix(); 
		public abstract void SetValueX(Matrix4 xVal);
		public abstract void SetValueY(Matrix4 yVal);

		public abstract void SetTranslation(float trans, char c);

		public virtual Vector3[] GetNormals()
		{
			return this.Normals;
		}
		public void CalculateNormals()
		{
			
			Vector3[] normals = new Vector3[VerticesCount];
			Vector3[] verts = GetVertex();
			int[] inds = GetIndices();
			for(int i = 0; i < IndicesCount; i += 3)
			{
				Vector3 v1 = verts[inds[i]];
				Vector3 v2 = verts[inds[i + 1 ]];
				Vector3 v3 = verts[inds[i + 2 ]];

				normals[inds[i]] += Vector3.Cross(v2 - v1,v3 -v1);
				normals[inds[i + 1]] += Vector3.Cross(v2 - v1,v3 - v1);
				normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
			}
			for(int i = 0; i < Normals.Length; i++)
			{
				normals[i] = normals[i].Normalized();
			}
			Normals = normals;
		}

	}
}

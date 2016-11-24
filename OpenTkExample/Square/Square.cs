using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTkExample.SquareType
{
	class Square : Shape
	{

		public Square()
		{
			VerticesCount = 4;
			IndicesCount = 6;
			ColorDataCount = 4;
		}

		public override void CalculateModelMatrix()
		{
			ModelMatrix = Matrix4.Scale(Scale) *
			Matrix4.CreateRotationX(Rotation.X) *
			Matrix4.CreateRotationY(Rotation.Y) *
			Matrix4.CreateRotationZ(Rotation.Z) *
			Matrix4.CreateTranslation(Position);
		}

		public override Vector3[] GetColorData()
		{
			return new Vector3[] {
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 1f, 0f)
			};
		}
	

		public override int[] GetIndices(int offset = 0)
		{
			int[] inds = new int[] {
                //left
                0, 1, 2,
				2, 3, 1
			};
			if (offset != 0)
			{
				for (int i = 0; i < inds.Length; i++)
				{
					inds[i] += offset;
				}
			}

			return inds;
		}

		public override Vector3[] GetVertex()
		{
			return new Vector3[]{
				new Vector3(-0.5f,  0.5f, 0f),
				new Vector3(-0.5f, -0.5f, 0f),
				new Vector3( 0.5f, -0.5f, 0f),
				new Vector3( 0.5f,  0.5f, 0f)
			};
		}

		public override void SetTranslation(float trans, char c)
		{
			throw new NotImplementedException();
		}

		public override void SetValueX(Matrix4 xVal)
		{
			throw new NotImplementedException();
		}

		public override void SetValueY(Matrix4 yVal)
		{
			throw new NotImplementedException();
		}
	}
}

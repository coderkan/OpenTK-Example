﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTkExample
{
	class Cube : BaseShape
	{

		public Cube()
		{
			this.VecticesCount = 8;
			this.IndicesCount = 36;
			this.ColorDataCount = 8;
		}



		public override void CalculateModelMatrix()
		{
			ModelMatrix = Matrix4.Scale(Scale) *
				Matrix4.CreateRotationX(Rotation.X) *
				Matrix4.CreateRotationY(Rotation.Y) *
				Matrix4.CreateRotationZ(Rotation.Z) *
				Matrix4.CreateTranslation(Position) ;
		}

 

		public override void SetValueX(Matrix4 xVal)
		{
			ModelMatrix = ModelMatrix * xVal;
		}
		public override void SetValueY(Matrix4 yVal)
		{
			ModelMatrix = ModelMatrix * yVal;
		}

		public void CalcMatrix()
		{
			Matrix4 mat = Matrix4.CreateRotationX(Rotation.X) *
			Matrix4.CreateRotationY(Rotation.Y) *
			Matrix4.CreateRotationZ(Rotation.Z);
			ModelMatrix = mat*ModelViewProjectionMatrix;
		}
	

		public override Vector3[] GetColorData()
		{
			return new Vector3[] {

				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f),
				//new Vector3(0f, 1f, 0f)
				

				//new Vector3(1f, 0f, 0f),
				//new Vector3( 0f, 0f, 1f),
				//new Vector3( 0f, 1f, 0f),
				//new Vector3( 1f, 0f, 0f),
				//new Vector3( 0f, 0f, 1f),
				//new Vector3( 0f, 1f, 0f),
				//new Vector3( 1f, 0f, 0f),
				//new Vector3( 0f, 0f, 1f)

				new Vector3(1f, 0f, 0f),
				new Vector3( 1f, 0f, 0f),
				new Vector3( 1f, 0f, 0f),
				new Vector3( 1f, 0f, 0f),
				new Vector3( 0f, 0f, 1f),
				new Vector3( 0f, 1f, 0f),
				new Vector3( 1f, 0f, 0f),
				new Vector3( 0f, 0f, 1f)


			};
		}

		public override int[] GetIndices(int offset = 0)
		{
			int[] inds = new int[] {
                //left
                0, 2, 1,
				0, 3, 2,
                //back
                1, 2, 6,
				6, 5, 1,
                //right
                4, 5, 6,
				6, 7, 4,
                //top
                2, 3, 6,
				6, 3, 7,
                //front
                0, 7, 3,
				0, 4, 7,
                //bottom
                0, 1, 5,
				0, 5, 4
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
			return new Vector3[]
			{
				new Vector3(-0.5f, -0.5f,  -0.5f),
				new Vector3(0.5f, -0.5f,  -0.5f),
				new Vector3(0.5f, 0.5f,  -0.5f),
				new Vector3(-0.5f, 0.5f,  -0.5f),
				new Vector3(-0.5f, -0.5f,  0.5f),
				new Vector3(0.5f, -0.5f,  0.5f),
				new Vector3(0.5f, 0.5f,  0.5f),
				new Vector3(-0.5f, 0.5f,  0.5f),
			};
		}
	}
}

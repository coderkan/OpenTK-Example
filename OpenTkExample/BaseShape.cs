﻿using OpenTK;
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

		public int VecticesCount;
		public int IndicesCount;
		public int ColorDataCount;

		public Matrix4 ModelMatrix = Matrix4.Identity;
		public Matrix4 ViewMatrix = Matrix4.Identity;
		public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
		public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

		public abstract Vector3[] GetVertex();
		public abstract int[] GetIndices(int offset = 0);
		public abstract Vector3[] GetColorData();

		public abstract void CalculateModelMatrix(); 
		public abstract void SetValueX(Matrix4 xVal);
		public abstract void SetValueY(Matrix4 yVal);

	}
}

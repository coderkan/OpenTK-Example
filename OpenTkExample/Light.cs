using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkExample
{

	public struct Light
	{
		public Vector3 Position;
		public Vector3 Intensities;

		public Light(Vector3 pos, Vector3 intensities)
		{
			this.Position = pos;
			this.Intensities = intensities;
		}
	};

}

using WPFLabs.Drawings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WPFLabs
{
	public class EntityVertex
	{
		public string? Name { get; init; }

		public int Size { get; private set; }

		public Color Color { get; set; }

		public EntityVertex(string? name, int size, Color color)
		{
			if (size < 10)
			{
				size = 10;
			}
			Name = name;
			Size = size;
			Color = color;
		}

		public void ReSize(int newSize)
		{
			if (newSize < 10)
			{
				newSize = 10;
			}
			Size = newSize;
		}

		public override string ToString()
		{
			return string.Join(SeparatorsForDrawingGraph.SeparatorForEntityVertexParameters, [Name, Size, Color.ToArgb()]);
		}
	}
}

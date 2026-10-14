using System;
using System.Linq;
using System.Drawing;

namespace WPFLabs.Drawings
{
	public abstract class DrawingEdge
	{
		public DrawingVertex VertexFrom { get; protected set; }

		public DrawingVertex VertexTo { get; protected set; }

		public int Weight { get; protected set; }

		public Color Color { get; set; }

		public DrawingEdge(DrawingVertex first,DrawingVertex second, int weight)
		{
			VertexFrom = first;
			VertexTo = second;
			Weight = weight;
			Color = Color.Black;
		}

		public abstract void DrawEdge(Graphics g);

		protected Point[] CorrectPositions()
		{
			int PosXFirst = VertexFrom.PosX;
			int PosXSecond = VertexTo.PosX;
			int PosYFirst = VertexFrom.PosY;
			int PosYSecond = VertexTo.PosY;
			int SizeFirst = VertexFrom._vertex.Size;
			int SizeSecond = VertexTo._vertex.Size;

			Point[] points = new Point[2];
			Point middleFirst = new(PosXFirst + SizeFirst / 2, PosYFirst + SizeFirst / 2);
			Point middleSecond = new(PosXSecond + SizeSecond / 2, PosYSecond + SizeSecond / 2);
			double degree = Math.Atan2((middleSecond.Y - middleFirst.Y), (middleSecond.X - middleFirst.X));
			points[0] = new Point((int)(middleFirst.X + SizeFirst/2 * Math.Cos(degree)), (int)(middleFirst.Y + SizeFirst/2 * Math.Sin(degree)));
			points[1] = new Point((int)(middleSecond.X - SizeSecond/2 * Math.Cos(degree)), (int)(middleSecond.Y - SizeSecond/2 * Math.Sin(degree)));
			return points;
		}

		public override string ToString()
		{
			return string.Join(SeparatorsForDrawingGraph.SeparatorForEdgeParameters, [VertexFrom.ToString(), VertexTo.ToString(), Weight, Color.ToArgb()]);
		}
	}
}

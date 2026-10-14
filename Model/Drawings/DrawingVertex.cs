using System.Drawing;

namespace WPFLabs.Drawings
{
	public class DrawingVertex
	{
		public EntityVertex _vertex {  get; init; }

		public int PosX { get;private set; }

		public int PosY { get; private set; }

		public DrawingVertex(int x,int y,EntityVertex vertex)
		{
			PosX = x;
			PosY = y;
			_vertex = vertex;
		}

		public DrawingVertex(int x, int y, string? name, int size, Color color) : this(x, y, new EntityVertex(name, size, color)) { }

		public void ChangePosition(int x, int y)
		{
			PosX = x;
			PosY = y;
		}

		public void ChangePosition(Point point) => ChangePosition(point.X, point.Y);

		public void DrawVertex(Graphics g)
		{
			Pen pen = new(_vertex.Color);
			Brush br = new SolidBrush(_vertex.Color);
			g.DrawEllipse(pen, PosX, PosY, _vertex.Size, _vertex.Size);
			g.DrawString(_vertex.Name, new Font("Times New Roman",10), br, PosX + _vertex.Size/3, PosY + _vertex.Size/3);
		}

		public override string ToString()
		{
			return string.Join(SeparatorsForDrawingGraph.SeparatorForVertexParameters, _vertex.ToString(), PosX, PosY);
		}
	}
}

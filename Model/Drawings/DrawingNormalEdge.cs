using System.Drawing;

namespace WPFLabs.Drawings
{
	public class DrawingNormalEdge : DrawingEdge
	{
		public DrawingNormalEdge(DrawingVertex first,DrawingVertex second,int weight) : base(first,second,weight)
		{
			//Hitbox = new Panel();
			//Hitbox.Visible = false;
			//Hitbox.Location = new Point(PosXFirst, PosYFirst);
			//Hitbox.Size = new Size(Math.Abs(PosXFirst - PosXSecond), Math.Abs(PosYFirst - PosYSecond));
		}

		public override void DrawEdge(Graphics g)
		{
			Point[] get = base.CorrectPositions();
			using (Pen pen = new Pen(Color, 2))
			{
				Brush br = new SolidBrush(Color);
				g.DrawLine(pen, get[0].X, get[0].Y, get[1].X, get[1].Y);
				g.DrawString(Weight.ToString(), new Font("Times New Roman", 15), br, new Point((get[0].X + get[1].X) / 2, (get[0].Y + get[1].Y) / 2));
				br.Dispose();
			}
		}
	}
}

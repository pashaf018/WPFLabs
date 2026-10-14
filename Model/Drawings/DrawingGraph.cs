using WPFLabs.CollectionsEdges;
using WPFLabs.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WPFLabs.Drawings
{
	public class DrawingGraph
	{
		public List<LinkedListEdges> Vertexes { get; private set; } = [];

		private int _drawingWidth;

		private int _drawingHeight;

		public int Size => Vertexes.Count;

		public DrawingGraph(int width,int height)
		{
			_drawingHeight = height;
			_drawingWidth = width;
		}

		public DrawingGraph(List<LinkedListEdges> verticies,int width,int height) : this(width,height)
		{
			Vertexes = verticies;
		}

		public void AddVertex(DrawingVertex? vertex)
		{
			if (vertex is null)
			{
				throw new ArgumentNullException("Вершина оказалась null");
			}
			if(Vertexes.Find(v => v.MainVertex._vertex.Name == vertex._vertex.Name) is not null)
			{
				throw new CollectionIndexNotNullException(vertex);
			}
			if (vertex._vertex.Size > _drawingWidth || vertex._vertex.Size > _drawingHeight)
			{
				throw new DrawingSizeTooBig(vertex._vertex.Size);
			}

			vertex.ChangePosition(CorrectPosition(vertex.PosX, vertex.PosY, vertex._vertex.Size));
			Vertexes.Add(new LinkedListEdges(vertex));
		}

		public DrawingVertex? getVertex(int index)
		{
			if(index < 0 || index >= Size)
			{
				throw new IndexOutOfRangeException("Позиция за границами коллекции вершин: " + index);
			}

			return Vertexes[index].MainVertex;
		}

		public Point CorrectPosition(int x, int y,int size)
		{
			if (x + size > _drawingWidth)
			{
				x = _drawingWidth - size;
			}
			if (x < 0)
			{
				x = 0;
			}
			if (y + size > _drawingHeight)
			{
				y = _drawingHeight - size;
			}
			if (y < 0)
			{
				y = 0;
			}
			return new Point(x, y);
		}

		public void DeleteVertex(DrawingVertex? vertex)
		{
			if (vertex is null)
			{
				throw new ArgumentNullException("Вершина оказалась null");
			}

			var itemToRemove = Vertexes.Find(v => v.MainVertex._vertex.Name == vertex._vertex.Name);
			if (itemToRemove is null)
			{
				return;
			}

			Vertexes.Remove(itemToRemove);
		}

		public void SetPictureSize(int width, int height)
		{
			_drawingWidth = width;
			_drawingHeight = height;
		}

		public Bitmap DrawGraph(Color colorEdge,Color colorVertex,DrawingEdge? hightlightEdge = null,DrawingVertex? hightlightVertex = null)
		{
			Bitmap bmp = new(_drawingWidth, _drawingHeight);
			Graphics graphics = Graphics.FromImage(bmp);
			Bitmap temp = new(32, 32);
			using(Graphics g = Graphics.FromImage(temp))
			{
				g.Clear(Color.Transparent);

				using(Pen p = new Pen(Color.FromArgb(125,0,0,0),1))
				{
					g.DrawLine(p, 0, 16, 32, 16);
					g.DrawLine(p, 16, 0, 16, 32);
				}
			}

			TextureBrush br = new TextureBrush(temp);
			br.WrapMode = WrapMode.Tile;
			graphics.FillRectangle(br,0, 0, _drawingWidth, _drawingHeight);


			foreach (var vertex in Vertexes)
			{
				if(vertex.MainVertex._vertex.Name == hightlightVertex?._vertex.Name)
				{
					vertex.MainVertex._vertex.Color = colorVertex;
				}

				vertex.MainVertex.DrawVertex(graphics);
				vertex.MainVertex._vertex.Color = Color.Black;

				foreach (var edge in vertex._collection)
				{
					if(edge == hightlightEdge || (edge.VertexFrom == hightlightEdge?.VertexTo && edge.VertexTo == hightlightEdge?.VertexFrom))
					{
						edge.Color = colorEdge;
					}

					edge.DrawEdge(graphics);
					edge.Color = Color.Black;
				}
			}
			return bmp;
		}

		public void AddEdge(DrawingVertex? From,DrawingVertex? to,int weight)
		{
			if(From is null || to is null)
			{
				throw new ArgumentNullException("Одна из вершин null");
			}
			if(From._vertex.Name == to._vertex.Name)
			{
				throw new ArgumentsMatchingException(From);
			}

			LinkedListEdges? foundFrom = Vertexes.Find(p => p.MainVertex._vertex.Name == From._vertex.Name);
			LinkedListEdges? foundTo = Vertexes.Find(p => p.MainVertex._vertex.Name == to._vertex.Name);
			if (foundFrom is null || foundTo is null)
			{
				throw new NullReferenceException("Одна из вершин не была найдена");
			}

			foundFrom.AddEdge(foundTo.MainVertex,weight);
			foundTo.AddEdge(foundFrom.MainVertex,weight);
		}

		public DrawingEdge? GetEdge(int positionFrom,int positionTo)
		{
			if(positionFrom < 0 || positionFrom >= Size)
			{
				throw new IndexOutOfRangeException("Позиция за границами коллекции вершин: " + positionFrom);
			}
			if(positionTo < 0 || positionTo >= Size)
			{
				throw new IndexOutOfRangeException("Позиция за границами коллекции вершин: " + positionTo);
			}

			DrawingEdge? get = Vertexes[positionFrom].GetEdge(Vertexes[positionTo].MainVertex);

			return get;
		}

		public override string ToString()
		{
			string info = "";
			foreach(LinkedListEdges lle in Vertexes)
			{
				info += lle.ToString() + SeparatorsForDrawingGraph.SeparatorForVertexes;
			}
			return info;
		}
	}
}

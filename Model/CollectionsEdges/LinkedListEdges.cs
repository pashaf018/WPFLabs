using WPFLabs.Drawings;
using WPFLabs.Exceptions;
using System.Collections.Generic;

namespace WPFLabs.CollectionsEdges
{
	public class LinkedListEdges
	{
		public DrawingVertex MainVertex { get; init; }

		public List<DrawingEdge> _collection{ get; private set; }

		public int Size => _collection.Count;

		public LinkedListEdges(DrawingVertex mainVertex)
		{
			MainVertex = mainVertex;
			_collection = [];
		}

		public void AddEdge(DrawingVertex vertexTo,int weight)
		{
			if (_collection.Find(p => p.VertexTo._vertex.Name == vertexTo._vertex.Name) is not null)
			{
				throw new CollectionIndexNotNullException(vertexTo);
			}

			_collection.Add(new DrawingNormalEdge(MainVertex,vertexTo,weight));
		}

		public void DeleteEdge(DrawingVertex vertexTo)
		{
			DrawingEdge? edge = _collection.Find(p => p.VertexTo._vertex.Name == vertexTo._vertex.Name);
			if (edge is null)
			{
				return;
			}

			_collection.Remove(edge);
		}

		public DrawingEdge? GetEdge(DrawingVertex to)
		{
			return _collection.Find(p => p.VertexTo._vertex.Name == to._vertex.Name);
		}

		public override string ToString()
		{
			string info = MainVertex.ToString() + SeparatorsForDrawingGraph.SeparatorForMainVertex;
			foreach(DrawingEdge edge in _collection)
			{
				info += edge.ToString() + SeparatorsForDrawingGraph.SeparatorForLinkedListEdgesParameters;
			}
			return info;
		}
	}
}

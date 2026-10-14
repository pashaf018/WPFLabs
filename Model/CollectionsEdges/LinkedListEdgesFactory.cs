using WPFLabs.Drawings;
using System;

namespace WPFLabs.CollectionsEdges
{
	public static class LinkedListEdgesFactory
	{
		public static LinkedListEdges CreateLinkedListEdges(string info)
		{
			string[] parameters = info.Split(SeparatorsForDrawingGraph.SeparatorForMainVertex, StringSplitOptions.RemoveEmptyEntries);
			if(parameters.Length < 2)
			{
				throw new ArgumentException("Не хватает данных для создания коллекции: " + parameters.Length);
			}

			DrawingVertex? mainVertex = DrawingVertexFactory.CreateDrawingVertex(parameters[0]);
			if(mainVertex is null)
			{
				throw new NullReferenceException("Главная вершина в коллекции оказалась null");
			}

			LinkedListEdges? edges = new LinkedListEdges(mainVertex);
			string[] edgesStr = parameters[1].Split(SeparatorsForDrawingGraph.SeparatorForLinkedListEdgesParameters,StringSplitOptions.RemoveEmptyEntries);
			if(edgesStr.Length == 0)
			{
				return edges;
			}

			try
			{
				foreach (string edgeParameters in edgesStr)
				{
					string[] drawingEdge = edgeParameters.Split(SeparatorsForDrawingGraph.SeparatorForEdgeParameters, StringSplitOptions.RemoveEmptyEntries);
					if (drawingEdge.Length < 4)
					{
						throw new ArgumentException("Не хватает параметров для ребра: " + drawingEdge.Length);
					}

					DrawingVertex vertexFrom = DrawingVertexFactory.CreateDrawingVertex(drawingEdge[0]);

					DrawingVertex vertexTo = DrawingVertexFactory.CreateDrawingVertex(drawingEdge[1]);

					if (!int.TryParse(drawingEdge[2], out int weight) || !int.TryParse(drawingEdge[3], out int color))
					{
						throw new ArgumentException("1 из аргументов для ребра неверный");
					}

					edges.AddEdge(vertexTo, weight);
				}

				return edges;
			} catch(ArgumentException)
			{
				throw;
			}
		}
	}
}

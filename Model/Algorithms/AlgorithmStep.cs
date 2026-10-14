using WPFLabs.Drawings;
using System;

namespace WPFLabs.Algorithms
{
	public class AlgorithmStep
	{
		public bool[] VisitedVertex { get; init; }

		public int[] Weights { get; init; }

		public int[] Previous { get; init; }

		public DrawingEdge? HighlightedEdge { get;init; }

		public DrawingVertex? HighlightedVertex { get; init; }

		public TypeOfStep StepType { get; init; }

		public AlgorithmStep(bool[] visited, int[] weights, int[] prev,DrawingEdge? edge,DrawingVertex? vertex,TypeOfStep type)
		{
			VisitedVertex = new bool[visited.Length];
			Array.Copy(visited, VisitedVertex, visited.Length);

			Weights = new int[weights.Length];
			Array.Copy(weights, Weights, weights.Length);

			Previous = new int[prev.Length];
			Array.Copy(prev, Previous, prev.Length);

			HighlightedEdge = edge;

			HighlightedVertex = vertex;

			StepType = type;
		}
	}
}

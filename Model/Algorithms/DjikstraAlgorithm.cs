using WPFLabs.Drawings;
using WPFLabs.Exceptions;
using System;
using System.Collections.Generic;

namespace WPFLabs.Algorithms
{
	public class DjikstraAlgorithm : BaseAlgorithm
	{
		public DrawingGraph? _collection { get; protected set; }

		public DjikstraAlgorithm(DrawingGraph graph,int from,int to)
		{
			Name = "Алгоритм Дэйкстры";
			Description = "Это алгоритм, который находит оптимальные маршруты и их длину " +
				"между одной конкретной вершиной и остальными вершинами графа.";
			CollectionSize = graph.Vertexes.Count;
			_collection = graph;
			ChangePoints(from, to);
		}

		public override List<AlgorithmStep> StartAlgorithm()
		{
			List<AlgorithmStep> steps = new List<AlgorithmStep>();

			//Инициализация нужных компонентов
			bool[] visited = new bool[CollectionSize];
			Array.Fill(visited, false);
			int[] weights = new int[CollectionSize];
			Array.Fill(weights, int.MaxValue);
			int[] previous = new int[CollectionSize];
			Array.Fill(previous, -1);
			weights[startingIndex] = 0;

			steps.Add(new AlgorithmStep(visited, weights, previous, null, null,TypeOfStep.Initialisation));

			//Алгоритм Дэйкстры
			for(int i = 0;i < CollectionSize;i++)
			{
				//Поиск нерассмотренной вершины с минимальным весом
				int MinWeight = int.MaxValue;
				int index = i;
				for(int j = 0;j < CollectionSize;j++)
				{
					if(MinWeight > weights[j] && visited[j] != true)
					{
						MinWeight = weights[j];
						index = j;
					}
					steps.Add(new AlgorithmStep(visited, weights, previous, null, _collection.getVertex(j), TypeOfStep.FindingMin));
				}

				visited[index] = true;
				steps.Add(new AlgorithmStep(visited, weights, previous, null, _collection.getVertex(index), TypeOfStep.ObservingVertex));

				// Поиск минимального расстояния для этой вершины
				for(int j = 0;j < CollectionSize;j++)
				{
					DrawingEdge? edge = _collection.GetEdge(index, j);
					if (edge is null)
					{
						continue;
					}
					steps.Add(new AlgorithmStep(visited, weights, previous, edge, _collection.getVertex(index), TypeOfStep.ObservingEdge));

					int newWeight = weights[index] + edge.Weight;
					if(newWeight < weights[j] && newWeight > 0)
					{
						weights[j] = newWeight;
						previous[j] = index;
					}
					steps.Add(new AlgorithmStep(visited, weights, previous, edge, _collection.getVertex(index), TypeOfStep.ObservingEdge));
				}
			}

			steps.Add(new AlgorithmStep(visited, weights, previous, null, null, TypeOfStep.End));

			return steps;
		}

		public override int GetLengthAnswer(AlgorithmStep finalStep,int from,int to)
		{
			if(finalStep.StepType != TypeOfStep.End)
			{
				throw new InvalidAlgorithmStepException(finalStep.StepType);
			}

			return finalStep.Weights[to];
		}
	}
}

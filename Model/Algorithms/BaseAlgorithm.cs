using System;
using System.Collections.Generic;

namespace WPFLabs.Algorithms
{
	public abstract class BaseAlgorithm
	{
		public string Name { get; protected set; }

		public string Description { get; protected set; }
		
		public int CollectionSize { get; protected set; }

		public int startingIndex { get; protected set; }

		public int endingIndex { get; protected set; }

		public abstract List<AlgorithmStep> StartAlgorithm();

		public abstract int GetLengthAnswer(AlgorithmStep finalStep, int from, int to);

		public void ChangePoints(int from, int to)
		{
			if (from < 0 || from >= CollectionSize)
			{
				throw new IndexOutOfRangeException("Начальная точка по позиции не найдена: " + from);
			}
			if (to < 0 || to >= CollectionSize)
			{
				throw new IndexOutOfRangeException("Конечная точка по позиции не найдена: " + to);
			}

			startingIndex = from;
			endingIndex = to;
		}
	}
}

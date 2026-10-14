using System;

namespace WPFLabs.Drawings
{
	public static class DrawingVertexFactory
	{
		public static DrawingVertex CreateDrawingVertex(string info)
		{
			string[] parameters = info.Split(SeparatorsForDrawingGraph.SeparatorForVertexParameters,StringSplitOptions.RemoveEmptyEntries);
			if(parameters.Length < 3)
			{
				throw new ArgumentException("Не хватает аргументов для создания вершины: " + parameters.Length);
			}

			try
			{
				EntityVertex entity = EntityVertexFactory.CreateEntity(parameters[0]);


				if (!int.TryParse(parameters[1], out int posX) || !int.TryParse(parameters[2], out int posY))
				{
					throw new ArgumentException("1 из аргументов для вершины неверного типа");
				}

				return new DrawingVertex(posX, posY, entity);
			}
			catch(ArgumentException)
			{
				throw;
			}
		}
	}
}

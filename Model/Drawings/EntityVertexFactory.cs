using System;
using System.Drawing;

namespace WPFLabs.Drawings
{
	public static class EntityVertexFactory
	{
		public static EntityVertex CreateEntity(string info)
		{
			string[] parameters = info.Split(SeparatorsForDrawingGraph.SeparatorForEntityVertexParameters,StringSplitOptions.RemoveEmptyEntries);
			if(parameters.Length < 3)
			{
				throw new ArgumentException("Не хватает аргументов для создания энтити: " + parameters.Length);
			}

			if (string.IsNullOrEmpty(parameters[0]) || !int.TryParse(parameters[1],out int size) || !int.TryParse(parameters[2],out int color))
			{
				throw new ArgumentException("1 из аргументов энтити оказался неверного типа");
			}

			return new EntityVertex(parameters[0],size, Color.FromArgb(color));
		}
	}
}

using WPFLabs.CollectionsEdges;
using System;
using System.Collections.Generic;
using System.IO;

namespace WPFLabs.Drawings
{
	public static class GraphFactory
	{
		public static DrawingGraph CreateGraph(string info,int pictureWidth,int pictureHeight)
		{
			List<LinkedListEdges> verticies = new List<LinkedListEdges>();
			try
			{
				string[] verticiesStr = info.Split(SeparatorsForDrawingGraph.SeparatorForVertexes, StringSplitOptions.RemoveEmptyEntries);
				foreach (string vs in verticiesStr)
				{
					LinkedListEdges toAdd = LinkedListEdgesFactory.CreateLinkedListEdges(vs);
					if (toAdd != null)
					{
						verticies.Add(toAdd);
					}
				}
			} catch(Exception)
			{
				throw;
			}

			return new DrawingGraph(verticies, pictureWidth, pictureHeight);
		}

		public static void SaveData(string filename,DrawingGraph? graph)
		{
			if(graph is null)
			{
				throw new ArgumentNullException("Граф оказался null");
			}

			if(File.Exists(filename))
			{
				File.Delete(filename);
			}


			using(StreamWriter fs = new(filename))
			{
				fs.Write(graph.ToString());
			}
			
		}
	}
}

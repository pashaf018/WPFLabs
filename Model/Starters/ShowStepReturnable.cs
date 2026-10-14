using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLabs.Algorithms;

namespace WPFLabs.Model.Starters
{
	public class ShowStepReturnable
	{
		public Color _colorEdge { get; private set; }
		public Color _colorVertex { get; private set; }
		public AlgorithmStep _currentStep { get; private set; }

		public string _info { get; private set; }

		public ShowStepReturnable(Color colorEdge, Color colorVertex, AlgorithmStep currentStep, string info)
		{
			_colorEdge = colorEdge;
			_colorVertex = colorVertex;
			_currentStep = currentStep;
			_info = info;
		}
	}
}

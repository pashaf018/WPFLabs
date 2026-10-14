using WPFLabs.Algorithms;
using WPFLabs.Drawings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Threading;
using WPFLabs.Model.Starters;

namespace WPFLabs.Starters
{
	public class StartDjikstra : IProgramStarter
	{
		private readonly Color ColorFindingMin = Color.Red;
		private readonly Color ColorObservingVertex = Color.Yellow;
		private readonly Color ColorObservingEdge = Color.Blue;

		private List<AlgorithmStep> _steps;

		private DispatcherTimer _timer;

		private int currentStep = 0;

		private DrawingGraph? _graph;

		private int Answer = -1;

		public StartDjikstra(BaseAlgorithm algorithm)
		{
			_steps = algorithm.StartAlgorithm();
			_graph = ((DjikstraAlgorithm) algorithm)._collection;
			Answer = algorithm.GetLengthAnswer(_steps[_steps.Count-1],algorithm.startingIndex,algorithm.endingIndex);
		}

		public DrawingGraph LoadData(string filename,int pictureWidth,int pictureHeight)
		{
			if (!File.Exists(filename))
			{
				throw new Exception("Файл не найден");
			}

			string bufferFromTextFile = "";
			using (FileStream fs = new(filename, FileMode.Open))
			{
				byte[] b = new byte[fs.Length];
				UTF8Encoding temp = new(true);
				while (fs.Read(b, 0, b.Length) > 0)
				{
					bufferFromTextFile += temp.GetString(b);
				}
			}

			string[] strs = bufferFromTextFile.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
			if (strs.Length == 0)
			{
				throw new Exception("В файле пусто");
			}

			try
			{
				DrawingGraph graph = GraphFactory.CreateGraph(strs[0], pictureWidth, pictureHeight);
				return graph;
			}
			catch
			{
				throw;
			}

		}

		public void SaveData(string filename)
		{
			if (_graph is null)
			{
				throw new ArgumentNullException("Граф оказался null");
			}

			if (File.Exists(filename))
			{
				File.Delete(filename);
			}


			using (StreamWriter fs = new(filename))
			{
				fs.Write(_graph.ToString());
			}
		}

		public ShowStepReturnable ShowStep()
		{
			Color colorEdge = Color.Black;
			Color colorVertex = Color.Black;
			switch(_steps[currentStep].StepType)
			{
				case TypeOfStep.ObservingEdge:
					colorEdge = ColorObservingEdge;
					colorVertex = ColorObservingVertex;
					break;
				case TypeOfStep.ObservingVertex:
					colorVertex = ColorObservingVertex;
					break;
				case TypeOfStep.FindingMin:
					colorVertex = ColorFindingMin;
					break;
				default:
					break;
			}

			string info = $"Действие в данный момент: {_steps[currentStep].StepType.ToString()}\r\n";

			info += "Массив посещённых вершин:\r\n";
			for (int i = 0; i < _steps[currentStep].VisitedVertex.Length; i++)
			{
				info += $"[{i}] = " + _steps[currentStep].VisitedVertex[i] + "\r\n";
			}
			info += "Массив расстояний:\r\n";
			for (int i = 0; i < _steps[currentStep].Weights.Length; i++)
			{
				info += $"[{i}] = " + _steps[currentStep].Weights[i] + "\r\n";
			}
			info += "Массив Предков:\r\n";
			for (int i = 0; i < _steps[currentStep].Previous.Length; i++)
			{
				info += $"[{i}] = " + _steps[currentStep].Previous[i] + "\r\n";
			}

			if (_steps[currentStep].StepType == TypeOfStep.End)
			{
				info += "\r\nОтвет! Кратчайшее найденное расстояние между вашими точками: " + Answer;
			}

			return new ShowStepReturnable(colorEdge,colorVertex, _steps[currentStep], info);
		}

		public void ShowNextStep()
		{
			currentStep++;
			if(currentStep >= _steps.Count)
			{
				currentStep = _steps.Count - 1;
				_timer.Stop();
				return;
			}

			ShowStep();
		}

		public void ShowPreviousStep()
		{
			currentStep--;
			if(currentStep < 0)
			{
				currentStep = 0;
				return;
			}

			ShowStep();
		}

		public void StartAlgorithm(int interval)
		{
			_timer = new();
			_timer.Interval = TimeSpan.FromMilliseconds(interval);
			_timer.Tick += (s,e) => ShowNextStep();
			_timer.Start();
			ShowStep();
		}

		public void ChangeSpeed(int interval)
		{
			if(_timer is null)
			{
				throw new NullReferenceException("Таймер оказался null");
			}
			if(interval <= 0 || interval >= 10000)
			{
				throw new ArgumentOutOfRangeException("Интервал слишком большой/маленький: " + interval);
			}

			_timer.Interval = TimeSpan.FromMilliseconds(interval);
		}

		public void StopAlgorithm()
		{
			if(_timer is null)
			{
				throw new NullReferenceException("Таймер оказался null");
			}
			_timer.Stop();
		}
	}
}

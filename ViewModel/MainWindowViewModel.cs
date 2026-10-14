using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WPFLabs.Algorithms;
using WPFLabs.Drawings;
using WPFLabs.Model.Starters;
using WPFLabs.MVVM;
using WPFLabs.Starters;

namespace WPFLabs.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private string defaultImagePath = @"C:\WaitingCat.jpg";
		private DrawingGraph _drawingGraph;
		private BaseAlgorithm _algorithm;
		private IProgramStarter _programStarter;
		private BitmapSource _bitmapSource;
		private BitmapSource _graphImage;
		private int _canvasHeight;
		private int _canvasWidth;
		private int numericUpDownFrom;
		private int numericUpDownTo;
		private int _maximumSliderValue;
		private string _textInfo;
		private int _sliderSpeed;

		public int CanvasHeight { get => _canvasHeight; set { _canvasHeight = value; } }

		public int CanvasWidth { get => _canvasWidth; set { _canvasWidth = value; } }

		public int NumericUpDownTo { get => numericUpDownTo; set { numericUpDownTo = value; } }

		public int NumericUpDownFrom { get => numericUpDownFrom; set { numericUpDownFrom = value; } }

		public int MaximumSliderValue { get => _maximumSliderValue; set { _maximumSliderValue = value; OnPropertyChanged(); } }

		public string TextInfo { get => _textInfo; set { _textInfo = value; OnPropertyChanged(); } }

		public int SliderSpeed { get => _sliderSpeed; set { _sliderSpeed = value; ChangeAlgorithmSpeed(); } }

		public BitmapSource GraphImage
		{
			get => _graphImage;
			set { _graphImage = value; OnPropertyChanged(); }
		}

		public BitmapSource BitmapSource
		{
			get
			{
				if (_bitmapSource == null)
				{
					BitmapImage bitmapSource = new BitmapImage();
					bitmapSource.BeginInit();
					bitmapSource.UriSource = new Uri(defaultImagePath, UriKind.Absolute);
					bitmapSource.CacheOption = BitmapCacheOption.OnLoad;
					bitmapSource.EndInit();
					bitmapSource.Freeze();

					_bitmapSource = bitmapSource;
				}
				
				return _bitmapSource;
			}
			set
			{
				if (_bitmapSource != value)
				{
					_bitmapSource = value;
					OnPropertyChanged();
				}
			}
		}

		public ICommand GenerateGraph { get; }
		public ICommand ShowDesc { get; }
		public ICommand StartAlgorithm { get; }
		public ICommand PauseAlgorithm { get; }
		public ICommand StopAlgorithm { get; }

		public MainWindowViewModel()
		{
			GenerateGraph = new RelayCommand(GenerateGraphLogic);
			ShowDesc = new RelayCommand(ShowDescription);
			StartAlgorithm = new RelayCommand(StartDjikstraAlgorithm);
			PauseAlgorithm = new RelayCommand(PauseDjikstraAlgorithm);
			StopAlgorithm = new RelayCommand(StopDjikstraAlgorithm);
		}

		private void GenerateGraphLogic(object parameter)
		{
			try
			{
				_drawingGraph = new((int)CanvasWidth, (int)CanvasHeight);
				Random rand = new Random();
				int size = rand.Next(3, 10);
				for (int i = 0; i < size; i++)
				{
					DrawingVertex add = new DrawingVertex(rand.Next(0, CanvasWidth), rand.Next(0, CanvasHeight), i.ToString(), 40, Color.Black);
					_drawingGraph.AddVertex(add);
					int edgesCount = rand.Next(1, Math.Min(_drawingGraph.Size, 4));

					HashSet<int> uniqueIndexes = new HashSet<int>();
					while (uniqueIndexes.Count < edgesCount)
					{
						int index = rand.Next(0, _drawingGraph.Size - 1);
						if (index == _drawingGraph.Size - 1)
						{
							edgesCount--;
							continue;
						}
						uniqueIndexes.Add(index);
					}

					foreach (int j in uniqueIndexes)
					{
						DrawingVertex? to = _drawingGraph.getVertex(j);
						if (to is null)
						{
							continue;
						}

						_drawingGraph.AddEdge(add, to, rand.Next(1, 100));
					}
				}

				DrawGraph(new ShowStepReturnable(Color.Black,Color.Black,null,null));

				MaximumSliderValue = _drawingGraph.Vertexes.Count - 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ShowDescription(object parameter)
		{
			MessageBox.Show("Это алгоритм, который находит оптимальные маршруты и их длину " +
				"между одной конкретной вершиной и остальными вершинами графа.");
		}
		
		private void StartDjikstraAlgorithm(object parameter)
		{
			if (_drawingGraph is null)
			{
				MessageBox.Show("Вы не добавили граф!", "Ошибка");
				return;
			}
			if (NumericUpDownFrom == NumericUpDownTo)
			{
				MessageBox.Show("Точки не могут совпадать", "Ошибка");
				return;
			}

			try
			{
				_algorithm = new DjikstraAlgorithm(_drawingGraph, NumericUpDownFrom, NumericUpDownTo);

				if (_programStarter == null)
				{
					_programStarter = new StartDjikstra(_algorithm);
					_programStarter.StepChanged += DrawGraph;
				}

				_programStarter.StartAlgorithm(SliderSpeed * 100);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void PauseDjikstraAlgorithm(object parameter)
		{
			try
			{
				_programStarter.StopAlgorithm();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void StopDjikstraAlgorithm(object parameter)
		{
			try
			{
				_programStarter.StopAlgorithm();
				_programStarter = null;
				TextInfo = string.Empty;

				DrawGraph(new ShowStepReturnable(Color.Black,Color.Black,null,null));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ChangeAlgorithmSpeed()
		{
			if (_programStarter == null)
			{
				return;
			}

			_programStarter.ChangeSpeed(SliderSpeed * 100);
		}

		private void DrawGraph(ShowStepReturnable parameters)
		{
			IntPtr hBitmap = IntPtr.Zero;
			try
			{
				Bitmap image = _drawingGraph?.DrawGraph(parameters._colorEdge, parameters._colorVertex, parameters._currentStep?.HighlightedEdge, parameters._currentStep?.HighlightedVertex);
				hBitmap = image.GetHbitmap();
				BitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				TextInfo = parameters._info;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				DeleteObject(hBitmap);
			}
		}

		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);
	}
}

using System;
using System.Collections.Generic;
using System.Windows.Input;
using WPFLabs.Drawings;
using WPFLabs.MVVM;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;

namespace WPFLabs.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private string defaultImagePath = @"C:\WaitingCat.jpg";
		private DrawingGraph _drawingGraph;
		private BitmapSource _bitmapSource;
		private BitmapSource _graphImage;
		private int _canvasHeight;
		private int _canvasWidth;
		private int numericUpDownFrom;
		private int numericUpDownTo;

		public int CanvasHeight { get => _canvasHeight; set { _canvasHeight = value; } }

		public int CanvasWidth { get => _canvasWidth; set { _canvasWidth = value; } }

		public int NumericUpDownTo { get => numericUpDownTo; set { numericUpDownTo = value; OnPropertyChanged(); } }

		public int NumericUpDownFrom { get => numericUpDownFrom; set { numericUpDownFrom = value; OnPropertyChanged(); } }

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

		public MainWindowViewModel()
		{
			GenerateGraph = new RelayCommand(GenerateGraphLogic);
			ShowDesc = new RelayCommand(ShowDescription);
		}

		private void GenerateGraphLogic(object parameter)
		{
			IntPtr hBitmap = IntPtr.Zero;
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

				Bitmap image = _drawingGraph?.DrawGraph(Color.Black, Color.Black);
				hBitmap = image.GetHbitmap();
				BitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				DeleteObject(hBitmap);
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


		}

		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);
	}
}

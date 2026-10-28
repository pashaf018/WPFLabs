using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFLabs;
using WPFLabs.ViewModel;

namespace WPFLabs.View
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MediaPlayer _clickSound;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel();

			var vm = DataContext as MainWindowViewModel;

			vm.VideoCommand += () => { VideoPlayer.Position = TimeSpan.Zero; VideoPlayer.Play(); };
			vm.OnVideoClose += () => { MainImage.UpdateLayout(); };
			_clickSound = new MediaPlayer();
			_clickSound.Open(new Uri(@"C:\ClickSound.mp3", UriKind.Absolute));
		}



		private void MainImage_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			if (this.DataContext is MainWindowViewModel viewModel)
			{
				viewModel.CanvasWidth = (int)e.NewSize.Width;
				viewModel.CanvasHeight = (int)e.NewSize.Height;
			}
		}

		private void SliderFrom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.DataContext is MainWindowViewModel viewModel)
			{
				viewModel.NumericUpDownFrom = (int)e.NewValue;
			}
		}

		private void SliderTo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.DataContext is MainWindowViewModel viewModel)
			{
				viewModel.NumericUpDownTo = (int)e.NewValue;
			}
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (this.DataContext is MainWindowViewModel viewModel)
			{
				viewModel.SliderSpeed = (int)e.NewValue;
			}
		}

		private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as MainWindowViewModel;

			vm.OnVideoEnded();
			vm.VideoCommand += () => { VideoPlayer.Position = TimeSpan.Zero; VideoPlayer.Play(); };
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (Resources["CosmocBackground"] is DrawingBrush br)
			{
				double tileWidth = (e.NewSize.Width / 2.5) / e.NewSize.Width.ToString().Length;
				double tileHeight = (e.NewSize.Height / 2.5) / e.NewSize.Height.ToString().Length;

				br.Viewport = new Rect(0,0,tileWidth,tileHeight);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Random rand = new Random();
			_clickSound.Position = TimeSpan.Zero;
			_clickSound.Play();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			_clickSound.Open(new Uri(@"C:\bark-fart.mp3"));
		}
	}
}

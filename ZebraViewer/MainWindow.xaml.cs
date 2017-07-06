using SimpleInjector;
using System.Windows;
using System.Windows.Media;
using ZebraViewer.Services;
using ZebraViewer.Services.Interfaces;
using ZebraViewer.ViewModels;

namespace ZebraViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var container = new Container();
            container.Register<IZebraApiService, ZebraApiService>();
            var zebraApiService = container.GetInstance<IZebraApiService>();
            
            DataContext = new MainViewModel(zebraApiService);
        }
        
        private void ImgLabel_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var element = sender as UIElement;
            var position = e.GetPosition(element);
            var transform = element.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);

            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            element.RenderTransform = new MatrixTransform(matrix);
        }

        private void TglPortChanged_Checked(object sender, RoutedEventArgs e)
        {
            this.CboPrinters.IsEnabled = false;
        }

        private void TglPortChanged_Unchecked(object sender, RoutedEventArgs e)
        {
            this.CboPrinters.IsEnabled = true;
        }
    }
}

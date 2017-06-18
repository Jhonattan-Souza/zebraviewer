using SimpleInjector;
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
    }
}

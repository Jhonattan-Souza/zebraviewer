using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using ZebraViewer.Models;
using ZebraViewer.Services;
using ZebraViewer.Services.Interfaces;
using System.Windows.Media;
using System.Windows.Input;

namespace ZebraViewer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IZebraApiService _zebraApiService;

        public MainViewModel(IZebraApiService zebraApiService)
        {
            _zebraApiService = zebraApiService;

            ChangePortCommand = new DelegateCommand(ExecuteChangePortCommand,
                CanExecuteChangePortCommand);
            UpdateLabelCommand = new DelegateCommand(ExecuteUpdateLabelCommand);
            //ZoomLabelImage = new DelegateCommand(ExecuteZoomLabelImage, () => Label != null);
            Printers = new ObservableCollection<Printer>(PrinterService.GetPrinters());
            IsPrintersEnabled = true;
        }

        public ObservableCollection<Printer> Printers { get; }

        private Printer _selectedPrinter;
        public Printer SelectedPrinter
        {
            get => _selectedPrinter;
            set
            {
                SetProperty(ref _selectedPrinter, value);
                ChangePortCommand.RaiseCanExecuteChanged();
            }
        }

        private BitmapImage _label;
        public BitmapImage Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public bool IsPrintersEnabled { get; set; }
        public bool IsPortChanged { get; set; }

        public DelegateCommand ChangePortCommand { get; }
        public DelegateCommand UpdateLabelCommand { get; }
        public DelegateCommand ZoomLabelImage { get; }
        
        private async void ExecuteChangePortCommand()
        {
            try
            {
                IsPrintersEnabled = !IsPortChanged;
                await Task.Run(() => PrinterService.SetPrinterPort(SelectedPrinter, 
                    !IsPortChanged));
            }
            catch (Exception o)
            {
                MessageBox.Show("Erro ao alterar a porta. Output: " + o.ToString(),
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);                
            }            
        }

        private bool CanExecuteChangePortCommand() => SelectedPrinter != null;

        private async void ExecuteUpdateLabelCommand()
        {
            var labelPath = string.Empty;

            try {
                labelPath = await _zebraApiService.GetLabelAsync(PrinterService.GetPrinterFileCode());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (labelPath != null)
            {
                Label = new BitmapImage();
                Label.BeginInit();
                Label.CacheOption = BitmapCacheOption.OnLoad;
                Label.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                Label.UriSource = new Uri(labelPath);
                Label.EndInit();
            } else
                MessageBox.Show("A etiqueta não foi gerada corretamente.",
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

       /* private void ExecuteZoomLabelImage(object sender, MouseWheelEventArgs e)
        {
            var element = sender as UIElement;
            var position = e.GetPosition(element);
            var transform = element.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);

            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            element.RenderTransform = new MatrixTransform(matrix);
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using ZebraViewer.Models;
using ZebraViewer.Services;

namespace ZebraViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            cboPrinters.ItemsSource = PrinterService.GetPrinters();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                if (cboPrinters.SelectedItem != null)
                    new PrinterService().SetPrinterPort((Printer)cboPrinters.SelectedItem, true);
            }
            catch (Exception exception) {
                MessageBox.Show($"Erro no fechamento do formuilário. \n{exception.Message}");
            }

            base.OnClosing(e);
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string printerCode = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt"));

            ZebraAPI.CreateImageFileFromPrinter(printerCode);

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.png")))
                imgLabel.Source = new BitmapImage(new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.png"), UriKind.Absolute));
            else
                MessageBox.Show("A etiqueta não foi gerada corretamente");

        }

        private void tglPortChanged_Clicked(object sender, RoutedEventArgs e)
        {
            if (cboPrinters.SelectedItem != null)
            {
                if (!tglPortChanged.IsChecked.HasValue)
                {
                    tglPortChanged.IsChecked = false;
                }

                new PrinterService().SetPrinterPort((Printer)cboPrinters.SelectedItem, (bool)!tglPortChanged.IsChecked);
            }
            else
            {
                MessageBox.Show("Selecione uma impressora por favor.");
                tglPortChanged.IsChecked = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            new ZebraAPI().GetPrinterFile(File.ReadAllText(@"E:\dev\label.txt"));

            imgLabel.Source = new BitmapImage(new Uri(@"E:\dev\label.png", UriKind.Absolute));
        }

        private void tglPortChanged_Checked(object sender, RoutedEventArgs e)
        {
            if (!tglPortChanged.IsChecked.HasValue)
            {
                tglPortChanged.IsChecked = false;
            }

            PrinterService.SetPrinterPort((Printer)cboPrinters.SelectedItem, @"E:\dev\label.txt", (bool)tglPortChanged.IsChecked);
        }
    }
}

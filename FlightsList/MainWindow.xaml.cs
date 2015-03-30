using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SITAOFPHelper.SmallClasses;
using System.Collections.ObjectModel;

namespace FlightsList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CSV_SPP_Reader reader = new CSV_SPP_Reader("C:\\Users\\Roman\\Desktop\\OFP\\spp.csv");
            //CSV_SPP_Reader reader = new CSV_SPP_Reader("C:\\Users\\Roman\\Desktop\\OFP\\spp.txt");
            ObservableCollection<Schedule_flight> flightList = new ObservableCollection<Schedule_flight>();
            dataGrid1.ItemsSource = reader.result();
        }
    }
}

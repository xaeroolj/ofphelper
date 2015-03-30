using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OFPLIST
{
    /// <summary>
    /// Interaction logic for IFPS_validate.xaml
    /// </summary>
    /// 
    delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);
    public partial class IFPS_validate : Window
    {
        BackgroundWorker worker;
        public ObservableCollection<string> resultList = new ObservableCollection<string>();
        public IFPS_validate()
        {
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            InitializeComponent();
            this.Loaded += IFPS_validate_Loaded;
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CopyFiles();
        }

        private void CopyFiles()
        {
            //StringBuilder SB = new StringBuilder();
            UpdateProgressBarDelegate updProgress = new UpdateProgressBarDelegate(progressBar1.SetValue);
            double value = 0;
            /*
            foreach (OFP_forList ofp in ((App)Application.Current).OFPLIST)
            {
                SB.AppendLine(ofp.IFPS_Validate());
                Dispatcher.Invoke(updProgress, new object[] { ProgressBar.ValueProperty, ++value });
            }
            */
            
            Parallel.ForEach(((App)Application.Current).OFPLIST, ofp => {
                
                //SB.AppendLine(ofp.IFPS_Validate());
                resultList.Add(ofp.IFPS_Validate());
                
                Dispatcher.Invoke(updProgress, new object[] { ProgressBar.ValueProperty, ++value });
            });

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                //newTB.AppendText(SB.ToString());
                outputList.DataContext = resultList;
                
            }));
            
        }

        
        void IFPS_validate_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar1.Maximum = ((App)Application.Current).OFPLIST.Count;
            progressBar1.Value = 0;
            worker.RunWorkerAsync();
        }
    }
}

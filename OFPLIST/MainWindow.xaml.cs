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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.ComponentModel;
using System.Configuration;
using SITAOFPHelper;
using System.IO;




namespace OFPLIST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 1. добавить:
    /// если
    /// _ok - в конце файла, результат обработки
    /// _error
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {

        IntPtr viewerHandle = IntPtr.Zero;
        IntPtr installedHandle = IntPtr.Zero;

        const int WM_DRAWCLIPBOARD = 0x308;
        const int WM_CHANGECBCHAIN = 0x30D;
        [DllImport("user32.dll")]
        private extern static IntPtr SetClipboardViewer(IntPtr hWnd);
        [DllImport("user32.dll")]
        private extern static int ChangeClipboardChain(IntPtr hWnd, IntPtr hWndNext);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        public MainWindow()
        {
            
            this.Loaded += MainWindow_Loaded;
            
            InitializeComponent();
           
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            openTxtBox.Text = Properties.Settings.Default.oFp_dir;
            cFP_List.DataContext = ((App)Application.Current).OFPLIST;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                installedHandle = hwndSource.Handle;
                viewerHandle = SetClipboardViewer(installedHandle);
                hwndSource.AddHook(new HwndSourceHook(this.hwndSourceHook));
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ChangeClipboardChain(this.installedHandle, this.viewerHandle);
            int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            e.Cancel = error != 0;

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            this.viewerHandle = IntPtr.Zero;
            this.installedHandle = IntPtr.Zero;
            base.OnClosed(e);

        }

        IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_CHANGECBCHAIN:
                    this.viewerHandle = lParam;
                    if (this.viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(this.viewerHandle, msg, wParam, lParam);
                    }

                    break;

                case WM_DRAWCLIPBOARD:
                    EventArgs clipChange = new EventArgs();
                    OnClipboardChanged(clipChange);

                    if (this.viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(this.viewerHandle, msg, wParam, lParam);
                    }

                    break;

            }
            return IntPtr.Zero;
        }

        private void OnClipboardChanged(EventArgs clipChange)
        {
            try
            {
                OFPCheck();
            }
            catch (Exception ex)
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Для улучшения продукта отправте расчет на r.trehlebov@rossiya-airlines.com");
                MessageBox.Show(sb.ToString());
               
            }
        }

        private void OFPCheck()
        {
            if (Clipboard.ContainsText() == true)
            {
                //SITAOFPHelper.PreOpsStatic.OFPinside
                if (OFP.OFPinside(OFP.GetTrimmedMassive(Clipboard.GetText())) == true)
                {
                    OFP_forList CurentOFP = new OFP_forList(Clipboard.GetText());

                    ((App)Application.Current).OFPLIST.Add(CurentOFP);

                }
            }
        }

        private void oPenFolderDialogBtn_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            openTxtBox.Text = dialog.SelectedPath + "\\";
            //exportStartBtn.Visibility = Visibility.Visible;
            Properties.Settings.Default.oFp_dir = openTxtBox.Text;
            Properties.Settings.Default.Save();
        }

        private void exportStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).OFPLIST.Count > 0 && Properties.Settings.Default.oFp_dir.Length > 2)
            {
                foreach (OFP_forList element in ((App)Application.Current).OFPLIST)
                {
                    
                    StringBuilder SB = new StringBuilder();

                    SB.Append(openTxtBox.Text);
                    SB.Append(element.Flight);
                    SB.Append("_");
                    if (element.DepAirField == "UKFF") element.DepAirField = "URFF";
                    SB.Append(element.DepAirField);
                    SB.Append("_");
                    if (element.ArrAirField == "UKFF") element.ArrAirField = "URFF";
                    SB.Append(element.ArrAirField);
                    SB.Append("_");
                    SB.Append(element.FlightDate.Date.ToString("yyMMdd"));
                    SB.Append(".txt");
                    

                    System.IO.StreamWriter file = new System.IO.StreamWriter(SB.ToString());
                    foreach(string line in element.OriginalOFPText)
                    {
                        file.WriteLine(line);
                    }
                    file.Close();
                }
                //FileSystemWatcher
                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = Properties.Settings.Default.oFp_dir;
                // Watch for changes in renaming of files
                watcher.NotifyFilter = NotifyFilters.FileName;
                // Only watch text files.
                watcher.Filter = "*.txt";

                // Add event handlers.
                watcher.Renamed += watcher_Renamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
        }

        void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (((App)Application.Current).OFPLIST.Count > 0)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate 
                    { 
                        foreach (OFP_forList element in ((App)Application.Current).OFPLIST)
                        {

                            element.checkFile(openTxtBox.Text);

                        }
                    }));
                //linq
                var pendingQuery = from OFP_forList ofp in (((App)Application.Current).OFPLIST)
                            where ofp.StS == "filePending"
                            select ofp;
                var errorQuery = from OFP_forList ofp in (((App)Application.Current).OFPLIST)
                            where ofp.StS == "fileError"
                            select ofp;
                var okQuery = from OFP_forList ofp in (((App)Application.Current).OFPLIST)
                            where ofp.StS == "fileOk"
                            select ofp;

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    pendingLable.Text = pendingQuery.Count().ToString();
                    errorLable.Text = errorQuery.Count().ToString();
                    okLable.Text = okQuery.Count().ToString();
                }));
                
            }
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).OFPLIST.Clear();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).OFPLIST.Count > 0)
            {
                foreach (OFP_forList element in ((App)Application.Current).OFPLIST)
                {
                    
                    element.checkFile(openTxtBox.Text);
 
                }
            }
        }

        private void cFP_List_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            {
                if (e.ExtentHeightChange > 0.0)
                    ((ScrollViewer)e.OriginalSource).ScrollToEnd();
            }
        }

        private void openNewWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new IFPS_validate();
            newWindow.Show();

        }

        private void mouseRecorderBtn_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}

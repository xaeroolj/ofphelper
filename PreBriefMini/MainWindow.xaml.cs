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
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.IO;
using System.Deployment.Application;
using System.Net.NetworkInformation;
using PreBriefMini.Statistic_Service;
using System.Security.Principal;


namespace PreBriefMini
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
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

        public string EX = "";

        public MainWindow()
        {
            
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            System.Net.WebProxy.GetDefaultProxy().UseDefaultCredentials = true;
            /*
            mode1.Click += new RoutedEventHandler(mode1_Click);
            mode2.Click += new RoutedEventHandler(mode2_Click);
            mode9.Click += new RoutedEventHandler(mode9_Click);
            USE_REG_Switch.Click += new RoutedEventHandler(USE_REG_Switch_Click);
            */
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            //System.Net.WebRequest.DefaultWebProxy = System.Net.WebRequest.GetSystemWebProxy();
            
            CheckNetworkState();
        }

        private void CheckNetworkState()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                // Сеть доступна
                netIndicator1.Fill = new SolidColorBrush(Colors.Green);
                using(Statistic_ServiceClient proxy = new Statistic_ServiceClient())
                {
                    try
                    {
                        
                        
                        proxy.GetServerTime();
                        netIndicator2.Fill = new SolidColorBrush(Colors.Green);
                    }
                    catch(Exception ex)
                    {
                        netIndicator2.Fill = new SolidColorBrush(Colors.Red);
                        StringBuilder sb = new StringBuilder();
                
                        sb.AppendLine();
                        sb.AppendLine("Error Source:");
                        sb.AppendLine(ex.Source);
                        sb.AppendLine("Message: ");
                        sb.AppendLine(ex.Message);
                        sb.AppendLine("StackTrace: ");
                        sb.AppendLine(ex.StackTrace.ToString());
                        sb.AppendLine("TargetSite: ");
                        sb.AppendLine(ex.TargetSite.ToString());


                        EX = sb.ToString();
                    }
                }
                
            }
            else
            {
                // Сеть недоступна
                netIndicator1.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            //CheckNetworkState();
        }
        #region CHGMODE definition
        /*
        void USE_REG_Switch_Click(object sender, RoutedEventArgs e)
        {
            char tempchar = '0';
            if (mode1.IsChecked == true) tempchar = '1';
            if (mode2.IsChecked == true) tempchar = '2';
            if (mode9.IsChecked == true) tempchar = '9';

            bool tempbool = true;
            if (USE_REG_Switch.IsChecked == true) tempbool = true;
            if (USE_REG_Switch.IsChecked == false) tempbool = false;

            if (((App)Application.Current).OFPLIST.Count > 0)
            {
                string teststring = ((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].generateCHG(tempchar, tempbool);
                Clipboard.Clear();
                FlowDocument document1 = new FlowDocument();
                FlowDocument document2 = new FlowDocument();

                FPLrtb.Document = document1;
                FPLrtb.AppendText(((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].IcaoFPL.FPLstring.ToString());
                CHGrtb.Document = document2;
                CHGrtb.AppendText(teststring);
                Clipboard.SetText(teststring);
            }
            Properties.Settings.Default.USE_REG=tempbool;
            Properties.Settings.Default.Save();
        }

        void mode9_Click(object sender, RoutedEventArgs e)
        {
            char tempchar = '0';
            if (mode1.IsChecked == true) tempchar = '1';
            if (mode2.IsChecked == true) tempchar = '2';
            if (mode9.IsChecked == true) tempchar = '9';

            bool tempbool = true;
            if (USE_REG_Switch.IsChecked == true) tempbool = true;
            if (USE_REG_Switch.IsChecked == false) tempbool = false;

            if (((App)Application.Current).OFPLIST.Count > 0)
            {

                string teststring = ((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].generateCHG(tempchar,tempbool);
                Clipboard.Clear();
                FlowDocument document1 = new FlowDocument();
                FlowDocument document2 = new FlowDocument();

                FPLrtb.Document = document1;
                FPLrtb.AppendText(((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].IcaoFPL.FPLstring.ToString());
                CHGrtb.Document = document2;
                CHGrtb.AppendText(teststring);
                Clipboard.SetText(teststring);
            }
            Properties.Settings.Default.CHGMode = '9';
            Properties.Settings.Default.Save();

            
        }

        void mode2_Click(object sender, RoutedEventArgs e)
        {
            char tempchar = '0';
            if (mode1.IsChecked == true) tempchar = '1';
            if (mode2.IsChecked == true) tempchar = '2';
            if (mode9.IsChecked == true) tempchar = '9';

            bool tempbool = true;
            if (USE_REG_Switch.IsChecked == true) tempbool = true;
            if (USE_REG_Switch.IsChecked == false) tempbool = false;

            if (((App)Application.Current).OFPLIST.Count > 0)
            {

                string teststring = ((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].generateCHG(tempchar,tempbool);
                Clipboard.Clear();
                FlowDocument document1 = new FlowDocument();
                FlowDocument document2 = new FlowDocument();

                FPLrtb.Document = document1;
                FPLrtb.AppendText(((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].IcaoFPL.FPLstring.ToString());
                CHGrtb.Document = document2;
                CHGrtb.AppendText(teststring);
                Clipboard.SetText(teststring);
            }
            Properties.Settings.Default.CHGMode = '2';
            Properties.Settings.Default.Save();
        }

        void mode1_Click(object sender, RoutedEventArgs e)
        {
            
            
            char tempchar ='0';
                    if (mode1.IsChecked == true) tempchar = '1';
                    if (mode2.IsChecked == true) tempchar = '2';
                    if (mode9.IsChecked == true) tempchar = '9';

                    bool tempbool = true;
                    if (USE_REG_Switch.IsChecked == true) tempbool = true;
                    if (USE_REG_Switch.IsChecked == false) tempbool = false;
            
                    if (((App)Application.Current).OFPLIST.Count > 0)
                    {

                        string teststring = ((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].generateCHG(tempchar,tempbool);
                        Clipboard.Clear();
                        FlowDocument document1 = new FlowDocument();
                        FlowDocument document2 = new FlowDocument();

                        FPLrtb.Document = document1;
                        FPLrtb.AppendText(((App)Application.Current).OFPLIST[((App)Application.Current).OFPLIST.Count - 1].IcaoFPL.FPLstring.ToString());
                        CHGrtb.Document = document2;
                        CHGrtb.AppendText(teststring);
                        Clipboard.SetText(teststring);
                    }
                    Properties.Settings.Default.CHGMode = '1';
                    Properties.Settings.Default.Save();
        }
        */
        #endregion

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
                        
            switch (Properties.Settings.Default.CHGMode)
            {
                case '1':
                    mode1.IsChecked = true;

                    break;
                case '2':
                    mode2.IsChecked = true;
                    break;
                default:
                    mode9.IsChecked = true;
                    break;
            }

            switch (Properties.Settings.Default.USE_REG)
            {
                case true:
                    USE_REG_Switch.IsChecked= true;
                    break;
                case false:
                    USE_REG_Switch.IsChecked = false;
                    break;
                default:
                    USE_REG_Switch.IsChecked = true;
                    break;
            }



            OFPCheck();
        }


        #region Copypast Definition
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
        #endregion
        private void OnClipboardChanged(EventArgs clipChange)
        {
            try
            {
                OFPCheck();
            }
            catch(Exception ex)
            {
                
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Для улучшения продукта отправте расчет на r.trehlebov@rossiya-airlines.com");
                /*
                sb.AppendLine();
                sb.AppendLine("Error Source:");
                sb.AppendLine(ex.Source);
                sb.AppendLine("Message: ");
                sb.AppendLine(ex.Message);
                sb.AppendLine("StackTrace: ");
                sb.AppendLine(ex.StackTrace.ToString());
                sb.AppendLine("TargetSite: ");
                sb.AppendLine(ex.TargetSite.ToString());
                */


                MessageBox.Show(sb.ToString());
                //Saved As a copy-rights!
                // Details: http://blogs.microsoft.co.il/blogs/tamir/archive/2007/10/24/clipboard-setdata-getdata-troubles-with-vpc-and-ts.aspx 
            }
        }

        private void OFPCheck()
        {
            if (Clipboard.ContainsText() == true)
            {
                if (OFP.OFPinside(OFP.GetTrimmedMassive(Clipboard.GetText())) == true)
                {
                    OFP CurentOFP = new OFP(Clipboard.GetText());

                    ((App)Application.Current).OFPLIST.Add(CurentOFP);
                    
                    char tempchar ='0';
                    if (mode1.IsChecked == true) tempchar = '1';
                    if (mode2.IsChecked == true) tempchar = '2';
                    if (mode9.IsChecked == true) tempchar = '9';
                    bool tempbool = true;
                    if (USE_REG_Switch.IsChecked == true) tempbool = true;
                    if (USE_REG_Switch.IsChecked == false) tempbool = false;

                    string teststring = CurentOFP.generateCHG(tempchar,tempbool);
                    Clipboard.Clear();
                    FlowDocument document1 = new FlowDocument();
                    FlowDocument document2 = new FlowDocument();
                    
                    FPLrtb.Document = document1;
                    FPLrtb.AppendText(CurentOFP.IcaoFPL.FPLstring.ToString());
                    CHGrtb.Document = document2;
                    CHGrtb.AppendText(teststring);
                    Clipboard.SetText(teststring);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            string dirpatch = "";
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                //строка для отладки
                 dirpatch = "CHG_maker_log.txt";
                 //dirpatch = @"C:\Users\Roman\Desktop\OFP" + "\\CHG_maker_log.txt";

            }
            else
            {
                //строка для продакшена
                 dirpatch = ApplicationDeployment.CurrentDeployment.DataDirectory.ToString()+ "\\CHG_maker_log.txt";
            }
                
                try
                {
                    //((App)Application.Current).OFPLIST
                    string dateTImeNow = String.Format("{0:MM.dd.yyyy HH:mm:ss}", DateTime.Now);
                    string username = Environment.UserName;
                    //
                    Statistic_ServiceClient service_proxy = new Statistic_ServiceClient();
                    StreamWriter sw = File.AppendText(dirpatch);
                        
                            foreach(OFP tempOfp in ((App)Application.Current).OFPLIST)
                            {
                                sw.WriteLine(tempOfp.Flight + " " + dateTImeNow + " " + username);
                                //service_proxy.AddLogAsync(tempOfp.Flight + " " + dateTImeNow + " " + username);
                            }
                    service_proxy.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file. Error message: " + ex.Message);
                }
            
        }

        private void netIndicator2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(EX);
            
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using SITAOFPHelper;
using System.Collections.ObjectModel;

namespace OFPLIST
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public ObservableCollection<OFP_forList> OFPLIST = new ObservableCollection<OFP_forList>();
        
    }
}

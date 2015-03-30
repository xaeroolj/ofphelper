﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using SITAOFPHelper;

namespace PreBriefMini
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ObservableCollection<OFP> OFPLIST = new ObservableCollection<OFP>();
    }
}

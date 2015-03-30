using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SITAOFPHelper;
using System.IO;
using System.ComponentModel;

namespace OFPLIST 
{
    public class OFP_forList  : OFP,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string stS = "def";
        public string StS
        {
            get { return stS; }
            set {
                stS = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("StS");
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        
        public OFP_forList(): base() { }
        public OFP_forList(string str) : base (str) {}

        public void checkFile(string filepatch)
        {
                StringBuilder SB = new StringBuilder();

                SB.Append(filepatch);
                SB.Append(this.Flight);
                SB.Append("_");
                SB.Append(this.DepAirField);
                SB.Append("_");
                SB.Append(this.ArrAirField);

                SB.Append("_");
                SB.Append(this.FlightDate.Date.ToString("yyMMdd"));

                string fileOk    = SB.ToString() + "_ok.txt";
                string fileError = SB.ToString() + "_error.txt";
                string file      = SB.ToString() + ".txt";
                string returnString = "filePending";
                if (File.Exists(fileOk) == true) returnString = "fileOk";
                if (File.Exists(fileError) == true) returnString = "fileError";
                if (File.Exists(file) == true) returnString = "filePending";
                this.StS = returnString;
                   
        }
    }
}

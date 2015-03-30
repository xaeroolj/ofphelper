using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SITAOFPHelper.SmallClasses
{

        class PreBriefClass
    {
        private string flightNbr;
        public string FlightNbr
        {
            get { return flightNbr; }
            set { flightNbr = value; }
        }

        private List<string> mainPart;
        public List<string> MainPart
        {
            get { return mainPart; }
            set { mainPart = value; }
        }

        private List<string> etopsPart;
        public List<string> EtopsPart
        {
            get { return etopsPart; }
            set { etopsPart = value; }
        }

        private List<string> tnkPart;
        public List<string> TnkPart
        {
            get { return tnkPart; }
            set { tnkPart = value; }
        }

        public PreBriefClass(){
            this.MainPart = new List<string>();
            this.EtopsPart = new List<string>();
            this.TnkPart = new List<string>();
        }

            
    }
}

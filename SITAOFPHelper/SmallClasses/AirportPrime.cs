using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SITAOFPHelper.SmallClasses
{
    class AirportPrime
    {
        private string iATACode;
        public string IATACode
        {
            get { return iATACode; }
            set { iATACode = value; }
        }

        private string iCAOCode;
        public string ICAOCode
        {
            get { return iCAOCode; }
            set { iCAOCode = value; }
        }

        private string fAACode;
        public string FAACode
        {
            get { return fAACode; }
            set { fAACode = value; }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        
        private string airport;
        public string Airport
        {
            get { return airport; }
            set { airport = value; }
        }
        
        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        
        public AirportPrime(){}




    }
}

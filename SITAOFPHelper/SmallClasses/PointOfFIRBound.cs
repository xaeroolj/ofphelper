using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SITAOFPHelper.SmallClasses
{
    class PointOfFIRBound
    {
        private AirportPrime apICAO;
        public AirportPrime APICAO
        {
            get { return apICAO; }
            set { apICAO = value; }
        }

        private DateTime eET; //estimate elapsed time
        public DateTime EET
        {
            get { return eET; }
            set { eET = value; }
        }

        public PointOfFIRBound() { }
        public PointOfFIRBound(string inputSTR)
        {
            AirportPrime tempAP = new AirportPrime();
            tempAP.ICAOCode = inputSTR.Substring(0, 4);
            apICAO = tempAP;
            int hour = Convert.ToInt32(inputSTR.Substring(4, 2));
            int minute = Convert.ToInt32(inputSTR.Substring(6, 2));
            try
            {
                eET = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, hour, minute, 0);
            }
            catch
            {
                eET = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month - 1, DateTime.UtcNow.Day, hour, minute, 0);
            }

        }

        public static PointOfFIRBound GeneratePOB(string inputSTR)
        {
            PointOfFIRBound ReturnItem = new PointOfFIRBound(inputSTR);
            return ReturnItem;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();


            sb.Append(this.apICAO.ICAOCode.ToString());
            if (this.eET.Hour < 10)
            {
                sb.Append("0");
                sb.Append(this.eET.Hour.ToString());
            }
            else
            {
                sb.Append(this.eET.Hour.ToString());
            }
            if (this.eET.Minute < 10)
            {
                sb.Append("0");
                sb.Append(this.eET.Minute.ToString());
            }
            else
            {
                sb.Append(this.eET.Minute.ToString());
            }

            return sb.ToString();


        }

    }
}

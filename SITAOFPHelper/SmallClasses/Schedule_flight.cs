using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SITAOFPHelper.SmallClasses
{
    public class Schedule_flight
    {
        //Begin of Class
        /*
         * Для импорта из CSV файла.
        */
        //Begin of Fields
        #region All Fields
        
            
            //Номер рейса
            private string flight_N = "";
            public string Flight_N
            {
                get { return flight_N; }
                set { flight_N = value; }
            }
            //Тип ВС
            private string acType = "";
            public string AcType 
            {
                get { return acType; }
                set { acType = value; }
            }
            //Регистр. номер ВС
            private string regN = "";
            public string RegN
            {
                get { return regN; }
                set { regN = value; }
            }
            //Аэродром вылета
            private string aDep = "";
            public string ADep
            {
                get { return aDep; }
                set { aDep = value; }
            }
            //Аэродром назначения
            private string aDes = "";
            public string ADes
            {
                get { return aDes; }
                set { aDes = value; }
            }
            //EOBT (UTC)
            private string eOBT = "";
            public string EOBT
            {
                get { return eOBT; }
                set { eOBT = value; }
            }
            //ETA (UTC)
            private string eTA = "";
            public string ETA
            {
                get { return eTA; }
                set { eTA = value; }
            }
            //RTW
            private string rTW = "";
            public string RTW
            {
                get { return rTW; }
                set { rTW = value; }
            }
            //RLW
            private string rLW = "";
            public string RLW
            {
                get { return rLW; }
                set { rLW = value; }
            }
            //DOF
            private string dOF = "";
            public string DOF
            {
                get { return dOF; }
                set { dOF = value; }
            }

        #endregion
        //End of Fields

        //Begin of Constructors
        #region Constructors
        public Schedule_flight() { }

        #endregion
        //End of Constructors

        //End of Class

    }
}

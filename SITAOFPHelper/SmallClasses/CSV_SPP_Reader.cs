using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace SITAOFPHelper.SmallClasses
{
    public class CSV_SPP_Reader
    {
        /*Begin of Class
         * 
         * Для импорта из CSV файла.
        */
        //Begin of Fields
        #region Fields

        //Номер рейса
        private int flight_N = 0;
        //Тип ВС
        private int acType = 0;
        //Регистр. номер ВС
        private int regN = 0;
        //Аэродром вылета
        private int aDep = 0;
        //Аэродром назначения
        private int aDes = 0;
        //EOBT (UTC)
        private int eOBT = 0;
        //ETA (UTC)
        private int eTA = 0;
        //RTW
        private int rTW = 0;
        //RLW
        private int rLW = 0;

        private List<Schedule_flight> flight_list = new List<Schedule_flight>();

        #endregion
        //End of Fields

        //Begin of Constructions
        #region Constructions

        public CSV_SPP_Reader() { }
        public CSV_SPP_Reader(string filePath)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(1251));
                string line = sr.ReadLine();
                String[] Laters = line.Trim().Split(';');
                for (int i = 0; i <= Laters.Length-1; i++)
                {
                    #region switch
                    switch (Laters[i])
                    {
                        case "Номер рейса":
                            flight_N = i;
                            break;

                        case "Тип ВС":
                            acType = i;
                            break;

                        case "Регистр. номер ВС":
                            regN = i;
                            break;

                        case "Аэродром вылета":
                            aDep = i;
                            break;

                        case "Аэродром назначения":
                            aDes = i;
                            break;

                        case "EOBT (UTC)":
                            eOBT = i;
                            break;

                        case "ETA (UTC)":
                            eTA = i;
                            break;

                        case "RTW":
                            rTW = i;
                            break;

                        case "RLW":
                            rLW = i;
                            break;
                    }
                    #endregion
                }

                
                while ((line = sr.ReadLine()) != null)
                {
                    String[] laters = line.Trim().Split(';');
                    if (laters.Length > 5 && laters[this.flight_N] != "")
                    { 
                        Schedule_flight temp = new Schedule_flight();
                        temp.Flight_N = laters[this.flight_N];
                        temp.AcType = laters[this.acType];
                        temp.RegN = laters[this.regN];
                        temp.ADep = laters[this.aDep];
                        temp.ADes = laters[this.aDes];
                        temp.EOBT = laters[this.eOBT];
                        temp.ETA = laters[this.eTA];
                        temp.RTW = laters[this.rTW];
                        temp.RLW = laters[this.rLW];
                        flight_list.Add(temp);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Could not read the file");
            }
        }
        #endregion
        //End of Constructions

        public List<Schedule_flight> result()
        {
            return this.flight_list;
        }
    }
}

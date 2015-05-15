using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SITAOFPHelper;
using System.ComponentModel;
using HtmlAgilityPack;
using System.Net;

namespace SITAOFPHelper
{
    public partial class OFP 
    {
        //Begin of Class

        //Begin of Fields
        private List<string> etpList = new List<string>();

        public List<string> ETPList 
        {
            get { return etpList; }
            set { etpList = value; }
        }

        private bool backFlight = false;
        public bool BackFlight
        {
            get { return backFlight; }
            set { backFlight = value; }
        }
        private DateTime cFPGeneratedTimeStamp;
        public DateTime CFPGeneratedTimeStamp
        {
            get { return cFPGeneratedTimeStamp; }
            set { cFPGeneratedTimeStamp = value; }
        }
        private string cFPREF;
        public string CFPREF
        {
            get { return cFPREF; }
            set { cFPREF = value; }
        }
        private string flight;
        public string Flight
        {
            get { return flight; }
            set { flight = value; }
        }
        private string depAirField;
        public string DepAirField
        {
            get { return depAirField; }
            set { depAirField = value; }
        }
        private string arrAirField;
        public string ArrAirField
        {
            get { return arrAirField; }
            set { arrAirField = value; }
        }
        private string aircraftReg;
        public string AircraftReg
        {
            get { return aircraftReg; }
            set { aircraftReg = value; }
        }
        private DateTime flightDate;
        public DateTime FlightDate
        {
            get { return flightDate; }
            set { flightDate = value; }
        }
        private DateTime eTD;
        public DateTime ETD
        {
            get { return eTD; }
            set { eTD = value; }
        }

        private List<string> originalOFPText;
        public List<string> OriginalOFPText
        {
            get { return originalOFPText; }
            set { originalOFPText = value; }
        }
        private List<string> trimmedOFPText;
        public List<string> TrimmedOFPText
        {
            get { return trimmedOFPText; }
            set { trimmedOFPText = value; }
        }
        private ICAOFPL icaoFPL;
        public ICAOFPL IcaoFPL
        {
            get { return icaoFPL; }
            set { icaoFPL = value; }
        }

        //End of Fields

        //Begin of Constructors
        public OFP() { }
        public OFP(string inputString)
        {
            originalOFPText = OFP.GetOriginalMassive(inputString);
            trimmedOFPText = OFP.GetTrimmedMassive(inputString);
            int ManeLineMark = 0;
            //Поиск необходимой иннформации
            for (int i = 0; i < originalOFPText.Count; i++)
            {
                List<string> words = new List<string>(); 

                string[] wordss = trimmedOFPText[i].Trim().Split(' '); //масмсив слов

                foreach (string ssss in wordss) 
                {
                    words.Add(ssss);
                }
                //вставить сюда проверку на пустую строку

                if (words[0] == "") continue;
                if (words.Count <= 5)
                {
                    for (int o = words.Count; o <= 5; o++)
                    { words.Add(""); }
                }
                //СТАРЫЙ ВАРИАНТ
                //if (words[0] == "CFP" && words[1] == "INPUT" && words[2] == "MESSAGE" && words[3] == "DATE")
                if (words[1] == "INPUT" && words[2] == "MESSAGE" && words[3] == "DATE")
                {
                    string temp = words[6];
                    int day = Convert.ToInt32(temp.Substring(0, 2));
                    int hour = Convert.ToInt32(temp.Substring(2, 2));
                    int minute = Convert.ToInt32(temp.Substring(4, 2));
                    try
                    {
                        cFPGeneratedTimeStamp = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, day, hour, minute, 0);
                        //DateTime.UtcNow.Month;
                    }
                    catch
                    {
                        cFPGeneratedTimeStamp = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month - 1, day, hour, minute, 0);
                    }
                    //NEXT FOR
                    continue;
                }
                if (words[0] == "START" && words[1] == "OF" && words[2] == "CFP" && words[3] == "REF")
                {
                    cFPREF = words[5];
                    continue;
                }
                if (words[0] == "FLT" && words[1] == "AC/REG" && words[2] == "DATE" && words[3] == "ROUTE")
                {
                    ManeLineMark = i + 2;
                }
                if (i == ManeLineMark && ManeLineMark != 0)
                {
                    flight = words[0];
                    aircraftReg = words[1];
                    flightDate = Convert.ToDateTime(words[2]);
                    depAirField = words[3];
                    arrAirField = words[4];

                    string temp = words[5];
                    int hour = Convert.ToInt32(temp.Substring(0, 2));
                    int minute = Convert.ToInt32(temp.Substring(2, 2));
                    eTD = new DateTime(flightDate.Year, flightDate.Month, flightDate.Day, hour, minute, 0);
                    if (arrAirField == "ULLI")
                    {
                        backFlight = true;
                    }
                }

                /* Поиск Маршрутных запасных
                                 ALTERNATE REQUIRED AVAILABILITY TIMES                                             
                  ALTERNATE      FROM         TO                                                   
                   ULLI          12.05        14.18                                                
                   UWGG          12.12        15.10                                                
                   USSS          13.01        15.13                                                
                                                                                   
                                                                                   
                 FUEL TO ETP BETWEEN ULLI AND USSS IS 2730  TIME 00.49 DIST  394 NM 
                 */
                if (words[0] == "ALTERNATE" && words[1] == "FROM" && words[2] == "TO")
                {
                    List<string> wordsInString = new List<string>();

                    string[] stringsInFile = trimmedOFPText[i+1].Trim().Split(' '); 
                    
                    
                    int n = 1;
                    do
                    {

                        etpList.Add((trimmedOFPText[i + n].Trim().Split(' '))[0]);
                        n++;
                    } while ((trimmedOFPText[i + n].Trim().Split(' '))[0] != "");
                    //etpList.Add("");
                    continue;
                }
            }
            icaoFPL = new ICAOFPL(trimmedOFPText);

            //End of Constructor
        }
        public string IFPS_Validate()
        {
            string result = "";
            string URI = @"http://validation.eurofpl.eu/"; 
            string Parameters = "freeEntry=%28" + icaoFPL.FPLstring.ToString() + "%29";
            //FPL-SDM6573-IS-A320/M-SDFGHIRWYZ/H-ULLI1620-K0824F350 DCT GONBI R30 KTL R22 ZJ B228 TODES R497 KUMOD/K0822F370 A310 ROTLI A300 RANET LG3E -UNKL0406 UNAA UNEE -PBN/B1D1 NAV/TCAS EQUIPPED DOF/141025 REG/VQBDR EET/ULWW0025 ULKK0052 UUYY0111 USSS0145 USTR0202 UNNT0245 UNKL0339%29";

            


            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true); 
            
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            WebProxy proxy = WebProxy.GetDefaultProxy();
            if (proxy.Address != null)
            {
                
                //Указываем системные учетные данные приложения.
                req.Credentials = System.Net.CredentialCache.DefaultCredentials;
                //Указываем сетевые учетные данные текущего контекста безопасности.
                proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                req.Proxy = proxy;
            }
            try
            {
                StringBuilder SB = new StringBuilder();
                SB.AppendLine(this.flight.ToString());
                System.IO.Stream os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
                System.Net.WebResponse resp = req.GetResponse();
                if (resp != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string htmldocs = sr.ReadToEnd();
                    sr.Close();
                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                    document.LoadHtml(htmldocs);
                    ///Список всех строк
                    HtmlNode bodyNode = document.DocumentNode.SelectSingleNode("//span[@class='ifpuv_result']");

                    foreach (var str in bodyNode.ChildNodes)
                    {
                        if (str.Name == "#text")
                        {

                            SB.AppendLine(str.InnerText);
                        }
                    }

                }
                result = SB.ToString();
            }
            catch (Exception ex)
            {
                
                result = ex.Message.ToString();
                return result;
            }
            return result;
        }
        //End of Class
    }
}

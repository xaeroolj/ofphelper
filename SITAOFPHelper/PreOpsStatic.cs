using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SITAOFPHelper
{
    public partial class OFP
    {
        //Массив необработаных строк
        static public List<string> GetOriginalMassive(string InputString)
        {
            string[] InputLines = InputString.Trim().Replace('\r', ' ').Split('\n');
            List<string> SR = new List<string>();
            foreach (string ssss in InputLines)
            {
                SR.Add(ssss);
            }
            List<string> originalFile = new List<string>();


            for (int i = 0; i < SR.Count; i++)
            {
                originalFile.Add(SR[i]);
            }
            return originalFile;
        }
        //Массив обработаных строк
        static public List<string> GetTrimmedMassive(string InputString)
        {
            string[] InputLines = InputString.Trim().Replace('\r', ' ').Split('\n');
            List<string> SR = new List<string>();
            foreach (string ssss in InputLines)
            {
                SR.Add(ssss);
            }
            List<string> trimmedFile = new List<string>();

            for (int i = 0; i < SR.Count; i++)
            {
                trimmedFile.Add(System.Text.RegularExpressions.Regex.Replace(SR[i], "\\s{2,}", " ").Trim());
            }
            return trimmedFile;
        }
        static public bool OFPinside(List<string> trimmedfile)
        {
            bool OFPStart = false;
            bool OFPEND = false;
            bool result = false;
            int FPLCOUNT = 0;
            for (int i = 0; i < trimmedfile.Count; i++)
            {
                List<string> words = new List<string>();

                string[] wordss = trimmedfile[i].Trim().Split(' ');

                //[1] = "START OF CFP REF : G7Y2A - PLK431 01 ULLI USMU"
                //[205] = "+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+"
                foreach (string ssss in wordss)
                {
                    words.Add(ssss);
                }
                if (words[0] == "START" && words[1] == "OF" && words[2] == "CFP" && words[3] == "REF")
                {
                    OFPStart = true;
                    //Console.WriteLine("OFPSTRT found! REF: {0} FLight: {1} ADEP: {2} ADES: {3}", words[5], words[7], words[9], words[10]);
                }
                if (words[0] == "+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+")
                {
                    OFPEND = true;
                    //Console.WriteLine("OFPEND found!");
                }
                if (words[0] == "END" && words[1] == "OF" && words[2] == "ICAO" && words[3] == "FLIGHT")
                {
                    ++FPLCOUNT;
                    //END OF ICAO FLIGHT PLAN
                    if (FPLCOUNT == 2)
                    {
                        OFPEND = true;
                    }
                    //Console.WriteLine("OFPSTRT found! REF: {0} FLight: {1} ADEP: {2} ADES: {3}", words[5], words[7], words[9], words[10]);
                }


            }
            if (OFPStart == true && OFPEND == true)
            {
                result = true;
            }

            return result;
        }


    }
}

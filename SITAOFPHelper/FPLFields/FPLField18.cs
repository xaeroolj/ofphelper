

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SITAOFPHelper.SmallClasses;

namespace SITAOFPHelper.FPLFields
{


    public class FPLField18
    {
        private bool segmentEnd(string inputStr)
        {
            bool result = false;

            if (inputStr.Length < 5)
                return result;
            if (inputStr.Substring(0, 4) == "REG/")
                result = true;
            if (inputStr.Substring(0, 4) == "DOF/")
                result = true;
            if (inputStr.Substring(0, 5) == "RALT/")
                result = true;
            if (inputStr.Substring(0, 5) == "ALTN/")
                result = true;
            if (inputStr.Substring(0, 5) == "DEST/")
                result = true;
            if (inputStr.Substring(0, 4) == "DEP/")
                result = true;
            if (inputStr.Substring(0, 4) == "NAV/")
                result = true;
            if (inputStr.Substring(0, 4) == "COM/")
                result = true;
            if (inputStr.Substring(0, 4) == "PER/")
                result = true;
            if (inputStr.Substring(0, 4) == "TYP/")
                result = true;
            if (inputStr.Substring(0, 4) == "STS/")
                result = true;
            if (inputStr.Substring(0, 4) == "OPR/")
                result = true;
            if (inputStr.Substring(0, 4) == "SEL/")
                result = true;
            if (inputStr.Substring(0, 4) == "RIF/")
                result = true;
            if (inputStr.Substring(0, 4) == "PBN/")
                result = true;
            if (inputStr.Substring(0, 4) == "RMK/")
                result = true;


            return result;
        }


        /* 
         * -0 IF NO OTHER INFORMATION
         * 
           EET/ULWW0030 ULKK0102 UUYY0124 USDD0213 USMM0301
         * RIF/ 
           REG/RA61706
         * SEL/
         * OPR/
         * STS/
         * TYP/
         * PER/
         * COM/
         * DAT/
         * NAV/
         * DEP/
         * DEST/
         * ALTN/
         * RALT/
           RMK/FAWT 8946/07 BELARUS SAC521/230611 TURKEY CAC.318
           DOF/110707
         */

        /*
         * FIELD type 18
         * PREVIOUSE TYPE   |   THIS TYPE   |   NEXT TYPE
         *      16                  ALR             19
         *      16                  FPL             )
         *      16                  CPL             )
         *      16                  SPL             19
         */

        private string originalText;
        public string OriginalText
        {
            get { return originalText; }
            set { originalText = value; }
        }

        private string eET;
        public string EET
        {
            get { return eET; }
            set { eET = value; }
        }

        private string rIF;
        public string RIF
        {
            get { return rIF; }
            set { rIF = value; }
        }

        private string rEG;
        public string REG
        {
            get { return rEG; }
            set { rEG = value; }
        }

        private string pBN;
        public string PBN
        {
            get { return pBN; }
            set { pBN = value; }
        }

        private string nAV;
        public string NAV
        {
            get { return nAV; }
            set { nAV = value; }
        }

        private string rMK;
        public string RMK
        {
            get { return rMK; }
            set { rMK = value; }
        }

        private string dOF;
        public string DOF
        {
            get { return dOF; }
            set { dOF = value; }
        }

        private List<PointOfFIRBound> EETLIST = new List<PointOfFIRBound>();

        public FPLField18() { }
        public FPLField18(string inputString)
        {
            List<string> wordss = new List<string>();
            string[] words = inputString.Trim().Split(' ');

            foreach (string ssss in words)
            {
                wordss.Add(ssss);
            }

            for (int i = 0; i < wordss.Count; i++)
            {
                if (words[i].Length > 0)
                {
                    //PBN
                    if (words[i].Substring(0, 4) == "PBN/")
                    {
                        pBN = words[i].Substring(4);
                        continue;
                    }

                    //NAV
                    if (words[i].Substring(0, 4) == "NAV/")
                    {
                        StringBuilder SB = new StringBuilder();
                        SB.Append(words[i].Substring(4));
                        ++i;


                        while (segmentEnd(words[i]) != true)
                        {

                            SB.Append(" ");
                            SB.Append(words[i].ToString());

                            i++;
                        }
                        --i;
                        nAV = SB.ToString();
                        continue;
                    }

                    //DOF
                    if (words[i].Substring(0, 4) == "DOF/")
                    {
                        dOF = words[i].Substring(4);
                        continue;
                    }

                    //REG
                    if (words[i].Substring(0, 4) == "REG/")
                    {
                        rEG = words[i].Substring(4);
                        continue;
                    }

                    //EET
                    if (words[i].Substring(0, 4) == "EET/")
                    {
                        PointOfFIRBound tempPOB = new PointOfFIRBound(words[i].Substring(4, 8));
                        EETLIST.Add(tempPOB);
                        words[i].Substring(0, 4);
                        ++i;
                        /*
                         * Далее 3 варианта%
                         * 1. UMMV0039
                         * 2. След Сегмент к примеру REG/EIDZH
                         * 3. Ничего
                         */
                        while (i != words.Count() && segmentEnd(words[i]) != true)
                        {

                            EETLIST.Add(PointOfFIRBound.GeneratePOB(words[i]));

                            i++;
                        }
                        --i;
                        string tempeET = "";
                        foreach (PointOfFIRBound element in EETLIST)
                        {
                            tempeET += element.ToString() + " ";
                        }
                        eET = tempeET.Substring(0, tempeET.Length - 1);
                        continue;
                    }

                    //RMK
                    if (words[i].Substring(0, 4) == "RMK/")
                    {
                        StringBuilder SB1 = new StringBuilder();
                        SB1.Append(words[i].Substring(4));
                        ++i;
                        while (i != words.Count() && segmentEnd(words[i]) != true)
                        {

                            SB1.Append(" ");
                            SB1.Append(words[i].ToString());

                            i++;
                        }
                        --i;
                        rMK = SB1.ToString();
                        continue;

                    }
                }

            }

        }
        /*
        

         for (int i = 0; i < originalOFPText.Count; i++)
            {
                List<string> words = new List<string>();

                string[] wordss = trimmedOFPText[i].Trim().Split(' ');

                foreach (string ssss in wordss)
                {
                    words.Add(ssss);
                }
                if (words[0] == "CFP" && words[1] == "INPUT" && words[2] == "MESSAGE" && words[3] == "DATE")
                {
                    string temp = words[6];
                    int day = Convert.ToInt32(temp.Substring(0, 2));
                    int hour = Convert.ToInt32(temp.Substring(2, 2));
                    int minute = Convert.ToInt32(temp.Substring(4, 2));
                    cFPGeneratedTimeStamp = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, day, hour, minute, 0);
                    //DateTime.UtcNow.Month;
                }
          
         */
    }
}

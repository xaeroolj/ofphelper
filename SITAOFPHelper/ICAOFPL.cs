using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SITAOFPHelper.FPLFields;

namespace SITAOFPHelper
{
    public class ICAOFPL
    {
        /*
            (
             FPL-SDM431-IS
            -A148/M-SDGHRYW/S
            -ULLI0025
            -K0772S1110 GR3D GR R30 METAT R822 UHT R30 MEMBA DCT
            -USMU0313 USRR USNN
            -EET/ULWW0030 ULKK0102 UUYY0124 USDD0213 USMM0301
             REG/RA61706)
        */
        /*
         * Field 3  = FPL (Message Type)
         * Field 7  = SDM431 (Aircraft ID - Flight #)
         * Field 8  = IS (Fligth Rules and Type of Flight)
         * Flight rules:    I = IFR
         *                  V = VFR
         *                  Y = IFR first
         *                  Z = VFR first
         * Type of Flight:  S = if scheduled air transport
         *                  N = if non-scheduled air transport
         *                  G = if general aviation
         *                  M = if military
         *                  X = other flights
         * Field 9  = A148/M (Type of Aircraft and Turbulence Category)
         * Turbulence Cat:  H = Heavy
         *                  M = Medium
         *                  L = Light
         * Field 10 = SDGHRYW/S (Equpment)
         * Field 13 = ULLI0025 (Dep Aerodrome and Time)
         * Field 15 = K0772S1110 GR3D GR R30 METAT R822 UHT R30 MEMBA DCT (Route)
         * Field 16 = USMU0313 USRR USNN (Dest Aerodrome and Total EET, Alternate Aerodrome's)
         * Field 18 = Other Information
         * See page 214 in DOC4444
         */

        //(Message Type)
        private string filed3;
        public string Field3
        {
            get { return filed3; }
            set { filed3 = value; }
        }
        //(Aircraft ID - Flight #)
        private string filed7;
        public string Field7
        {
            get { return filed7; }
            set { filed7 = value; }
        }
        //(Fligth Rules and Type of Flight)
        private string filed8;
        public string Field8
        {
            get { return filed8; }
            set { filed8 = value; }
        }
        //(Type of Aircraft and Turbulence Category)
        private string filed9;
        public string Field9
        {
            get { return filed9; }
            set { filed9 = value; }
        }
        //(Equpment)
        private string filed10;
        public string Field10
        {
            get { return filed10; }
            set { filed10 = value; }
        }
        //(Dep Aerodrome and Time)
        private string filed13;
        public string Field13
        {
            get { return filed13; }
            set { filed13 = value; }
        }
        //(Route)
        private string filed15;
        public string Field15
        {
            get { return filed15; }
            set { filed15 = value; }
        }
        //(Dest Aerodrome and Total EET, Alternate Aerodrome's)
        private string filed16;
        public string Field16
        {
            get { return filed16; }
            set { filed16 = value; }
        }
        //Other Information
        private string filed18;
        public string Field18
        {
            get { return filed18; }
            set { filed18 = value; }
        }
        //===============
        private FPLField18 fld18Class;
        public FPLField18 FLD18CLASS
        {
            get { return fld18Class; }
            set { fld18Class = value; }
        }
        //=========================


        private string fPLstring;
        public string FPLstring
        {
            get { return fPLstring; }
            set { fPLstring = value; }
        }
        public ICAOFPL() { }
        public ICAOFPL(List<string> trimmedfile)
        {
            int startStringIndex = 0;
            int endStringIndex = 0;
            for (int i = 0; i < trimmedfile.Count; i++)
            {
                List<string> words = new List<string>();

                string[] wordss = trimmedfile[i].Trim().Split(' ');

                foreach (string ssss in wordss)
                {
                    words.Add(ssss);
                }
                if (words[0] == "START" && words[2] == "ICAO" && words[4] == "PLAN")
                {
                    startStringIndex = i;
                }

                if (words[0] == "END" && words[2] == "ICAO" && words[4] == "PLAN")
                {
                    endStringIndex = i;
                }


            }

            StringBuilder SB = new StringBuilder();
            for (int i = startStringIndex + 2; i < endStringIndex - 1; i++)
            {
                if (trimmedfile[i].Substring(0, 1) != "-")
                {
                    SB.AppendLine(" " + trimmedfile[i]);
                }
                else SB.AppendLine(trimmedfile[i]);

            }
            SB.Remove(0, 2);
            SB.Replace("\r\n", "");

            //SB.Replace("\r", " ");
            //SB.Replace("\n", " ");
            SB.Remove(SB.ToString().Length - 1, 1);
            fPLstring = SB.ToString();
            string[] wordsss = fPLstring.Trim().Split('-');

            Field3 = wordsss[0];
            Field7 = wordsss[1];
            Field8 = wordsss[2];
            Field9 = wordsss[3];
            Field10 = wordsss[4];
            Field13 = wordsss[5];
            Field15 = wordsss[6];
            Field16 = wordsss[7];
            Field18 = wordsss[8];
            fld18Class = new FPLField18(wordsss[8]);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SITAOFPHelper.SmallClasses;

namespace SITAOFPHelper
{
    public partial class OFP
    {

        public List<string> MakePreBrief()
        //public List<string> MakePreBrief(List<string> originalfile, List<string> trimmedfile)
        {
            PreBriefClass ShortOFP = new PreBriefClass();
            ShortOFP.FlightNbr = this.Flight;
            List<string> trimmedfile = this.TrimmedOFPText;
            List<string> originalfile = this.OriginalOFPText;
            List<string> Output = new List<string>();
            //[7] = " FLT AC/REG DATE ROUTE ETD ALTN WX OBS TIME/PROGS"
            //[22] = "ATC CLRNC:.. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .."
            //[34] = "AWY TAS G/S LAT FL WYPNT MGN DTGO TRT T/T RMF ETO"
            //[39] = "AWY TAS G/S LAT FL WPT MGN DTG TRT ELT RMF ETO"
            //[110] = "START OF ICAO FLIGHT PLAN"
            //[179] = "ECONOMICS OF CARRYING EXTRA FUEL"
            //TANKERING ANALYSIS
            //" TANKERING - NO"
            //TANKERING - NO
            //ALTERNATE REQUIRED AVAILABILITY TIMES
            //[41] = "----- ---- --- ---- --- --- ----- ---- --- --- --- ---"
            //DIST/T   624   AIR/D   647   AVG FF  2425  AVG W/C M14
            //[178] = "ALTERNATE FROM TO"
            //[183] = "FUEL TO ETP BETWEEN ULLI AND UUDD IS 2160 TIME 00.31 DIST 191 NM"
            int ETPENDIndex = -1;
            int startindex = trimmedfile.IndexOf("FLT AC/REG DATE ROUTE ETD ALTN WX OBS TIME/PROGS") - 2;
            int EndAltindex = -1;
            int DistanceIndex = -1;
            for (int i = 0; i < trimmedfile.Count; i++)
            {
                List<string> words = new List<string>();

                string[] wordss = trimmedfile[i].Trim().Split(' ');

                foreach (string ssss in wordss)
                {
                    words.Add(ssss);
                }
                if (words[0] == "TAKE-OFF")
                {//TAKE-OFF
                    EndAltindex = i;
                }
                if (words[0] == "DIST/T" && words[2] == "AIR/D" && words[4] == "AVG")
                {
                    DistanceIndex = i;
                }
                if (words[0] == "FUEL" && words[1] == "TO" && words[2] == "ETP" && words[3] == "BETWEEN")
                {
                    ETPENDIndex = i;
                }

            }
            int AlternateIndex = trimmedfile.IndexOf("ALTERNATE FROM TO");
            int ExtraFuelindex = trimmedfile.IndexOf("ECONOMICS OF CARRYING EXTRA FUEL");
            int TankeringNOmarker = trimmedfile.IndexOf("TANKERING - NO");
            bool spaceinprewline = false;
            if (startindex != -1)
            {
                for (int i = startindex; i < EndAltindex - 1; i++)
                {
                    if (originalfile[i] == "  " || originalfile[i] == " ")
                    {
                        if (spaceinprewline == true)
                        {

                        }
                        else
                        {
                            spaceinprewline = false;
                            Output.Add(originalfile[i]);
                            ShortOFP.MainPart.Add(originalfile[i]);
                        }
                        spaceinprewline = true;
                    }
                    else
                    {
                        spaceinprewline = false;
                        Output.Add(originalfile[i]);
                        ShortOFP.MainPart.Add(originalfile[i]);
                    }
                }
            }

            /*
             if (EndAltindex != -1)
             {
                 spaceinprewline = false;
                 for (int i = EndAltindex + 1; i < EndAltindex; i++)
                 {
                     if (originalfile[i] == "  " || originalfile[i] == " ")
                     {
                         if (spaceinprewline == true)
                         {

                         }
                         else
                         {
                             spaceinprewline = false;
                             System.Console.WriteLine(originalfile[i]);
                             Output.Add(originalfile[i]);
                         }
                         spaceinprewline = true;
                     }
                     else
                     {
                         spaceinprewline = false;
                         System.Console.WriteLine(originalfile[i]);
                         Output.Add(originalfile[i]);
                     }

                    

                 }
             }
              */
            if (DistanceIndex != -1)
            {
                Output.Add(" ");
                Output.Add(originalfile[DistanceIndex]);
                Output.Add(" ");
                ShortOFP.MainPart.Add(" ");
                ShortOFP.MainPart.Add(originalfile[DistanceIndex]);
                ShortOFP.MainPart.Add(" ");
            }

            //ShortOFP.MainPart = Output;

            if (AlternateIndex != -1)
            {
                for (int i = AlternateIndex - 1; i <= ETPENDIndex - 1; i++)
                {
                    if (originalfile[i] == "  " || originalfile[i] == " ")
                    {
                        if (spaceinprewline == true)
                        {

                        }
                        else
                        {
                            spaceinprewline = false;
                            ShortOFP.EtopsPart.Add(originalfile[i]);
                            Output.Add(originalfile[i]);
                        }
                        spaceinprewline = true;
                    }
                    else
                    {
                        spaceinprewline = false;
                        ShortOFP.EtopsPart.Add(originalfile[i]);
                        Output.Add(originalfile[i]);
                    }
                    //System.Console.WriteLine(originalfile[i]);
                    //Output.Add(originalfile[i]);
                }
            }

            if (ExtraFuelindex != -1)
            {
                for (int i = ExtraFuelindex; i < ExtraFuelindex + 7; i++)
                {
                    if (originalfile[i] == "  " || originalfile[i] == " ")
                    {
                        if (spaceinprewline == true)
                        {

                        }
                        else
                        {
                            spaceinprewline = false;
                            ShortOFP.TnkPart.Add(originalfile[i]);
                            Output.Add(originalfile[i]);
                        }
                        spaceinprewline = true;
                    }
                    else
                    {
                        spaceinprewline = false;
                        ShortOFP.TnkPart.Add(originalfile[i]);
                        Output.Add(originalfile[i]);
                    }
                    //System.Console.WriteLine(originalfile[i]);
                    //Output.Add(originalfile[i]);
                }
            }


            if (TankeringNOmarker != -1)
            {
                for (int i = TankeringNOmarker - 2; i <= TankeringNOmarker; i++)
                {
                    if (originalfile[i] == "  " || originalfile[i] == " ")
                    {
                        if (spaceinprewline == true)
                        {

                        }
                        else
                        {
                            spaceinprewline = false;
                            ShortOFP.TnkPart.Add(originalfile[i]);
                            Output.Add(originalfile[i]);
                        }
                        spaceinprewline = true;
                    }
                    else
                    {
                        spaceinprewline = false;
                        ShortOFP.TnkPart.Add(originalfile[i]);
                        Output.Add(originalfile[i]);
                    }
                    //System.Console.WriteLine(originalfile[i]);
                    //Output.Add(originalfile[i]);
                }
            }

            return Output;
        }

    }
}

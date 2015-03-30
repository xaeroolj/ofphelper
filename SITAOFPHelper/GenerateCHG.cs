using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SITAOFPHelper
{
    partial class OFP
    {
        public string generateCHG(char mode = '9', bool REG_SWITCH = true)
        {
            /*
            SB.Append("(CHG-");
            SB.Append(this.icaoFPL.Field7);
            SB.Append("-");
            SB.Append(this.icaoFPL.Field13);
            SB.Append("-");
            SB.Append(this.icaoFPL.Field16.Remove(4));
            SB.AppendLine("-9/");
            SB.Append(this.icaoFPL.Field9);
            SB.Append("-10/");
            SB.Append(this.icaoFPL.Field10);
            SB.AppendLine("-15/");
            SB.Append(this.icaoFPL.Field15);
            SB.AppendLine("-16/");
            SB.Append(this.icaoFPL.Field16);
            SB.AppendLine("-18/");
            SB.Append(this.icaoFPL.Field18);
            SB.Append(")");
             */
            string CHGRESULT = null;
            StringBuilder SB = new StringBuilder();
            String icaodate;
            if (this.icaoFPL.FLD18CLASS.DOF != null)
                icaodate = this.icaoFPL.FLD18CLASS.DOF;
            else
                icaodate = String.Format("{0:yyMMdd}", this.flightDate);
            /* mode=
             * 1 for Eurocontrol MODE
             * 2 for Russiya MODE
             * 
            */

            StringBuilder fld18_string = new StringBuilder();
            if (REG_SWITCH == false)
            {

                fld18_string.Append(this.icaoFPL.Field18);
                int regstart = fld18_string.ToString().IndexOf("REG/") - 1;
                int regend = fld18_string.Length;
                fld18_string.Remove(regstart, regend - regstart);
            }
            else
            {
                fld18_string.Append(this.icaoFPL.Field18);
            }


            switch (mode)
            {
                case '1':

                    SB.AppendLine("(CHG-" + this.icaoFPL.Field7 + "-" + this.icaoFPL.Field13 + "-" + this.icaoFPL.Field16.Remove(4) + "-DOF/" + icaodate);
                    SB.AppendLine("-9/" + this.icaoFPL.Field9 + "-10/" + this.icaoFPL.Field10);
                    SB.AppendLine("-15/" + this.icaoFPL.Field15);
                    SB.AppendLine("-16/" + this.icaoFPL.Field16);
                    SB.AppendLine("-18/" + fld18_string + ")");
                    //SB.AppendLine("-18/" + this.icaoFPL.Field18 + ")");

                    break;
                case '2':

                    SB.AppendLine("(CHG-" + this.icaoFPL.Field7 + "-" + this.icaoFPL.Field13 + "-" + this.icaoFPL.Field16.Remove(4));
                    SB.AppendLine("-9/" + this.icaoFPL.Field9 + "-10/" + this.icaoFPL.Field10);
                    SB.AppendLine("-15/" + this.icaoFPL.Field15);
                    SB.AppendLine("-16/" + this.icaoFPL.Field16);
                    SB.AppendLine("-18/" + fld18_string + ")DOF/" + icaodate);
                    //SB.AppendLine("-18/" + this.icaoFPL.Field18 + ")DOF/" + icaodate);

                    break;
                default:
                    SB.AppendLine("(CHG-" + this.icaoFPL.Field7 + "-" + this.icaoFPL.Field13 + "-" + this.icaoFPL.Field16.Remove(4));
                    SB.AppendLine("-9/" + this.icaoFPL.Field9 + "-10/" + this.icaoFPL.Field10);
                    SB.AppendLine("-15/" + this.icaoFPL.Field15);
                    SB.AppendLine("-16/" + this.icaoFPL.Field16);
                    SB.AppendLine("-18/" + fld18_string + ")");
                    //SB.AppendLine("-18/" + this.icaoFPL.Field18 + ")");

                    break;

            }



            CHGRESULT = SB.ToString();
            return CHGRESULT;
        }

    }
}

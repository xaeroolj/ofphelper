using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows;
using System.Net;

namespace PreBriefMini
{
    class SQLconnect
    {

        static public void sendCHG(string FLight, string localIP)
        {
            string connstring = "Data Source=xaero_11.hosting.parking.ru;Initial Catalog=xaero_11;User ID=xaero_11;Password={A33SX)jC0";
            //string connstring = "server=xaero_8.hosting.parking.ru:uid=xaero_8:pwd=Hjvf1987:database=CHGLOG";
            SqlDataAdapter conn = new SqlDataAdapter();
            
            SqlConnection con = new SqlConnection(connstring);
            String dateTImeNow = String.Format("{0:MM.dd.yyyy hh:mm:ss}", DateTime.Now);
            string cmdstring = "INSERT INTO CHGLOG (DateTime, Flight, LocalIP) VALUES ('" + dateTImeNow + "','" + FLight + "','" + localIP +"')";
            
            //string cmdstring = "INSERT INTO CHGLOG (DateTime, Flight) VALUES ('09.15.2005 14:15:00','SDM431')";
            //"INSERT INTO CHGLOG (DateTime, Flight) VALUES ('09:15:2011 10:04:9','SDM431')"
            //"INSERT INTO CHGLOG (DateTime, Flight) VALUES ('15.09.2011 21:47:08', 'SDM431')"
            //"INSERT INTO CHGLOG (DateTime, Flight) VALUES ('15.09.2011 21:48:53','SDM431')"
            //"INSERT INTO CHGLOG (DateTime, Flight) VALUES ('10.11.2005 14:15:00','SDM123')"
            SqlCommand cmd = new SqlCommand(cmdstring, con);
   

            

            try
            {
                
                con.Open();
                //AsyncCallback callback = new AsyncCallback(HandleCallback);
                //cmd.BeginExecuteNonQuery(callback,cmd);
                
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                StringBuilder sb =new StringBuilder();
                sb.AppendLine("Error Source:" );
                sb.AppendLine(ex.Source);
                sb.AppendLine("Message: ");
                sb.AppendLine(ex.Message);
                sb.AppendLine("StackTrace: ");
                sb.AppendLine(ex.StackTrace.ToString());
                sb.AppendLine("TargetSite: ");
                sb.AppendLine(ex.TargetSite.ToString());

                               
                MessageBox.Show(sb.ToString());
                //MessageBox.Show("Conn problem");
            }
            finally
            {
                con.Close();
            }

        }

        private void HandleCallback(IAsyncResult result)
        {
            try
            {
                // You may not interact with the form and its contents
                // from a different thread, and this callback procedure
                // is all but guaranteed to be running from a different thread
                // than the form. Therefore you cannot simply call code that 
                // displays the results, like this:
                // DisplayResults(rowText)

                // Instead, you must call the procedure from the form's thread.
                // One simple way to accomplish this is to call the Invoke
                // method of the form, which calls the delegate you supply
                // from the form's thread. 
                

            }
            catch (Exception ex)
            {
                // Because you are now running code in a separate thread, 
                // if you do not handle the exception here, none of your other
                // code catches the exception. Because none of 
                // your code is on the call stack in this thread, there is nothing
                // higher up the stack to catch the exception if you do not 
                // handle it here. You can either log the exception or 
                // invoke a delegate (as in the non-error case in this 
                // example) to display the error on the form. In no case
                // can you simply display the error without executing a delegate
                // as in the try block here. 

                // You can create the delegate instance as you 
                // invoke it, like this:
                }
            finally
            {
               
            }
        }
    }
}

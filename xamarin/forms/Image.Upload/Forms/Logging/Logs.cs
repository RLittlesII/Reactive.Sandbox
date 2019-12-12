using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Logging
{
    public class Logs : ILogs
    {
        public void Log(Exception ex)
        {
            //Do whatever you want here.
            //For demo purposes we will just write out the message to the Output window
            Debug.Write(ex.Message);
        }
    }
}

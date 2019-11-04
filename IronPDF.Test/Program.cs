using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPDF.Test
{
   class Program
   {
      static void Main(string[] args)
      {
         var command = @"lpr -S localhost -P ""DAILY"" ""C:\Test.pdf""";
         ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/C " + command);
         procStartInfo.RedirectStandardOutput = true;
         procStartInfo.UseShellExecute = false;
         procStartInfo.RedirectStandardError = true;
         procStartInfo.CreateNoWindow = true;

         // start process
         Process proc = new Process();
         proc.StartInfo = procStartInfo;
         proc.Start();

         proc.WaitForExit();
      }
   }
}

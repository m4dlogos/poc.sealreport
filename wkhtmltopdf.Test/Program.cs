using Codaxy.WkHtmlToPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wkhtmltopdf.Test
{
   class Program
   {
      static void Main(string[] args)
      {
         string file = @"C:\Users\as\source\git\poc.sealreport\wkhtmltopdf.Test\bin\Debug\Chart.js_demo.html";
         PdfConvert.ConvertHtmlToPdf(new PdfDocument
         {
            Url = file,
            HeaderLeft = "[title]",
            HeaderRight = "[date] [time]",
            FooterCenter = "Page [page] of [topage]",
            ExtraParams = new Dictionary<string, string> { { "javascript-delay", "5000" } }

         }, new PdfOutput
         {
            OutputFilePath = "Chart.js_demo.pdf"
         });
      }
   }
}

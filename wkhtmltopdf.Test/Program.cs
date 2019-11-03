using Codaxy.WkHtmlToPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wkhtmltopdf.Test
{
   class Program
   {
      static void Main(string[] args)
      {
			Console.WriteLine("Dossier de travail:");
			string workingDir = Console.ReadLine();
			if (string.IsNullOrEmpty(workingDir))
			{
				workingDir = Directory.GetParent("../../").FullName;
				Console.WriteLine(workingDir + "\n");
			}

			Console.WriteLine("Fichier à convertir:");
			string fileToConvert = Console.ReadLine();
			string filePath = Path.Combine(workingDir, fileToConvert);

			string fileName = Path.GetFileNameWithoutExtension(filePath);
			string fileDist = Path.Combine(Path.GetDirectoryName(filePath), fileName + ".pdf");

			var WkHtmlToPdfPath = Path.Combine(Directory.GetParent("../../../").FullName, "wkhtmltox/bin/wkhtmltopdf.exe");


			PdfConvert.ConvertHtmlToPdf(new PdfDocument
         {
            Url = filePath,
            HeaderLeft = "[title]",
            HeaderRight = "[date] [time]",
            FooterCenter = "Page [page] of [topage]",
            ExtraParams = new Dictionary<string, string> { { "javascript-delay", "5000" } }

         }
			, new PdfConvertEnvironment { WkHtmlToPdfPath = WkHtmlToPdfPath, TempFolderPath = Path.GetTempPath(), Timeout = 60000 }
			, new PdfOutput
         {
            OutputFilePath = fileDist
			});

			Console.WriteLine("Fin de la conversion: " + fileDist);
			Console.ReadLine();
      }
   }
}

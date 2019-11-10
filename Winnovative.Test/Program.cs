using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winnovative.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create a HTML to PDF converter object with default settings
			HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

			// Set license key received after purchase to use the converter in licensed mode
			// Leave it not set to use the converter in demo mode
			htmlToPdfConverter.LicenseKey = "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

			// Set HTML Viewer width in pixels which is the equivalent in converter of the browser window width
			htmlToPdfConverter.HtmlViewerWidth = 1024;

			// Set PDF page size which can be a predefined size like A4 or a custom size in points 
			// Leave it not set to have a default A4 PDF page
			htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;

			// Set PDF page orientation to Portrait or Landscape
			// Leave it not set to have a default Portrait orientation for PDF page
			htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

			// Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
			// Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
			htmlToPdfConverter.ConversionDelay = 2;

			// Convert HTML to PDF using the settings above
			string outPdfFile = @"test_convert_clyd_report.pdf";

			try
			{
				string url = @"file://C:/users/famille/source/git/poc.sealreport/Winnovative.Test/bin/Debug/Clyd_Report4.htm";

				// Convert the HTML page given by an URL to a PDF document in a memory buffer
				byte[] outPdfBuffer = htmlToPdfConverter.ConvertUrl(url);

				// Write the memory buffer in a PDF file
				System.IO.File.WriteAllBytes(outPdfFile, outPdfBuffer);

				System.Diagnostics.Process.Start(outPdfFile);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

		}
	}
}

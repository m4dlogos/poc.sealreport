using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSharp.Test
{
   class Program
   {
      static void Main(string[] args)
      {
         // Create a new PDF document
         PdfDocument document = new PdfDocument();
         document.Info.Title = "Created with PDFsharp";

         // Create an empty page
         PdfPage page = document.AddPage();

         // Get an XGraphics object for drawing
         XGraphics gfx = XGraphics.FromPdfPage(page);

         // Create a font
         XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

         // Draw the text
         gfx.DrawString("Hello, World!", font, XBrushes.Black,
         new XRect(0, 0, page.Width, page.Height),
         XStringFormats.Center);
         // Save the document...

         const string filename = "HelloWorld.pdf";
         document.Save(filename);

         // ...and start a viewer.
         Process.Start(filename);
      }
   }
}

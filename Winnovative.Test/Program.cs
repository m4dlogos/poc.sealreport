using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
         var htmlToPdfConverter = GetDefaultHtmlToPdfConverter();
         SetDefaultDocumentOptions(htmlToPdfConverter);
         SetDefaultSecurityOptions(htmlToPdfConverter);
         SetDefaultHeaderOptions(htmlToPdfConverter);
         SetDefaultFooterOptions(htmlToPdfConverter);
         SetDefaultDocumentInfo(htmlToPdfConverter);
         SetDefaultBookmarkOptions(htmlToPdfConverter);
         SetDefaultViewerPreferences(htmlToPdfConverter);
         AddConverterScript(htmlToPdfConverter);


			htmlToPdfConverter.PdfDocumentOptions.PageBreakAfterHtmlElementsSelectors = new string[] { "#page_break_model_after" };

			// Convert HTML to PDF using the settings above
			string outPdfFile = @"test_convert_clyd_report.pdf";

			try
			{
				string htmlString = File.ReadAllText("Clyd_Report.htm", Encoding.UTF8);
				string baseUrl = "";


				// Convert the HTML page given by an URL to a PDF document in a memory buffer
				byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(htmlString, baseUrl);

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

      static void AddConverterScript(HtmlToPdfConverter converter)
      {
         // Header
         converter.PdfDocumentOptions.ShowHeader = true;
         var logoPath = @"C:\Users\as\source\git\poc.sealreport\Repository\Views\Images\logo.png";
         var logoImg = Image.FromFile(logoPath);

         // Add logo image
         if (System.IO.File.Exists(logoPath))
         {
            float x = (converter.PdfDocumentOptions.PdfPageSize.Width - (float)logoImg.Width) / (float)2.0;
            ImageElement headerImage = new ImageElement(x, 0, logoPath);
            converter.PdfHeaderOptions.AddElement(headerImage);
         }
         else
         {
            TextElement logoText = new TextElement(0, 0, "Telelogos", new Font(new System.Drawing.FontFamily("Arial"), 12, FontStyle.Italic, GraphicsUnit.Point));
            logoText.TextAlign = HorizontalTextAlign.Left;
            logoText.EmbedSysFont = true;
            converter.PdfHeaderOptions.AddElement(logoText);
         }

         // Add the date of the report
         string strDateZone = string.Format("{0:yyyy-MM-dd HH:mm:ss} GMT{1}", converter.PdfDocumentInfo.CreatedDate.ToLocalTime(), converter.PdfDocumentInfo.CreatedDate.ToLocalTime().ToString("%K"));
         string txtReportDate = $"Rapport ({strDateZone})";
         TextElement headerText = new TextElement(0, logoImg.Height, txtReportDate, new Font(new System.Drawing.FontFamily("Arial"), 10, FontStyle.Regular, GraphicsUnit.Point));
         headerText.TextAlign = HorizontalTextAlign.Center;
         headerText.EmbedSysFont = true;
         converter.PdfHeaderOptions.AddElement(headerText);

         var headerTitle = new TextElement(0, logoImg.Height + 10, "JUSTIFICATIF DU RAPPORT DE PERFORMANCES", new Font(new System.Drawing.FontFamily("Arial"), 16, FontStyle.Bold, GraphicsUnit.Point));
         headerTitle.TextAlign = HorizontalTextAlign.Center;
         headerTitle.EmbedSysFont = true;
         converter.PdfHeaderOptions.AddElement(headerTitle);

         float documentWidth = converter.PdfDocumentOptions.PdfPageSize.Width - converter.PdfDocumentOptions.LeftMargin - converter.PdfDocumentOptions.RightMargin;
         if (converter.PdfDocumentOptions.PdfPageOrientation == PdfPageOrientation.Landscape)
         {
            documentWidth = converter.PdfDocumentOptions.PdfPageSize.Height - converter.PdfDocumentOptions.LeftMargin - converter.PdfDocumentOptions.RightMargin;
         }

         converter.PdfHeaderOptions.HeaderHeight = 2 * logoImg.Height + 10;

         // Header line 
         LineElement headerLine = new LineElement(0, converter.PdfHeaderOptions.HeaderHeight - 4, documentWidth, converter.PdfHeaderOptions.HeaderHeight - 4);
         headerLine.ForeColor = Color.Gray;
         converter.PdfHeaderOptions.AddElement(headerLine);

         // Footer 
         converter.PdfDocumentOptions.ShowFooter = true;
         TextElement footerText = new TextElement(0, 6, "Page &p; of &P;", new Font(new System.Drawing.FontFamily("Arial"), 9, GraphicsUnit.Point));
         footerText.TextAlign = HorizontalTextAlign.Right;
         footerText.ForeColor = Color.Gray;
         footerText.EmbedSysFont = true;
         converter.PdfFooterOptions.AddElement(footerText);

         TextElement footerText2 = new TextElement(0, 6, DateTime.Now.ToString("d") + " " + DateTime.Now.ToString("t"), new Font(new FontFamily("Arial"), 9, GraphicsUnit.Point));
         footerText2.TextAlign = HorizontalTextAlign.Left;
         footerText2.ForeColor = Color.Gray;
         footerText2.EmbedSysFont = true;
         converter.PdfFooterOptions.AddElement(footerText2);

         // Footer line 
         LineElement footerLine = new LineElement(0, 4, documentWidth, 4);
         footerLine.ForeColor = Color.Gray;
         converter.PdfFooterOptions.AddElement(footerLine);
      }

      // Get the pdf converter configured with the seal report parameters of the seal designer
      static HtmlToPdfConverter GetDefaultHtmlToPdfConverter()
      {
         var htmlToPdfConverter = new HtmlToPdfConverter
         {
            ClipHtmlView = false,
            ConversionDelay = 2,
            HiddenHtmlElementsSelectors = null,
            HtmlViewerHeight = 0,
            HtmlViewerWidth = 1024,
            HtmlViewerZoom = 100,
            ImagePartSize = 32000,
            InterruptSlowJavaScript = false,
            JavaScriptEnabled = true,
            MaxHtmlViewerHeight = 0,
            MinHtmlViewerHeight = 0,
            InitialHtmlViewerHeight = 0,
            MediaType = null,
            NavigationTimeout = 60,
            PersistentHttpRequestHeaders = true,
            PostScriptFontsEnabled = true,
            PrerenderEnabled = true,
            RenderedHtmlElementSelector = null,
            SvgFontsEnabled = false,
            TriggeringMode = TriggeringMode.ConversionDelay,
            WebFontsEnabled = true,
            Enable3DTransformations = false,
            EnableWebGL = false,
            EnableAccelerated2DCanvas = false,
            EnablePersistentStorage = false,
            LocalFilesAccessEnabled = true,
            ExtensionsEnabled = false,
            DownloadAllResources = false,
            DefaultHtmlEncoding = null,
            ManualTriggeringConversionDelay = 2,
            XPSupplementalLanguages = false,
            LicenseKey = "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og="
         };

         return htmlToPdfConverter;
      }

      // Set the same document options than in seal report designer
      static void SetDefaultDocumentOptions(HtmlToPdfConverter converter)
      {
         converter.PdfDocumentOptions.AutoCloseExternalDocs = true;
         converter.PdfDocumentOptions.LeftMargin = 10;
         converter.PdfDocumentOptions.TopMargin = 10;
         converter.PdfDocumentOptions.RightMargin = 10;
         converter.PdfDocumentOptions.BottomMargin = 10;
         converter.PdfDocumentOptions.X = 0;
         converter.PdfDocumentOptions.Y = 0;
         converter.PdfDocumentOptions.Width = -1;
         converter.PdfDocumentOptions.Height = -1;
         converter.PdfDocumentOptions.TopSpacing = 0;
         converter.PdfDocumentOptions.BottomSpacing = 0;
         converter.PdfDocumentOptions.BackColor = System.Drawing.Color.White;
         converter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
         converter.PdfDocumentOptions.JpegCompressionEnabled = true;
         converter.PdfDocumentOptions.JpegCompressionLevel = 10;
         converter.PdfDocumentOptions.CompressCrossReference = false;
         converter.PdfDocumentOptions.PdfPageSize.Width = 595;
         converter.PdfDocumentOptions.PdfPageSize.Height = 842;
         converter.PdfDocumentOptions.PdfStandardSubset = PdfStandardSubset.Full;
         converter.PdfDocumentOptions.ColorSpace = ColorSpace.RGB;
         converter.PdfDocumentOptions.ColorProfile = ColorProfile.Custom;
         converter.PdfDocumentOptions.FitWidth = true;
         converter.PdfDocumentOptions.FitHeight = false;
         converter.PdfDocumentOptions.StretchToFit = false;
         converter.PdfDocumentOptions.SinglePage = false;
         converter.PdfDocumentOptions.AutoSizePdfPage = true;
         converter.PdfDocumentOptions.AvoidTextBreak = true;
         converter.PdfDocumentOptions.AvoidImageBreak = false;
         converter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
         converter.PdfDocumentOptions.ShowHeader = false;
         converter.PdfDocumentOptions.ShowFooter = false;
         converter.PdfDocumentOptions.EmbedFonts = true;
         converter.PdfDocumentOptions.LiveUrlsEnabled = true;
         converter.PdfDocumentOptions.InteractiveHiddenElements = false;
         converter.PdfDocumentOptions.InternalLinksEnabled = true;
         converter.PdfDocumentOptions.TiledRenderingEnabled = true;
         converter.PdfDocumentOptions.EnhancedGraphicsQuality = true;
         converter.PdfDocumentOptions.TransparentImagesEnabled = true;
         converter.PdfDocumentOptions.ImagesScalingEnabled = false;
         converter.PdfDocumentOptions.NoSizeElementsEnabled = false;
         converter.PdfDocumentOptions.TransparencyEnabled = true;
         converter.PdfDocumentOptions.TableHeaderRepeatEnabled = true;
         converter.PdfDocumentOptions.TableFooterRepeatEnabled = true;
         converter.PdfDocumentOptions.StackRepeatedTableHeaders = true;
         converter.PdfDocumentOptions.StackRepeatedTableFooters = true;
      }

      static void SetDefaultSecurityOptions(HtmlToPdfConverter converter)
      {
         converter.PdfSecurityOptions.CanAssembleDocument = true;
         converter.PdfSecurityOptions.CanCopyContent = true;
         converter.PdfSecurityOptions.CanCopyAccessibilityContent = true;
         converter.PdfSecurityOptions.CanEditAnnotations = true;
         converter.PdfSecurityOptions.CanEditContent = true;
         converter.PdfSecurityOptions.CanFillFormFields = true;
         converter.PdfSecurityOptions.CanPrint = true;
         converter.PdfSecurityOptions.CanPrintHighResolution = true;
         converter.PdfSecurityOptions.KeySize = EncryptionKeySize.EncryptKey128Bit;
         converter.PdfSecurityOptions.EncryptionAlgorithm = EncryptionAlgorithm.RC4;
         converter.PdfSecurityOptions.OwnerPassword = string.Empty;
         converter.PdfSecurityOptions.UserPassword = string.Empty;
      }

      static void SetDefaultHeaderOptions(HtmlToPdfConverter converter)
      {
         converter.PdfHeaderOptions.HeaderBackColor = System.Drawing.Color.White;
         converter.PdfHeaderOptions.HeaderHeight = 40;
         converter.PdfHeaderOptions.PageNumberingStartIndex = 0;
         converter.PdfHeaderOptions.PageNumberingPageCountIncrement = 0;
      }

      static void SetDefaultFooterOptions(HtmlToPdfConverter converter)
      {
         converter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;
         converter.PdfFooterOptions.FooterHeight = 20;
         converter.PdfFooterOptions.PageNumberingStartIndex = 0;
         converter.PdfFooterOptions.PageNumberingPageCountIncrement = 0;
      }

      static void SetDefaultDocumentInfo(HtmlToPdfConverter converter)
      {
         converter.PdfDocumentInfo.AuthorName = "Telelogos";
         converter.PdfDocumentInfo.CreatedDate = DateTime.Now;
         converter.PdfDocumentInfo.Keywords = string.Empty;
         converter.PdfDocumentInfo.Subject = string.Empty;
         converter.PdfDocumentInfo.Title = "Rapport du matin";
      }

      static void SetDefaultBookmarkOptions(HtmlToPdfConverter converter)
      {
         converter.PdfBookmarkOptions.AutoBookmarksEnabled = false;
         converter.PdfBookmarkOptions.HierarchicalBookmarks = true;
         converter.PdfBookmarkOptions.AllowDefaultTitle = true;
         converter.PdfBookmarkOptions.DefaultTitle = "Bookmark";
         converter.PdfBookmarkOptions.MaxTitleLength = -1;
      }

      static void SetDefaultViewerPreferences(HtmlToPdfConverter converter)
      {
         converter.PdfViewerPreferences.HideToolbar = false;
         converter.PdfViewerPreferences.HideMenuBar = false;
         converter.PdfViewerPreferences.HideWindowUI = false;
         converter.PdfViewerPreferences.FitWindow = false;
         converter.PdfViewerPreferences.CenterWindow = false;
         converter.PdfViewerPreferences.DisplayDocTitle = false;
         converter.PdfViewerPreferences.NonFullScreenPageMode = ViewerFullScreenExitMode.UseNone;
         converter.PdfViewerPreferences.Direction = ViewerTextOrder.L2R;
         converter.PdfViewerPreferences.PageMode = ViewerPageMode.UseNone;
         converter.PdfViewerPreferences.PageLayout = ViewerPageLayout.OneColumn;
      }
   }
}

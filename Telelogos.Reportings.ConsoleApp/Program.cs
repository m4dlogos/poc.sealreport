using System;
using System.Diagnostics;
using System.IO;

namespace Telelogos.Reportings.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
		    // Avec la librairie Telelogos.Reportings
		      Settings.RepositoryPath = Path.Combine(Path.GetFullPath(@"..\..\..\"), "Repository"); // remonter au dossier de la solution
            var builder = new DashboardReportBuilder();
            var director = new DashboardReportBuilderDirector();
            var data = new DashboardStatistics();
            Helper.ShallowCopyValues(DATA_Statistics, data);
            director.BuildReport(builder, data);

			   Console.WriteLine("Format ? (html/pdf): ");
			   var format = Console.ReadLine();
            var reportFile = builder.GenerateReport(format == "html" ? ReportFormat.html : ReportFormat.pdf);

				var destFile = Path.Combine(Directory.GetParent("../../").FullName, Path.GetFileName(reportFile));
				File.Copy(reportFile, destFile, true);

            //Helper.SendEMail(destFile);

				// Show the report
				Process.Start(destFile);

			   Console.WriteLine("Génération terminée !");
			   Console.ReadLine();
        }

        static DashboardStatistics DATA_Statistics = new DashboardStatistics
        {
            PlayersOkCount = 0,
            PlayersUnreachableCount = 1,
            PlayersActivCount = 1,
            PlayersUpToDateCount = 0,
            PlayersNotUpToDateCount = 1,
            PlayersConformCount = 1,
            PlayersNotConformCount = 0,
            PlayerIsInitialized = 2,
            PlayerLicencesCount = -1
        };

        //private static string getPdfHeaderConfiguration()
        //{
        //   var configurationPath = Path.GetDirectoryName(Repository.Instance.ConfigurationPath);
        //   return File.ReadAllText(Path.Combine(configurationPath, "PdfHeaderOptions.xml"));
        //}

        static void sendEmail(string file)
        {
         try
         {
            //var device = OutputEmailDevice.Create();
            //device.Server = "smtp3";
            //device.Port = 25;
            //device.UserName = "";
            //device.Password = "";
            //device.SenderEmail = "aseguin@telelogos.com";

            //var from = "aseguin@telelogos.com";
            //var to = "aseguin@telelogos.com";
            //MailMessage message = new MailMessage(from, to);
            //message.From = new MailAddress(Helper.IfNullOrEmpty(from, device.SenderEmail));
            //device.AddEmailAddresses(message.To, to);
            //message.Subject = "M4D - Rapport de conformité du " + DateTime.Now.ToLongDateString();
            //message.Body = "Test d'envoie du rapport de conformité";
            //message.Attachments.Insert(0, new Attachment(file));

            //device.SmtpClient.Send(message);
            //MailMessage message = new MailMessage(from, to);
            //message.Subject = "M4D - Rapport de conformité du " + DateTime.Now.ToLongDateString();
            //message.Body = "Test d'envoie du rapport de conformité";
            //var smtp = new SmtpClient(device.Server, device.Port);
            //smtp.Send(message);
         }
         catch (Exception emailEx)
         {
            //Helper.WriteLogEntryScheduler(EventLogEntryType.Error, "Error got trying sending notification email.\r\n{0}", emailEx.Message);
         }
      }
    }
}

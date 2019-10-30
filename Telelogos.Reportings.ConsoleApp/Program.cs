using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telelogos.Reportings.ConsoleApp
{
   class Program
   {
      static void Main(string[] args)
      {
         // Avec la librairie Telelogos.Reportings
         Telelogos.Reportings.Settings.RepositoryPath = @"C:\Users\as\Documents\Etudes\SealReport\Seal-Report-5.0\Projects\Repository";
         var builder = new Telelogos.Reportings.DashboardReportBuilder();
         var director = new Telelogos.Reportings.DashboardReportBuilderDirector();
         var data = new Telelogos.Reportings.DashboardStatistics();
         Telelogos.Reportings.Helper.ShallowCopyValues(DATA_Statistics, data);
         director.BuildReport(builder, data);
         var reportFile = builder.GenerateReport();

         // Show the report
         Process.Start(reportFile);
      }

      static DashboardStatistics DATA_Statistics = new DashboardStatistics
      {
         PlayersOkCount = 1,
         PlayersUnreachableCount = 20,
         PlayersActivCount = 80,
         PlayersUpToDateCount = 70,
         PlayersNotUpToDateCount = 30,
         PlayersConformCount = 66,
         PlayersNotConformCount = 34,
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
         //try
         //{
         //   var device = OutputEmailDevice.Create();
         //   device.Server = "smtp3";
         //   device.Port = 25;
         //   device.UserName = "";
         //   device.Password = "";
         //   device.SenderEmail = "aseguin@telelogos.com";

         //   var from = "aseguin@telelogos.com";
         //   var to = "aseguin@telelogos.com";
         //   MailMessage message = new MailMessage(from, to);
         //   message.From = new MailAddress(Helper.IfNullOrEmpty(from, device.SenderEmail));
         //   device.AddEmailAddresses(message.To, to);
         //   message.Subject = "M4D - Rapport de conformité du " + DateTime.Now.ToLongDateString();
         //   message.Body = "Test d'envoie du rapport de conformité";
         //   message.Attachments.Insert(0, new Attachment(file));

         //   device.SmtpClient.Send(message);
         //   MailMessage message = new MailMessage(from, to);
         //   message.Subject = "M4D - Rapport de conformité du " + DateTime.Now.ToLongDateString();
         //   message.Body = "Test d'envoie du rapport de conformité";
         //   var smtp = new SmtpClient(device.Server, device.Port);
         //   smtp.Send(message);
         //}
         //catch (Exception emailEx)
         //{
         //   Helper.WriteLogEntryScheduler(EventLogEntryType.Error, "Error got trying sending notification email.\r\n{0}", emailEx.Message);
         //}
      }
   }
}

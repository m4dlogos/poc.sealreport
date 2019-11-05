using System;
using System.Diagnostics;
using System.IO;

namespace Telelogos.Reportings.Test
{
   class Program
   {
      static void Main(string[] args)
      {
         // Avec la librairie Telelogos.Reportings
         Settings.RepositoryPath = Path.Combine(Path.GetFullPath(@"..\..\..\"), "Repository"); // remonter au dossier de la solution
         var builder = new DashboardReportBuilder();
         var director = new DashboardReportBuilderDirector();
         var data = new Telelogos.Reportings.DashboardStatistics();
         Helper.ShallowCopyValues(DATA_Statistics, data);
         director.BuildReport(builder, data);

         Console.WriteLine("Format ? (html/pdf): ");
         var format = Console.ReadLine();
         var reportFile = builder.GenerateReport(format == "html" ? ReportFormat.html : ReportFormat.pdf);

         var destFile = Path.Combine(Directory.GetParent("../../").FullName, Path.GetFileName(reportFile));
         File.Copy(reportFile, destFile, true);

         Helper.SendEMail(destFile);

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
   }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Telelogos.Reportings.Test
{
   class Program
   {
      static void Main(string[] args)
      {
			ClydReport();
      }

		static void ClydReport()
		{
			Settings.RepositoryPath = Path.Combine(Path.GetFullPath(@"..\..\..\"), "Repository"); // remonter au dossier de la solution
			var builder = new ClydReportBuilder();
			var director = new ClydReportBuilderDirector();
			var data = new Dictionary<string, List<ChartData>>();
			var connexions = new List<ChartData>
			{
				new ChartData { Label = "1D", Serie = "Last Year", Value = 3},
				new ChartData { Label = "1D", Serie = "Current", Value = 4},
				new ChartData { Label = "1W", Serie = "Last Year", Value = 5},
				new ChartData { Label = "1W", Serie = "Current", Value = 2}
			};

			data.Add(ClydReportBuilder.MODEL_BAR, connexions);

			var utilisations = new List<ChartData>
			{
				new ChartData {Label = "Work", Serie = "", Value = 75},
				new ChartData {Label = "Home", Serie = "", Value = 25}
			};

			data.Add(ClydReportBuilder.MODEL_PIE, utilisations);

			director.BuildReport(builder, data);
			var reportFile = builder.GenerateReport();

			var destFile = Path.Combine(Directory.GetParent("../../").FullName, Path.GetFileName(reportFile));
			File.Copy(reportFile, destFile, true);

			Process.Start(destFile);
		}

		static void M4D_DashboardReport()
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
   }
}

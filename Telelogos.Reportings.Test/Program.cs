﻿using System;
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

         var data = new Dictionary<string, List<ChartDetail>>();
         var devicesConnectionChartData = new List<ChartDetail>
         {
            new ChartDetail { IdData = null, LabelData = "1", SerieData = "C", ValueData = 0, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "1", SerieData = "D", ValueData = 55434, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "3", SerieData = "C", ValueData = 0, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "3", SerieData = "D", ValueData = 55434, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "7", SerieData = "C", ValueData = 0, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "7", SerieData = "D", ValueData = 55434, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "15", SerieData = "C", ValueData = 39666, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "15", SerieData = "D", ValueData = 15768, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "30", SerieData = "C", ValueData = 42793, VersionData = null },
            new ChartDetail { IdData = null, LabelData = "30", SerieData = "D", ValueData = 12641, VersionData = null }
         };

         data.Add("DevicesConnection", devicesConnectionChartData);

         var utilisations = new List<ChartDetail>
         {
            new ChartDetail {IdData = null, LabelData = "Connected", SerieData = "C", ValueData = 75, VersionData = null},
            new ChartDetail {IdData = null, LabelData = "Disconnected", SerieData = "C", ValueData = 25, VersionData = null}
         };

         data.Add("Pie", utilisations);

         var chartDef = new Dictionary<string, ChartType> { { "DevicesConnection", ChartType.Bar }, { "Pie", ChartType.Pie } };
         var columnElem = new Dictionary<string, string> { { "DevicesConnection", ClydReportBuilder.COLUMN_SERIE } };

         director.BuildReport(builder, data, chartDef, columnElement:columnElem);
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
         var data = new Reportings.DashboardStatistics();
         Helper.ShallowCopyValues(DATA_Statistics, data);
         director.BuildReport(builder, data);

         Console.WriteLine("Format ? (html/pdf): ");
         var format = Console.ReadLine();
         Settings.ReportFormat = (format == "pdf") ? ReportFormat.pdf : ReportFormat.html;

         var reportFile = builder.GenerateReport();

         var destFile = Path.Combine(Directory.GetParent("../../").FullName, Path.GetFileName(reportFile));
         File.Copy(reportFile, destFile, true);

         //Helper.SendEMail(destFile);

         // Show the report
         Process.Start(destFile);

         Console.WriteLine("Génération terminée !");
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

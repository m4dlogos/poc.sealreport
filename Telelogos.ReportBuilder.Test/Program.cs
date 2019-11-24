using Seal.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Telelogos.ReportBuilder.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Settings.RepositoryPath = Path.Combine(Path.GetFullPath(@"..\..\..\"), "Repository"); // remonter au dossier de la solution

			var devicesConnectionChartData = new List<ChartDetail>
			{
				new ChartDetail { IdData = null, LabelData = "24H", SerieData = "Connecté", ValueData = 0, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "24H", SerieData = "Déconnecté", ValueData = 55434, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "3J", SerieData = "Connecté", ValueData = 0, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "3J", SerieData = "Déconnecté", ValueData = 55434, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "7J", SerieData = "Connecté", ValueData = 0, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "7J", SerieData = "Déconnecté", ValueData = 55434, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "15J", SerieData = "Connecté", ValueData = 39666, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "15J", SerieData = "Déconnecté", ValueData = 15768, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "30J", SerieData = "Connecté", ValueData = 42793, VersionData = null },
				new ChartDetail { IdData = null, LabelData = "30J", SerieData = "Déconnecté", ValueData = 12641, VersionData = null }
			};

			var builder = new DevicesConnectionReportBuilder() { Data = devicesConnectionChartData };
			var director = new ReportBuilderDirector();
			director.BuildReport(builder);
			var file = builder.GenerateReport(ReportFormat.html);

			Process.Start(file);
		}
	}
}

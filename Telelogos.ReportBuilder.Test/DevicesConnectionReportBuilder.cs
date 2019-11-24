using Seal.Converter;
using Seal.Model;
using System.Collections.Generic;
using System.Data;

namespace Telelogos.ReportBuilder.Test
{
	class DevicesConnectionReportBuilder : ReportBuilder
	{
		public List<ChartDetail> Data { get; set; }

		public override string ReportDisplayName => "Devices Connection";

		public override void AddModels()
		{
			var model = AddModel("DevicesConnection", "DevicesConnection");

			model.AddElement("LabelData", Seal.Model.PivotPosition.Row)
				.SerieDefinition(Seal.Model.SerieDefinition.Axis)
				.SerieSortType(Seal.Model.SerieSortType.None)
				.DisplayName("Label")
				.SortOrder(SortOrderConverter.kNoSortKeyword);

			model.AddElement("ValueData", Seal.Model.PivotPosition.Data)
				.ChartJSSerie(Seal.Model.ChartJSSerieDefinition.Bar)
				.SerieSortType(Seal.Model.SerieSortType.None)
				.DisplayName("Value")
				.SortOrder(SortOrderConverter.kNoSortKeyword);

			model.AddElement("SerieData", Seal.Model.PivotPosition.Column)
				.SerieDefinition(Seal.Model.SerieDefinition.Splitter)
				.SerieSortType(Seal.Model.SerieSortType.None)
				.DisplayName("Serie")
				.SortOrder(SortOrderConverter.kNoSortKeyword);
		}

		public override void AddSources()
		{
			var table = new DataTable();
			table.Columns.Add("IdData", typeof(string));
			table.Columns.Add("LabelData", typeof(string));
			table.Columns.Add("VersionData", typeof(string));
			table.Columns.Add("SerieData", typeof(string));
			table.Columns.Add("ValueData", typeof(decimal));

			if (this.Data != null)
			{
				foreach(var d in this.Data)
				{
					table.Rows.Add(d.IdData, d.LabelData, d.VersionData, d.SerieData, d.ValueData);
				}
			}

			AddSource("DevicesConnection", table);
		}

		public override void AddViews()
		{
			var root = AddRootView();
			var model = root.AddChildView(ReportViewTemplate.ModelName).ModelGUID(_report.Models[0].GUID).Name("DevicesConnection");
			model.AddChildView(ReportViewTemplate.ChartJSName);
			model.AddChildView(ReportViewTemplate.DataTableName);
		}

		
	}
}

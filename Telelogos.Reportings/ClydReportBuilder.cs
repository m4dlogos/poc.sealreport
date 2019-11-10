using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seal.Converter;
using Seal.Model;

namespace Telelogos.Reportings
{
	public class ClydReportBuilder : NoSqlReportBuilder
	{
		public const string COLUMN_LABEL = "Label";
		public const string COLUMN_SERIE = "Serie";
		public const string COLUMN_VALUE = "Value";
		public const string MODEL_BAR = "Bar";
		public const string MODEL_PIE = "Pie";

		public override void CreateReport()
		{
			base.CreateReport();

			_report.DisplayName = "Clyd Report";
			// Clear default objects created by SealReport
			_report.Views.Clear();
			_report.Models.Clear();
			var reportForPdfConfig = Report.LoadFromFile(Path.Combine(Settings.RepositoryPath, "Reports", "pdfconfiguration.srex"), _repository);
			_repository.Configuration.PdfConfigurations = reportForPdfConfig.ExecutionView.PdfConfigurations;
		}


		public override void AddColumnElements(ReportModel model)
		{
			if (model == null)
				return;

			if (model.Name == MODEL_PIE)
				return;

			var master = model.Source.MetaData.MasterTable;
			var column = master.Columns.FirstOrDefault(c => c.Name == COLUMN_SERIE);
			if (column == null)
				return;

			var element = ReportElement.Create();
			element.MetaColumnGUID = column.GUID;
			element.PivotPosition = PivotPosition.Column;
			element.SerieSortType = SerieSortType.None;
			element.SortOrder = SortOrderConverter.kNoSortKeyword;
			element.SerieDefinition = SerieDefinition.Splitter;

			model.Elements.Add(element);
		}

		public override void AddDataElements(ReportModel model)
		{
			if (model == null)
				return;

			var master = model.Source.MetaData.MasterTable;
			var column = master.Columns.FirstOrDefault(c => c.Name == COLUMN_VALUE);
			if (column == null)
				return;

			var element = ReportElement.Create();
			element.MetaColumnGUID = column.GUID;
			element.PivotPosition = PivotPosition.Data;
			element.SerieSortType = SerieSortType.None;
			element.SortOrder = SortOrderConverter.kNoSortKeyword;
			element.ChartJSSerie = (model.Name == MODEL_PIE) ? ChartJSSerieDefinition.Pie : ChartJSSerieDefinition.Bar;

			model.Elements.Add(element);
		}

		public override void AddModel()
		{
			AddModel(MODEL_BAR);
			AddModel(MODEL_PIE);
		}

		public override void AddPageElements(ReportModel model)
		{
			
		}

		public override void AddRowElements(ReportModel model)
		{
			if (model == null)
				return;

			var master = model.Source.MetaData.MasterTable;
			var column = master.Columns.FirstOrDefault(c => c.Name == COLUMN_LABEL);
			if (column == null)
				return;

			var element = ReportElement.Create();
			element.MetaColumnGUID = column.GUID;
			element.PivotPosition = PivotPosition.Row;
			element.SerieSortType = SerieSortType.None;
			element.SortOrder = SortOrderConverter.kNoSortKeyword;
			element.SerieDefinition = SerieDefinition.Axis;

			model.Elements.Add(element);
		}

		// Add the root view
		protected ReportView AddRootView()
		{
			if (_report == null)
				return null;

			_report.Views.Clear();

			var view = _report.AddRootView();
			view.SortOrder = _report.Views.Count > 0 ? _report.Views.Max(i => i.SortOrder) + 1 : 1;
			view.Name = Seal.Helpers.Helper.GetUniqueName("View", (from i in _report.Views select i.Name).ToList());

			return view;
		}

		// Add the ChartJS view
		protected ReportView AddChartJSView(ReportView parentView)
		{
			if (_report == null)
				return null;

			var view = _report.AddChildView(parentView, ReportViewTemplate.ChartJSName);

			view.InitParameters(false);

			if (parentView.Name == MODEL_PIE)
			{
				view.GetParameter("chartjs_doughnut").BoolValue = true;
				view.GetParameter("chartjs_show_legend").BoolValue = true;
			}

			return view;
		}

		// Add the view for the model to the parent view
		protected ReportView AddModelView(ReportView parentView, ReportModel model)
		{
			if (_report == null)
				return null;

			var modelView = _report.AddChildView(parentView, ReportViewTemplate.ModelName);
			// Remove the views created by the model template
			modelView.Views.Clear();
			modelView.Name = model.Name;
			modelView.ModelGUID = model.GUID;

			AddChartJSView(modelView);

			return modelView;
		}

		public override void AddView()
		{
			if (_report == null)
				return;

			var rootView = AddRootView();

			foreach (var model in _report.Models)
			{
				AddModelView(rootView, model);
			}
		}

		public override void BuildResultTable()
		{
			// Vérifier si la table contient déjà les colonnes
			if (_resultTable.Columns.Count > 0)
				return;

			_resultTable.Columns.Add(new DataColumn(COLUMN_LABEL, typeof(string)));
			_resultTable.Columns.Add(new DataColumn(COLUMN_SERIE, typeof(string)));
			_resultTable.Columns.Add(new DataColumn(COLUMN_VALUE, typeof(int)));
		}

		public override void FillResultTable<T>(T data)
		{
			var modelsData = data as Dictionary<string, List<ChartData>>;
			if (modelsData == null)
				return;

			if (_report == null)
				return;

			foreach (var model in _report.Models)
			{
				if (modelsData.ContainsKey(model.Name))
				{
					var resultTable = _resultTable.Clone();
					
					foreach (var chartData in modelsData[model.Name])
					{
						resultTable.Rows.Add(chartData.Label, chartData.Serie, chartData.Value);
					}

					model.ResultTable = resultTable;
				}
			}
		}

		// Generate the report and returns the file path
		public string GenerateReport()
		{
			// Execute the report
			_report.RenderOnly = true;
			_report.Format = Seal.Model.ReportFormat.print;

			var execution = new ReportExecution() { Report = _report };
			execution.Execute();
			while (_report.IsExecuting) System.Threading.Thread.Sleep(100);

			// Generate the report
			var outputFile = execution.GeneratePDFResult();
			//var outputFile = execution.GeneratePrintResult();

			return outputFile;
		}
	}
}

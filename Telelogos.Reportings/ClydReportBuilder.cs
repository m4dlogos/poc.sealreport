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
      public const string COLUMN_ID = "IdData";
      public const string COLUMN_LABEL = "LabelData";
		public const string COLUMN_SERIE = "SerieData";
		public const string COLUMN_VALUE = "ValueData";
      public const string COLUMN_VERSION = "VersionData";

      // List of model names
      public List<string> ModelNames { get; set; }

      // Chart definition by model
      public Dictionary<string, ChartType> ChartDefinition { get; set; }

      // Indicates by model which column is the row element
      public Dictionary<string, string> RowElement { get; set; }

      // Indicates by model which column is the column element
      public Dictionary<string, string> ColumnElement { get; set; }

      // Indicates by model which column is the data element
      public Dictionary<string, string> DataElement { get; set; }

      // Indicates by model which column is the page element
      public Dictionary<string, string> PageElement { get; set; }

      // ChartData by model
      public Dictionary<string, List<ChartDetail>> ChartData { get; set; }

      // Constructor
      public ClydReportBuilder()
      {
         ModelNames = new List<string>();
         ChartDefinition = new Dictionary<string, ChartType>();
         RowElement = new Dictionary<string, string>();
         ColumnElement = new Dictionary<string, string>();
         DataElement = new Dictionary<string, string>();
         PageElement = new Dictionary<string, string>();
         ChartData = new Dictionary<string, List<ChartDetail>>();
      }

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

         if (!ColumnElement.ContainsKey(model.Name))
            return;

			var master = model.Source.MetaData.MasterTable;
			var column = master.Columns.FirstOrDefault(c => c.Name == ColumnElement[model.Name]);
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

         // On peut spécifier la colonne à prendre comme Data element: par défaut c'est la colonne Value
         var dataColumn = DataElement.ContainsKey(model.Name) ? DataElement[model.Name] : COLUMN_VALUE;

			var master = model.Source.MetaData.MasterTable;
			var column = master.Columns.FirstOrDefault(c => c.Name == dataColumn);
			if (column == null)
				return;

			var element = ReportElement.Create();
			element.MetaColumnGUID = column.GUID;
			element.PivotPosition = PivotPosition.Data;
			element.SerieSortType = SerieSortType.None;
			element.SortOrder = SortOrderConverter.kNoSortKeyword;

         if (ChartDefinition.ContainsKey(model.Name))
            element.ChartJSSerie = (ChartJSSerieDefinition)ChartDefinition[model.Name];

			model.Elements.Add(element);
		}

		public override void AddModel()
		{
         foreach (var name in ModelNames)
            AddModel(name);
		}

		public override void AddPageElements(ReportModel model)
		{
         if (model == null)
            return;

         if (!PageElement.ContainsKey(model.Name))
            return;

         var master = model.Source.MetaData.MasterTable;
         var column = master.Columns.FirstOrDefault(c => c.Name == PageElement[model.Name]);
         if (column == null)
            return;

         var element = ReportElement.Create();
         element.MetaColumnGUID = column.GUID;
         element.PivotPosition = PivotPosition.Page;
         element.SerieSortType = SerieSortType.None;
         element.SortOrder = SortOrderConverter.kNoSortKeyword;

         model.Elements.Add(element);
      }

		public override void AddRowElements(ReportModel model)
		{
			if (model == null)
				return;
			
         // On peut spécifier la colonne pour la Row element : par défaut c'est colonne Label
         var rowColumn = RowElement.ContainsKey(model.Name) ? RowElement[model.Name] : COLUMN_LABEL;

         var master = model.Source.MetaData.MasterTable;
         var column = master.Columns.FirstOrDefault(c => c.Name == rowColumn);
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
         view.InitParameters(false);
         view.GetParameter("show_pdf_button").BoolValue = false;

         return view;
		}

		// Add the ChartJS view
		protected ReportView AddChartJSView(ReportView parentView)
		{
			if (_report == null)
				return null;

			var view = _report.AddChildView(parentView, ReportViewTemplate.ChartJSName);

			view.InitParameters(false);
         view.GetParameter("chartjs_doughnut").BoolValue = true;
         view.GetParameter("chartjs_show_legend").BoolValue = true;

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

         _resultTable.Columns.Add(new DataColumn(COLUMN_ID, typeof(string)));
         _resultTable.Columns.Add(new DataColumn(COLUMN_LABEL, typeof(string)));
			_resultTable.Columns.Add(new DataColumn(COLUMN_SERIE, typeof(string)));
			_resultTable.Columns.Add(new DataColumn(COLUMN_VALUE, typeof(decimal)));
         _resultTable.Columns.Add(new DataColumn(COLUMN_VERSION, typeof(string)));
		}

		public override void FillResultTable<T>(T data)
		{
			var modelsData = data as Dictionary<string, List<ChartDetail>>;
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
						resultTable.Rows.Add(chartData.IdData, chartData.LabelData, chartData.SerieData, chartData.ValueData, chartData.VersionData);
					}

					model.ResultTable = resultTable;
				}
			}
		}

      public void FillResultTable()
      {
         FillResultTable(ChartData);
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

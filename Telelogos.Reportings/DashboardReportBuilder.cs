using Seal.Converter;
using Seal.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;


namespace Telelogos.Reportings
{
   // Class that implements the dashboard report generation
   public class DashboardReportBuilder : NoSqlReportBuilder
   {
      #region Constantes
      public const string GREEN = "#10BE5D";
      public const string RED = "#EA6153";
      public const string ORANGE = "#FD990B";
      public const string COLUMN_STATISTIC = "Statistic";
      public const string COLUMN_VALUE = "Value";
      public const string MODEL_CONFORMITY = "Conformity Model";
      public const string MODEL_CONNECTION = "Connection Model";
      public const string MODEL_UPDATE = "Update Model";
      public const string MODEL_DATATABLE = "DataTable";
      public const string STAT_CONFORM = "Conforme";
      public const string STAT_NOT_CONFORM = "Non conforme";
      public const string STAT_OK = "Connecté";
      public const string STAT_UNREACHABLE = "Injoignable";
      public const string STAT_UP_TO_DATE = "A jour";
      public const string STAT_NOT_UP_TO_DATE = "Non à jour";
      #endregion


      // The colors by model
      public static Dictionary<string, string> Colors = new Dictionary<string, string>
      {
         { MODEL_CONFORMITY, $"['{GREEN}', '{RED}']" },
         { MODEL_CONNECTION, $"['{GREEN}','{ORANGE}']" },
         { MODEL_UPDATE, $"['{GREEN}','{RED}']" }
      };

      // The statistics by model
      public static Dictionary<string, List<string>> Statistics = new Dictionary<string, List<string>>
      {
         { MODEL_CONFORMITY, new List<string> { STAT_CONFORM, STAT_NOT_CONFORM } },
         { MODEL_CONNECTION, new List<string> { STAT_OK, STAT_UNREACHABLE } },
         { MODEL_UPDATE, new List<string> { STAT_UP_TO_DATE, STAT_NOT_UP_TO_DATE } }
      };

      // Default constructor
      public DashboardReportBuilder()
      {
      }

      // Build and returns the result table
      public override void BuildResultTable()
      {
         // Vérifier si la table contient déjà les colonnes
         if (_resultTable.Columns.Count > 0)
            return;

         _resultTable.Columns.Add(new DataColumn(COLUMN_STATISTIC, typeof(string)));
         _resultTable.Columns.Add(new DataColumn(COLUMN_VALUE, typeof(int)));
      }

      // Create the report
      public override void CreateReport()
      {
         base.CreateReport();

         _report.DisplayName = "Dashboard Report";
         // Clear default objects created by SealReport
         _report.Views.Clear();
         _report.Models.Clear();
      }

      // Add the models
      public override void AddModel()
      {
         AddModel(MODEL_CONFORMITY);
         AddModel(MODEL_CONNECTION);
         AddModel(MODEL_UPDATE);
         AddModel(MODEL_DATATABLE);
      }

      // Fill the result table of each model
      public override void FillResultTable<T>(T data)
      {
         var statistics = data as DashboardStatistics;
         if (statistics == null)
            return;
         if (_report == null)
            return;

         foreach (var model in _report.Models)
            FillResultTable(model, statistics);
      }

      // Fill the result table for the model
      protected void FillResultTable(ReportModel model, DashboardStatistics statistics)
      {
         if (model == null)
            return;

         var resultTable = _resultTable.Clone();

         switch (model.Name)
         {
            case MODEL_CONFORMITY:
               {
                  resultTable.Rows.Add(STAT_CONFORM, statistics.PlayersConformCount);
                  resultTable.Rows.Add(STAT_NOT_CONFORM, statistics.PlayersNotConformCount);
               }
               break;
            case MODEL_CONNECTION:
               {
                  resultTable.Rows.Add(STAT_OK, statistics.PlayersOkCount);
                  resultTable.Rows.Add(STAT_UNREACHABLE, statistics.PlayersUnreachableCount);
               }
               break;
            case MODEL_UPDATE:
               {
                  resultTable.Rows.Add(STAT_UP_TO_DATE, statistics.PlayersUpToDateCount);
                  resultTable.Rows.Add(STAT_NOT_UP_TO_DATE, statistics.PlayersNotUpToDateCount);
               }
               break;
            case MODEL_DATATABLE:
               {
                  resultTable.Rows.Add(STAT_CONFORM, statistics.PlayersConformCount);
                  resultTable.Rows.Add(STAT_NOT_CONFORM, statistics.PlayersNotConformCount);
                  resultTable.Rows.Add(STAT_OK, statistics.PlayersOkCount);
                  resultTable.Rows.Add(STAT_UNREACHABLE, statistics.PlayersUnreachableCount);
                  resultTable.Rows.Add(STAT_UP_TO_DATE, statistics.PlayersUpToDateCount);
                  resultTable.Rows.Add(STAT_NOT_UP_TO_DATE, statistics.PlayersNotUpToDateCount);
               }
               break;
         }

         model.ResultTable = resultTable;
      }

      // Add the row elements to the model
      public override void AddRowElements(ReportModel model)
      {
         if (model == null)
            return;

         var master = model.Source.MetaData.MasterTable;
         var column = master.Columns.FirstOrDefault(c => c.Name == COLUMN_STATISTIC);
         if (column == null)
            return;

         var element = ReportElement.Create();
         element.MetaColumnGUID = column.GUID;
         element.PivotPosition = PivotPosition.Row;
         element.SerieSortType = SerieSortType.None;
         element.SortOrder = SortOrderConverter.kNoSortKeyword;

         if (model.Name == MODEL_DATATABLE)
         {
            element.DisplayName = "Statistique";
         }
         else
         {
            element.SerieDefinition = SerieDefinition.Axis;
         }

         model.Elements.Add(element);
      }

      // Add the data elements to the model
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

         if (model.Name == MODEL_DATATABLE)
         {
            element.DisplayName = "Nombre de players";
         }
         else
         {
            element.ChartJSSerie = ChartJSSerieDefinition.Pie;
         }

         model.Elements.Add(element);
      }

      // Add the page elements to the model
      public override void AddPageElements(ReportModel model)
      {

      }

      // Add the column elements to the model
      public override void AddColumnElements(ReportModel model)
      {

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

      // Add the container view
      protected ReportView AddContainerView(ReportView parentView)
      {
         if (_report == null)
            return null;

         var view = _report.AddChildView(parentView, "Container");
         view.InitParameters(false);
         view.Parameters.FirstOrDefault(p => p.Name == "grid_layout").Value = "col-sm-4;col-sm-4;col-sm-4\ncol-sm-4";

         return view;
      }

      // Add the views
      public override void AddView()
      {
         if (_report == null)
            return;

         var rootView = AddRootView();
         var containerView = AddContainerView(rootView);

         foreach (var model in _report.Models)
         {
            AddModelView(containerView, model);
         }
      }

      // Add the ChartJS view
      protected ReportView AddChartJSView(ReportView parentView, string colors)
      {
         if (_report == null)
            return null;

         var view = _report.AddChildView(parentView, ReportViewTemplate.ChartJSName);

         view.InitParameters(false);
         view.GetParameter("chartjs_doughnut").BoolValue = true;
         view.GetParameter("chartjs_show_legend").BoolValue = true;
         view.GetParameter("chartjs_legend_position").TextValue = "bottom";
         view.GetParameter("chartjs_colors").Value = colors;
         view.GetParameter("chartjs_options_circumference").NumericValue = 225; // 1.25*PI
         view.GetParameter("chartjs_options_rotation").NumericValue = 90; // 0.5*PI

         return view;
      }

      // Add datatable view
      protected ReportView AddDataTableView(ReportView parentView)
      {
         if (_report == null)
            return null;

         var view = _report.AddChildView(parentView, ReportViewTemplate.DataTableName);

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

         if (model.Name != MODEL_DATATABLE)
         {
            AddChartJSView(modelView, Colors[model.Name]);
         }
         else
         {
            AddDataTableView(modelView);
         }

         return modelView;
      }

      // Generate the report and returns the file path
      public string GenerateReport(ReportFormat format = ReportFormat.html)
      {
         // Execute the report
         _report.RenderOnly = true;
         _report.Format = Seal.Model.ReportFormat.print;

         var execution = new ReportExecution() { Report = _report };
         execution.Execute();
         while (_report.IsExecuting) System.Threading.Thread.Sleep(100);

         // Generate the report
         var outputFile = (format == ReportFormat.html) ? execution.GeneratePrintResult() : execution.GeneratePDFResult();
         
         return outputFile;
      }
   }
}

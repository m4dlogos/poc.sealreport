using Seal.Model;
using System.Data;
using System.Linq;

namespace Telelogos.Reportings
{
   /// <summary>
   /// Base class to implement a report builder without database
   /// </summary>
   public abstract class NoSqlReportBuilder : ReportBuilder
   {
      public const string SOURCE_NAME = "No SQL Source";

      #region Attributs
      /// <summary>
      /// DataTable to store the results data
      /// </summary>
      protected DataTable _resultTable;
      #endregion

      /// <summary>
      /// Constructor
      /// </summary>
      public NoSqlReportBuilder()
      {
         _resultTable = new DataTable();
      }

      #region Abstract methods
      /// <summary>
      /// Builds the result table
      /// </summary>
      public abstract void BuildResultTable();

      /// <summary>
      /// Add the model
      /// </summary>
      public abstract void AddModel();

      /// <summary>
      /// Add the view
      /// </summary>
      public abstract void AddView();

      /// <summary>
      /// Fill the result table with the input data
      /// </summary>
      /// <typeparam name="T">Type of the input data structure</typeparam>
      /// <param name="data">The data used to fill the result table</param>
      public abstract void FillResultTable<T>(T data);
      #endregion

      /// <summary>
      /// Add the source
      /// </summary>
      public virtual void AddSource()
      {
         var source = AddSource(SOURCE_NAME);
         AddMasterTable(source);
      }

      /// <summary>
      /// Add the master table
      /// </summary>
      /// <param name="source">The source for the master table</param>
      /// <returns>The master table</returns>
      public virtual MetaTable AddMasterTable(MetaSource source)
      {
         if (_resultTable.Columns.Count == 0)
            BuildResultTable();

         // Remove the master tables
         source.MetaData.Tables.RemoveAll(t => t.Alias == MetaData.MasterTableName);

         var master = MetaTable.Create();
         master.DynamicColumns = true;
         master.IsEditable = false;
         master.Alias = MetaData.MasterTableName;
         master.Source = source;

         foreach (DataColumn column in _resultTable.Columns)
         {
            var metaColumn = MetaColumn.Create(column.ColumnName);
            metaColumn.Source = source;
            metaColumn.DisplayName = Seal.Helpers.Helper.DBNameToDisplayName(column.ColumnName.Trim());
            metaColumn.Category = "Master";
            metaColumn.DisplayOrder = master.GetLastDisplayOrder();
            metaColumn.Type = Seal.Helpers.Helper.NetTypeConverter(column.DataType);
            metaColumn.SetStandardFormat();

            master.Columns.Add(metaColumn);
         }

         source.MetaData.Tables.Add(master);

         return master;
      }

      /// <summary>
      /// Add the model to the report and configure the elements of the model
      /// </summary>
      /// <param name="modelName">The name of the model to add</param>
      /// <returns>The created model</returns>
      public virtual ReportModel AddModel(string modelName)
      {
         if (_report == null)
            CreateReport();

         var model = _report.AddModel(false);
         model.Name = modelName;

         AddRowElements(model);
         AddDataElements(model);
         AddPageElements(model);
         AddColumnElements(model);

         model.InitReferences();
         return model;
      }

      /// <summary>
      /// Create an add the No SQL source as the default source
      /// </summary>
      /// <param name="sourceName">The source name</param>
      /// <param name="isDefault">Indicates whether the source to add must be the default source</param>
      /// <returns>The created source</returns>
      public virtual MetaSource AddSource(string sourceName, bool isDefault = true)
      {
         if (_repository == null)
            CreateRepository();

         var source = MetaSource.Create(_repository);
         source.Name = sourceName;
         source.IsNoSQL = true;
         source.IsDefault = isDefault;

         if (isDefault)
            _repository.Sources.All(s => s.IsDefault = false);

         _repository.Sources.Add(source);

         return source;
      }
   }
}

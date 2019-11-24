using Seal.Model;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

namespace Telelogos.ReportBuilder
{
   /// <summary>
   /// Classe de base pour implémenter un report builder
   /// </summary>
   public abstract class ReportBuilder
   {
		#region Attributs
		/// <summary>
		/// L'objet qui représente le dossier Repository
		/// </summary>
		protected Repository _repository;

      /// <summary>
      /// L'objet qui représente le rapport
      /// </summary>
      protected Report _report;

		/// <summary>
		/// L'objet qui execute le build du rapport
		/// </summary>
		protected ReportExecution _execution;
		#endregion

		#region Méthodes
		/// <summary>
		/// Supprime les sources
		/// </summary>
		public void ClearSources()
		{
			if (_report != null)
				_report.Sources.Clear();
		}

		/// <summary>
		/// Supprime les models
		/// </summary>
		public void ClearModels()
		{
			if (_report != null)
				_report.Models.Clear();
		}

		/// <summary>
		/// Supprime les vues
		/// </summary>
		public void ClearViews()
		{
			if (_report != null)
				_report.Views.Clear();
		}
		#endregion

		#region Méthodes virtuelles
		/// <summary>
		///Crée l'objet repository à partir des settings
		/// </summary>
		public virtual void CreateRepository()
      {
         if (!string.IsNullOrEmpty(Settings.RepositoryPath) && Directory.Exists(Settings.RepositoryPath))
         {
            _repository = Repository.Create(Settings.RepositoryPath);
         }
         else
         {
            _repository = Repository.Create();
         }

         if (!string.IsNullOrEmpty(Settings.WebApplicationPath))
         {
            _repository.WebApplicationPath = Settings.WebApplicationPath;
         }
      }

      /// <summary>
      /// Crée l'objet rapport
      /// </summary>
      public virtual void CreateReport()
      {
         if (_repository == null)
            CreateRepository();

         _report = Report.Create(_repository);
			ClearViews();
			ClearModels();

			_report.DisplayName = this.ReportDisplayName;
		}

		/// <summary>
		/// Ajoute une source de données non sql au rapport
		/// </summary>
		/// <param name="name">Le nom de la source</param>
		/// <param name="table">La table contenant les données</param>
		public virtual void AddSource(string name, DataTable table)
		{
			if (_report == null)
				CreateReport();

			// Ajouter une source non sql au rapport
			var newSource = _report.AddSource(null);
			newSource.Name = name;
			newSource.IsNoSQL = true;
			// Ajouter une master table à la source
			AddMasterTable(newSource, table);
		}

		/// <summary>
		/// Ajoute une master table à la source non sql
		/// </summary>
		/// <param name="source">La source où ajouter la master table</param>
		/// <param name="table">La table de données utilisée pour créer la master table</param>
		public virtual void AddMasterTable(ReportSource source, DataTable table)
		{
			var master = MetaTable.Create();
			master.DynamicColumns = true;
			master.IsEditable = true;
			master.Alias = MetaData.MasterTableName;
			master.Source = source;
			master.NoSQLTable = table;

			foreach (DataColumn column in table.Columns)
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
		}

		/// <summary>
		/// Ajoute un model lié à une source
		/// </summary>
		/// <param name="modelName">Le nom du model à créer</param>
		/// <param name="sourceName">Le nom de la source</param>
		/// <returns>Le model ajouté</returns>
		public virtual ReportModel AddModel(string modelName, string sourceName)
		{
			if (_report == null)
				CreateReport();

			var source = _report.Sources.FirstOrDefault(s => s.Name == sourceName);
			if (source == null)
				throw new ArgumentException($"La source {sourceName} n'existe pas.");

			// Ajoute un nouveau model
			var model = _report.AddModel(source.IsNoSQL);
			model.Name = modelName;
			model.SourceGUID = source.GUID;

			if (source.IsNoSQL)
				model.ResultTable = source.MetaData.MasterTable.NoSQLTable;

			return model;
		}

		/// <summary>
		/// Execute le build du rapport
		/// </summary>
		public virtual void Execute()
		{
			// Execute the report
			_report.RenderOnly = true;

			_execution = new ReportExecution() { Report = _report };
			_execution.Execute();
			while (_report.IsExecuting)
			{
				System.Threading.Thread.Sleep(100);
			}
		}

		/// <summary>
		/// Génère un rapport au format demandé et renvoie le chemin du fichier
		/// </summary>
		/// <param name="format">Le format du fichier</param>
		/// <returns>Le chemin du fichier généré</returns>
		public virtual string GenerateReport(ReportFormat format)
		{
			string path = string.Empty;

			if (_execution == null)
				throw new InvalidOperationException("Le rapport doit être construit en appelant ReportBuilder.Execute avant d'être généré.");

			switch (format)
			{
				case ReportFormat.html:
					path = _execution.GenerateHTMLResult();
					break;
				case ReportFormat.print:
					path = _execution.GeneratePrintResult();
					break;
				case ReportFormat.csv:
					path = _execution.GenerateCSVResult();
					break;
				case ReportFormat.pdf:
					path = _execution.GeneratePDFResult();
					break;
				case ReportFormat.excel:
					path = _execution.GenerateExcelResult();
					break;
				case ReportFormat.custom:
					throw new NotImplementedException($"Le format {format.ToString()} n'est pas implémenté");
			}

			return path;
		}

		/// <summary>
		/// Ajoute une vue racine
		/// </summary>
		/// <returns>La vue racine</returns>
		public virtual ReportView AddRootView()
		{
			if (_report == null)
				CreateReport();

			var view = _report.AddRootView();
			view.SortOrder = _report.Views.Count > 0 ? _report.Views.Max(i => i.SortOrder) + 1 : 1;
			view.Name = Seal.Helpers.Helper.GetUniqueName("View", (from i in _report.Views select i.Name).ToList());
			view.InitParameters(true);

			return view;
		}
		#endregion

		#region Méthodes abstraites
		/// <summary>
		/// Ajoute les sources
		/// </summary>
		public abstract void AddSources();

		/// <summary>
		/// Ajoute les models
		/// </summary>
		public abstract void AddModels();

		/// <summary>
		/// Ajoute les vues
		/// </summary>
		public abstract void AddViews();

		/// <summary>
		/// Le nom du rapport pour l'affichage
		/// </summary>
		public abstract string ReportDisplayName { get; }
		#endregion
	}
}

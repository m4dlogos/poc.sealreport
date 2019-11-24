using Seal.Model;
using System;
using System.Linq;

namespace Telelogos.ReportBuilder
{
	/// <summary>
	/// Classe d'extension de la classe Seal.Model.ReportModel
	/// </summary>
	public static class ReportModelExtension
	{
		/// <summary>
		/// Ajoute un élément au model et le retourne pour qu'il soit configuré
		/// </summary>
		/// <param name="model">Le model</param>
		/// <param name="columnName">Le nom de la colonne</param>
		/// <param name="position">La position</param>
		/// <returns>L'élément ajouté</returns>
		public static ReportElement AddElement(this ReportModel model, string columnName, PivotPosition position)
		{
			var master = model.Source.MetaData.MasterTable;
			var column = master.Columns.FirstOrDefault(c => c.Name == columnName);
			if (column == null)
				throw new ArgumentException($"La colonne {columnName} n'existe pas.");

			var element = ReportElement.Create();
			element.Source = model.Source;
			element.Format = column.Format;
			element.Report = model.Report;
			element.Model = model;
			element.MetaColumnGUID = column.GUID;
			element.MetaColumn = column;
			element.Name = column.Name;
			element.PivotPosition = position;
			element.SetDefaults();
			model.Elements.Add(element);

			return element;
		}
	}
}

using Seal.Model;

namespace Telelogos.ReportBuilder
{
	/// <summary>
	/// Classe d'extension de la classe Seal.Model.ReportElement
	/// </summary>
	public static class ReportElementExtension
	{
		/// <summary>
		/// Définit comment l'élément est utilisé dans le graphique. Les Row et Column éléments peuvent être soit Axis ou Splitter (pour créer une série pour chaque valeur de split).
		/// </summary>
		/// <param name="element">L'élément à modifier</param>
		/// <param name="serieDefinition">La définition</param>
		/// <returns>L'élément modifié</returns>
		public static ReportElement SerieDefinition(this ReportElement element, SerieDefinition serieDefinition)
		{
			element.SerieDefinition = serieDefinition;
			return element;
		}

		/// <summary>
		/// Définit la série pour l'élément dans le graphique CharJS
		/// </summary>
		/// <param name="element">L'élément à modifier</param>
		/// <param name="chartJSSerieDefinition">Le type de chart</param>
		/// <returns>L'élément modifié</returns>
		public static ReportElement ChartJSSerie(this ReportElement element, ChartJSSerieDefinition chartJSSerieDefinition)
		{
			element.ChartJSSerie = chartJSSerieDefinition;
			return element;
		}

		/// <summary>
		/// Définit comment les séries sont triées dans le graphique
		/// </summary>
		/// <param name="element">L'élément à modifier</param>
		/// <param name="serieSortType">Le type de tri</param>
		/// <returns>L'élément modifié</returns>
		public static ReportElement SerieSortType(this ReportElement element, SerieSortType serieSortType)
		{
			element.SerieSortType = serieSortType;
			return element;
		}

		/// <summary>
		/// Nom utilisé pour afficher la colonne dans les résultats du rapport
		/// </summary>
		/// <param name="element">L'élément à modifier</param>
		/// <param name="name">Le nom à afficher</param>
		/// <returns>L'élément modifié</returns>
		public static ReportElement DisplayName(this ReportElement element, string name)
		{
			element.DisplayName = name;
			return element;
		}

		/// <summary>
		/// Trie la table des résultats. Les éléments Page sont triés en premier suivi de Row, Column et Data.
		/// </summary>
		/// <param name="element">L'élément à modifier</param>
		/// <param name="sortOrder">Le tri</param>
		/// <returns>L'élément modifié</returns>
		public static ReportElement SortOrder(this ReportElement element, string sortOrder)
		{
			element.SortOrder = sortOrder;
			return element;
		}
	}
}

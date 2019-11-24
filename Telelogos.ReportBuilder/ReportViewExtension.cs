using Seal.Model;
using System;

namespace Telelogos.ReportBuilder
{
	/// <summary>
	/// Classe d'extension de Seal.Model.ReportView
	/// </summary>
	public static class ReportViewExtension
	{
		/// <summary>
		/// Ajoute une vue enfant à partir de son template
		/// </summary>
		/// <param name="parentView">La vue parent</param>
		/// <param name="templateName">Le nom du template pour la vue enfant</param>
		/// <returns>La vue enfant</returns>
		public static ReportView AddChildView(this ReportView parentView, string templateName)
		{
			var report = parentView.Report;
			if (report == null)
				throw new InvalidOperationException("La vue ne possède pas de rapport.");

			var childView = report.AddChildView(parentView, templateName);
			// On nettoie les vues enfants de la vue créée car parfois SealReport ajoute des vues enfants par défaut (cas de la vue Model).
			childView.Views.Clear();
			childView.InitParameters(true);

			return childView;
		}

		/// <summary>
		/// Set la valeur du paramètre
		/// </summary>
		/// <param name="view">La vue à paramétrer</param>
		/// <param name="name">Le nom du paramètre</param>
		/// <param name="value">La valeur du paramètre</param>
		/// <returns>La vue paramétrée</returns>
		public static ReportView SetParameter(this ReportView view, string name, string value)
		{
			var parameter = view.GetParameter(name);
			if (parameter != null)
				parameter.Value = value;

			return view;
		}

		/// <summary>
		/// Set la valeur de la propriété ModelGUID
		/// </summary>
		/// <param name="view">La vue à modifier</param>
		/// <param name="guid">La valeur à setter</param>
		/// <returns>La vue modifiée</returns>
		public static ReportView ModelGUID(this ReportView view, string guid)
		{
			view.ModelGUID = guid;
			return view;
		}

		/// <summary>
		/// Set le nom de la vue
		/// </summary>
		/// <param name="view">La vue à modifier</param>
		/// <param name="name">Le nom de la vue</param>
		/// <returns>La vue modifiée</returns>
		public static ReportView Name(this ReportView view, string name)
		{
			view.Name = name;
			return view;
		}
	}
}

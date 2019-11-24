using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telelogos.ReportBuilder
{
	/// <summary>
	/// Classe chargé de créer un rapport à l'aide du builder fournit
	/// </summary>
	public class ReportBuilderDirector
	{
		/// <summary>
		/// Crée un rapport à l'aide du builder fournit
		/// </summary>
		/// <param name="builder">Le builder du rapport</param>
		public virtual void BuildReport(ReportBuilder builder)
		{
			builder.CreateRepository();
			builder.CreateReport();
			builder.AddSources();
			builder.AddModels();
			builder.AddViews();
			builder.Execute();
		}
	}
}

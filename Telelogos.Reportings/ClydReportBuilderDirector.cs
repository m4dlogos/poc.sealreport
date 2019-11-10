using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telelogos.Reportings
{
	public class ClydReportBuilderDirector
	{
		// Build the report
		public void BuildReport(ClydReportBuilder builder, Dictionary<string, List<ChartData>> data)
		{
			builder.CreateRepository();
			builder.AddSource();
			builder.CreateReport();
			builder.AddModel();
			builder.AddView();
			builder.FillResultTable(data);
		}
	}
}

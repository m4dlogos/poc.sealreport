using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telelogos.Reportings
{
	public class ClydReportBuilderDirector
	{      
		public void BuildReport(ClydReportBuilder builder, 
                              Dictionary<string, List<ChartDetail>> chartData, 
                              Dictionary<string, ChartType> chartDefinition,
                              Dictionary<string, string> rowElement = null,
                              Dictionary<string, string> dataElement = null,
                              Dictionary<string, string> columnElement = null,
                              Dictionary<string, string> pageElement = null)
		{
         builder.ModelNames = chartData.Keys.ToList();
         builder.ChartData = chartData;
         builder.ChartDefinition = chartDefinition;

         if (rowElement != null)
            builder.RowElement = rowElement;
         if (dataElement != null)
            builder.DataElement = dataElement;
         if (columnElement != null)
            builder.ColumnElement = columnElement;
         if (pageElement != null)
            builder.PageElement = pageElement;

			builder.CreateRepository();
			builder.AddSource();
			builder.CreateReport();
			builder.AddModel();
			builder.AddView();
			builder.FillResultTable();
		}
   }
}

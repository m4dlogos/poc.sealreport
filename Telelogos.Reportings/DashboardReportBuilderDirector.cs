using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telelogos.Reportings
{
   // Class that handles the dashreport report builder
   public class DashboardReportBuilderDirector
   {
      // Constructor
      public DashboardReportBuilderDirector()
      {
      }

      // Build the report
      public void BuildReport(DashboardReportBuilder builder, DashboardStatistics statistics)
      {
         builder.CreateRepository();
         builder.AddSource();
         builder.CreateReport();
         builder.AddModel();
         builder.AddView();
         builder.FillResultTable(statistics);
      }
   }
}

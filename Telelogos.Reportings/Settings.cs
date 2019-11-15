

using System.ComponentModel;

namespace Telelogos.Reportings
{
   public static class Settings
   {
      public static string RepositoryPath { get; set; }
      public static string WebApplicationPath { get; set; }

      [DefaultValue(ReportFormat.html)]
      public static ReportFormat ReportFormat { get; set; }
   }
}

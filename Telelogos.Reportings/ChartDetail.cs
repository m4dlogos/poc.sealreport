
namespace Telelogos.Reportings
{
   /// <summary>
   /// Obtient ou définit un détail de graphique
   /// </summary>
   public class ChartDetail
   {
      // Id de la donnée
      public string IdData { get; set; }

      // Label de la donnée
      public string LabelData { get; set; }

      // Version de la donnée
      public string VersionData { get; set; }

      // Série de la donnée
      public string SerieData { get; set; }

      // Valeur de la donnée
      public decimal ValueData { get; set; }
   }
}
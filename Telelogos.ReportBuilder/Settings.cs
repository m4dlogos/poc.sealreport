using System.ComponentModel;

namespace Telelogos.ReportBuilder
{
   public static class Settings
   {
      /// <summary>
      /// Le chemin du dossier Repository
      /// </summary>
      public static string RepositoryPath { get; set; }

      /// <summary>
      /// Le chemin de l'application path quand on est dans une application web
      /// </summary>
      public static string WebApplicationPath { get; set; }
   }
}

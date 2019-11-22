using Seal.Model;
using System.IO;

namespace Telelogos.ReportBuilder
{
   /// <summary>
   /// Classe abstraite de base pour implémenter un report builder
   /// </summary>
   public abstract class ReportBuilder
   {
      /// <summary>
      /// L'objet qui représente le dossier Repository
      /// </summary>
      protected Repository _repository;

      /// <summary>
      /// L'objet qui représente le rapport
      /// </summary>
      protected Report _report;

      /// <summary>
      ///Crée l'objet repository à partir des settings
      /// </summary>
      public virtual void CreateRepository()
      {
         if (!string.IsNullOrEmpty(Settings.RepositoryPath) && Directory.Exists(Settings.RepositoryPath))
         {
            _repository = Repository.Create(Settings.RepositoryPath);
         }
         else
         {
            _repository = Repository.Create();
         }

         if (!string.IsNullOrEmpty(Settings.WebApplicationPath))
         {
            _repository.WebApplicationPath = Settings.WebApplicationPath;
         }
      }

      /// <summary>
      /// Crée l'objet rapport
      /// </summary>
      public virtual void CreateReport()
      {
         if (_repository == null)
            CreateRepository();

         _report = Report.Create(_repository);
      }
   }
}

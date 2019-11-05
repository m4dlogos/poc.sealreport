

using Seal.Model;
using System.IO;

namespace Telelogos.Reportings
{
   /// <summary>
   /// Base class to implement a report builder
   /// </summary>
   public abstract class ReportBuilder
   {
      protected Repository _repository;
      protected Report _report;

      /// <summary>
      /// Create the repository according to the settings
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
      /// Create the report
      /// </summary>
      public virtual void CreateReport()
      {
         if (_repository == null)
            CreateRepository();

         _report = Report.Create(_repository);
      }

      /// <summary>
      /// Add page elements to the model
      /// </summary>
      /// <param name="model">The model to configure</param>
      public abstract void AddPageElements(ReportModel model);

      /// <summary>
      /// Add column elements to the model
      /// </summary>
      /// <param name="model">The model to configure</param>
      public abstract void AddColumnElements(ReportModel model);

      /// <summary>
      /// Add row elements to the model
      /// </summary>
      /// <param name="model">The model to configure</param>
      public abstract void AddRowElements(ReportModel model);

      /// <summary>
      /// Add data elements to the model
      /// </summary>
      /// <param name="model">The model to configure</param>
      public abstract void AddDataElements(ReportModel model);
   }
}

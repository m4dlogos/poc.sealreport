using Seal.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telelogos.ReportBuilder
{
   /// <summary>
   /// Classe dédiée à la création de rapport avec une source de données non SQL
   /// </summary>
   public abstract class NoSqlReportBuilder : ReportBuilder
   {
      public virtual void AddSource(string name, DataTable data)
      {
         if (_report == null)
            CreateReport();

         // Add no sql source
         var newSource = _report.AddSource(null);
         newSource.IsNoSQL = true;
         // Add master table
         var master = MetaTable.Create();
         master.DynamicColumns = true;
         master.IsEditable = true;
         master.Alias = MetaData.MasterTableName;
         master.Source = newSource;
         newSource.MetaData.Tables.Add(master);
      }
   }
}

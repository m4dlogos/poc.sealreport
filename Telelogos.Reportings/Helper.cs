using Seal.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telelogos.MediaContact.SharedTools.Helpers;
using Telelogos.MediaContact.SharedTools.Models;

namespace Telelogos.Reportings
{
   public static class Helper
   {
      // Copy the first object fields values to the second one
      public static void ShallowCopyValues<T1, T2>(T1 firstObject, T2 secondObject)
      {
         const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
         var firstFieldDefinitions = firstObject.GetType().GetFields(bindingFlags);
         var secondFieldDefinitions = secondObject.GetType().GetFields(bindingFlags);

         foreach (var fieldDefinition in firstFieldDefinitions)
         {
            var matchingFieldDefinition = secondFieldDefinitions.FirstOrDefault(fd => fd.Name == fieldDefinition.Name &&
                                                                                      fd.FieldType == fieldDefinition.FieldType);
            if (matchingFieldDefinition == null)
               continue;

            var value = fieldDefinition.GetValue(firstObject);
            matchingFieldDefinition.SetValue(secondObject, value);
         }
      }

      // Send the email with the report as attached file
      public static void SendEMail(string attachmentPath)
      {
         try
         {
            var body = "Voici votre rapport journalier !";
            var helper = new MailHelper();
            var message = helper.GetMailMessage("dev.telelogos@gmail.com", "aseguin@telelogos.com", null, null, "[M4D] Rapport de conformité du " + DateTime.Today.ToLongDateString(), body, attachmentPath);
            var config = new SmtpConfiguration { Host = "smtp.gmail.com", Password = "!telelogos2009", User = "dev.telelogos@gmail.com", Port = 465, UseSecureSocket = true };
            helper.SendMail(config, message);
         }
         catch (Exception ex)
         {
            // TODO: Log something
         }
      }
   }
}

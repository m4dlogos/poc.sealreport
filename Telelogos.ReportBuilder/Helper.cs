using System.Linq;
using System.Reflection;

namespace Telelogos.ReportBuilder
{
   public static class Helper
   {
      /// <summary>
      /// Copie les champs du premier objet dans le second objet
      /// </summary>
      /// <typeparam name="T1">Type du premier objet</typeparam>
      /// <typeparam name="T2">Type du deuxième objet</typeparam>
      /// <param name="firstObject">Objet à copier</param>
      /// <param name="secondObject">Objet contenant la copie</param>
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
   }
}

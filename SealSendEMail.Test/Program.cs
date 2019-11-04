using Seal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telelogos.MediaContact.SharedTools;
using Telelogos.MediaContact.SharedTools.Helpers;

namespace SealSendEMail.Test
{
   class Program
   {
      static void Main(string[] args)
      {
         //var device = OutputEmailDevice.Create();
         //device.Server = "smtp.gmail.com";
         //device.UserName = "dev.telelogos@gmail.com";
         //device.Password = "!telelogos2009";
         //device.TestEmailTo = "aseguin@telelogos.com";
         //device.SenderEmail = "dev.telelogos@gmail.com";
         //device.EnableSsl = true;
         //device.Port = 465;
         //device.ReplyTo = "aseguin@telelogos.com";
         //device.SmtpClient.ServicePoint.MaxIdleTime = 1;
         //device.SmtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
         //device.SendTestEmail();

         var h = new MailHelper();
         var m = h.GetMailMessage("dev.telelogos@gmail.com", "aseguin@telelogos.com", null, null, "Test mail", "Hellop World !", null);
         var s = new Telelogos.MediaContact.SharedTools.Models.SmtpConfiguration() { Host = "smtp.gmail.com", Password = "!telelogos2009", User = "dev.telelogos@gmail.com", Port = 465, UseSecureSocket = true };
         h.SendMail(s, m);

         Console.ReadLine();
      }
   }
}

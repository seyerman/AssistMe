using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace AssistMeProject.Models
{
    public class Email
    {
        private SmtpClient Server;
        private static IConfiguration Configuration { get; set; }
        private MailMessage Msg;


        public Email()
        {
            InitConfiguration();
            Server = new SmtpClient(Configuration["host"], Int32.Parse(Configuration["port"]))
            {
                EnableSsl = Boolean.Parse(Configuration["enableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Configuration["user"], Configuration["password"])
            };
        }
        private static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddXmlFile("Configuration.xml");
            Configuration = builder.Build();
        }

        public void EnviarCorreo(string to, string tittle, string message, bool esHtml = false)
        {
            Msg = new MailMessage(Configuration["user"], to, tittle, message);
            Msg.IsBodyHtml = esHtml;
            Server.Send(Msg);
        }

        public void EnviarCorreo(MailMessage message)
        {
            Server.Send(message);
        }

        public async Task EnviarCorreoAsync(MailMessage message)
        {
            await Server.SendMailAsync(message);
        }

    }
}

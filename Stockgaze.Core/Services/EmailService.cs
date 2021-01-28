//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Essential.Templating.Razor.Email;
using Stockgaze.Core.Emails;
using Stockgaze.Core.Login;
using Stockgaze.Core.Models;
using Stockgaze.Core.WPFTools;

namespace Stockgaze.Core.Services
{

    public class EmailService
    {
        public EmailService()
        {
            EmailConfigFile = new EmailConfigFile();
            if (EmailConfigFile.FileExist)
            {
                EmailConfigFile.Load().Wait();
            }
        }

        public void SendOptionResults(string subject, string filter, List<string> emailAdresses, List<SymbolOptionModel> optionControllerOptionSymbols)
        {
            // create the message
            var msg = new MailMessage();
            msg.From = new MailAddress(EmailConfigFile.From);
            emailAdresses.ForEach(email => msg.To.Add(email));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = EmailTemplates.FillOptionTemplate(filter, optionControllerOptionSymbols);

            // configure the smtp server
            var smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(EmailConfigFile.From, EmailConfigFile.Password);
            smtp.Host = EmailConfigFile.SmtpServer;
            smtp.Port = 587;
            smtp.EnableSsl = true;
                
            
            // send the message
            try
            {
                smtp.Send(msg);
            }
            catch (Exception e)
            {
                
            }
        }

        public string SmtpServer
        {
            set => EmailConfigFile.SmtpServer = value;
        }

        public string From
        {
            set => EmailConfigFile.From = value;
        }

        public string To
        {
            set => EmailConfigFile.To = value;
        }

        public string Password
        {
            set => EmailConfigFile.Password = value;
        }

        public string Username
        {
            set => EmailConfigFile.Username = value;
        }

        public string Port
        {
            set => EmailConfigFile.Port = value;
        }

        public string Protocol
        {
            set => EmailConfigFile.Protocol = value;
        }

        public EmailConfigFile EmailConfigFile { get; set; } = new EmailConfigFile();

    }

}
﻿using System.Net.Mail;
using System.Net;
using ISAProject.Modules.Stakeholders.API.Public;

namespace ISAProject.Modules.Stakeholders.Core.UseCases
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _port = 587;
        private readonly string _email = "3ptravelserbia@gmail.com";
        private readonly string _password = "hneoqsqhudotiwoj";

        public void SendActivationEmail(string recipientEmail, string activationLink)
        {
            string link = $"http://localhost:4200/activate?token={activationLink}";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_email);
                mail.To.Add(recipientEmail);
                mail.Subject = "Account activation";
                mail.Body = $"Please click on this link to activate your account: {link}";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(_smtpServer, _port))
                {
                    smtp.Credentials = new NetworkCredential(_email, _password);
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        Console.WriteLine("Mejl je uspesno poslat!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Doslo je do greske prilikom slanja mejla: " + ex.Message);
                    }
                }
            }
        }

    }
}

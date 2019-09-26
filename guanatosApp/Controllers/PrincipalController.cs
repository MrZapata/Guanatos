using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace guanatosApp.Controllers
{
    public class PrincipalController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Enroll()
        {
            return View();
        }

        private void SendEmail()
        {

            MailMessage emailObj = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            emailObj.To.Add(new MailAddress("correo@destino.com"));
            emailObj.From = new MailAddress("webjocker2001@hotmail.com");
            emailObj.Subject = "Notificación ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            emailObj.SubjectEncoding = System.Text.Encoding.UTF8;
            emailObj.Body = "Se agregaron nuevos restaurant.";
            emailObj.IsBodyHtml = true;
            emailObj.Priority = MailPriority.Normal;

            smtp.Host = "smtp.hotmail.com"; 
            smtp.Port = 25;
            smtp.Timeout = 50;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("webjocker2001@hotmail.com", "Is50818.se");

            //implementarla funcionalidad para traer los correos
            string emails = "webjocker2001@hotmail.com";
            string output = string.Empty;

            emailObj.To.Add(emails);

            try
            {
                smtp.Send(emailObj);
                emailObj.Dispose();
            }
            catch (SmtpException exm)
            {
            }
            catch (Exception ex)
            {
            }

        }


    }
}
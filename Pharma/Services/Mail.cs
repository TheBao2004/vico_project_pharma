using System.IO;
using System.Text.Json;
using Pharma.Data;

using System;
using System.Net;
using System.Net.Mail;

namespace Pharma.Services{

     public class MailFunction
    {

        public readonly AppDbContext _context;
        public IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        private string _toUpload { get; set; }

        public MailFunction(IHttpContextAccessor accessor, AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _accessor = accessor;
            _env = env;
            _toUpload = Path.Combine(env.WebRootPath, "uploads");
        }

        public static void SendMail(){

            // MailMessage mailMessage = new MailMessage();
            // mailMessage.From = new MailAddress("nguyentheb705@gmail.com");
            // mailMessage.To.Add("nguyenthim7002@gmail.com");
            // mailMessage.Subject = "Subject";
            // mailMessage.IsBodyHtml = true;
            // mailMessage.Body = "This is test email";


            // SmtpClient smtpClient = new SmtpClient();
            // smtpClient.Host = "smtp.mywebsitedomain.com";
            // smtpClient.Port = 587;
            // smtpClient.UseDefaultCredentials = false;
            // smtpClient.Credentials = new NetworkCredential("username", "password");
            // smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            // smtpClient.EnableSsl = true;
    
            // smtpClient.Send(mailMessage);

        }


    }    


}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;  
using System.Linq;
using System.Net;
using System.Net.Mail; 
using System.Threading.Tasks;  
using System.Web;
using System.Web.Mvc;
using FIT5032_main_project.Models;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

namespace FIT5032_main_project.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult SendEmailWithAttachment()
        {
            return View();
        }


        public async Task SendEmailWithAttachmentAsync(string email, string subject, string message, Stream attachmentStream, string attachmentFilename)
        {
            var emailMessage = new IdentityMessage
            {
                Destination = email,
                Subject = subject,
                Body = message
            };

            using (var smtpClient = new SmtpClient())
            using (var msg = new MailMessage())
            {
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;

                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("samiasoudamini@gmail.com", "dkeh xzvm bppz ijhw");

                msg.To.Add(emailMessage.Destination);
                msg.Subject = emailMessage.Subject;
                msg.Body = emailMessage.Body;

                if (attachmentStream != null && !string.IsNullOrEmpty(attachmentFilename))
                {
                    msg.Attachments.Add(new System.Net.Mail.Attachment(attachmentStream, attachmentFilename));
                }

                await smtpClient.SendMailAsync(msg);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmailWithAttachment(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                Stream attachmentStream = null;
                string attachmentFilename = null;

                if (model.Attachment != null && model.Attachment.ContentLength > 0)
                {
                    attachmentStream = model.Attachment.InputStream;
                    attachmentFilename = Path.GetFileName(model.Attachment.FileName);
                }

                await SendEmailWithAttachmentAsync(model.Email, model.Subject, model.Message, attachmentStream, attachmentFilename);
                TempData["SuccessMessage"] = "Email sent successfully!";
                return RedirectToAction("SendEmailWithAttachment");
            }

            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult SendBulkEmails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendBulkEmails(BulkEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<string> emailList = model.Emails.Split(',').Select(e => e.Trim()).ToList();
                Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

                bool allValid = emailList.All(email => regex.IsMatch(email));

                if (!allValid)
                {
                    ModelState.AddModelError("Emails", "One or more email addresses are invalid.");
                    return View(model);
                }

                var apiKey = "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"; // Directly use the API key (not recommended for production)
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("samiasoudamini@gmail.com", "NexaScan");
                var subject = model.Subject;
                var tos = emailList.Select(email => new EmailAddress(email)).ToList();

                foreach (var to in tos)
                {
                    var plainTextContent = model.Message;
                    var htmlContent = $"<strong>{model.Message}</strong>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                }

                TempData["SuccessMessage"] = "Bulk emails sent successfully!";
                return RedirectToAction("SendBulkEmails");
            }

            return View(model);
        }
    }
}

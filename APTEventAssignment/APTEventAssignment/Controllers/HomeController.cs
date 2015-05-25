using APTEventAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Threading.Tasks;

namespace APTEventAssignment.Controllers
{
    public class HomeController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        public ActionResult Index()
        {
            IList<Event> eventList = new List<Event>();

            // get all the events having performance dates 
            var query = (from e in db.Event
                        join p in db.EventPerformance on e.Event_ID equals p.EventPerformance_EventID
                        orderby p.EventPerformance_Date ascending
                        select e).ToList();

            // return only one instance of the event (since one event can have many performances)
            var events = query.Distinct();

            // foreach event get the name and image
            foreach (var eventData in events)
            {
                eventList.Add(new Event()
                {
                    Event_ID = eventData.Event_ID,
                    Event_Name = eventData.Event_Name,
                    Event_Image = eventData.Event_Image
                });
            }

            return View(eventList);
        }
      
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult _Contact()
        {
            EmailFormModel efm = new EmailFormModel();
            return View(efm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("groupapt2015@gmail.com"));
                message.Subject = "Contact";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);              
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }     
    } 
}
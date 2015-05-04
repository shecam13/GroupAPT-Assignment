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

namespace APTEventAssignment.Controllers
{
    public class HomeController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        public ActionResult Index()
        {
            IList<Event> eventList = new List<Event>();

            // get the most recent events including the event name and image
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

        //public ActionResult AllEvents()
        //{
        //    // get the most recent events including the event name and image
        //    var Event = from e in db.Event
        //                join p in db.EventPerformance on e.Event_ID equals p.EventPerformance_EventID
        //                orderby p.EventPerformance_Date descending
        //                select e;

        //    //foreach ()

        //    //var Event = db.Event.Include(x => x.EventPerformance).OrderByDescending(x => x.EventPerformance_Date);
        //    return View(Event.ToList());
        //}
      
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }     
    } 
}
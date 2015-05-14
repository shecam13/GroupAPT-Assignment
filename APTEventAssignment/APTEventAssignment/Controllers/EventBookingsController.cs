using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;
using Microsoft.AspNet.Identity;
using APTEventAssignment.ViewModels;

namespace APTEventAssignment.Controllers
{
    public class EventBookingsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookings
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();
            //List<EventBooking> bookings = null;

            ////var query = ( from 
                
                
                
            ////    )
                
                
            ////    "SELECT UserName, EventBooking_Date, Event_Name, EventPerformance_Date " +
            ////            "FROM EventBooking "+
            ////            "INNER JOIN EventBooking eb "

            //if (User.IsInRole("admin"))
            //{
            //    bookings = db.EventBooking.Include(e => e.EventPerformance).ToList(); //get all evnets of all users
            //}
            //else
            //{
            //    bookings = db.EventBooking.Include(e => e.EventPerformance).Where(e => e.EventBooking_UserID == userId).ToList(); // get all event bookings of a particular user
            //}

            //return View(bookings); // return the list

            var viewmodel = (from eb in db.EventBooking
                             join pd in db.EventPerformance on eb.EventBooking_EventPerformanceID equals pd.EventPerformance_ID
                             join en in db.EventPerformance on eb.EventPerformance.EventPerformance_EventID equals en.Event.Event_ID
                             join u in db.EventBooking on eb.EventBooking_UserID equals u.AspNetUsers.Id
                             select new EventBookingViewModel()
                             {
                                 EventBooking_Date = eb.EventBooking_Date,
                                 EventBooking_ID = eb.EventBooking_ID,
                                 PerformanceDate = pd.EventPerformance_Date,
                                 EventName = en.Event.Event_Name,
                                 UserName = u.AspNetUsers.UserName

                             });

            return View(viewmodel.ToList());     
        }

        public ActionResult IndexBooking(int? id)
        {
            var userId = User.Identity.GetUserId();
            List<EventBooking> bookings = null;
            bookings = db.EventBooking.Include(e => e.EventPerformance).Where(e => e.EventBooking_UserID == userId).ToList();

            return View(bookings);
        }

        private void UpdateEventBooking(EventBooking eb, AddEventBookingViewModel addviewmodel)
        {
            eb.EventBooking_ID = addviewmodel.EventBooking_ID;
            eb.EventBooking_Date = addviewmodel.EventBooking_Date;
            eb.EventBooking_UserID = addviewmodel.EventBooking_UserID;
            eb.EventBooking_EventPerformanceID = addviewmodel.EventBooking_EventPerformanceID;
        }
        // GET: EventBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eb = db.EventBooking.Find(id);
            if (eb == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new EventBookingViewModel
            {
                EventBooking_Date = eb.EventBooking_Date,
                EventBooking_ID = eb.EventBooking_ID,
                PerformanceDate = eb.EventPerformance.EventPerformance_Date,
                EventName = eb.EventPerformance.Event.Event_Name,
                UserName = eb.AspNetUsers.UserName
            };

            return View(viewmodel);
        }


        public ActionResult Checkout()
        {
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.EventBooking.Add(eventBooking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            return View(eventBooking);
        }

        // GET: EventBookings/Create
        public ActionResult Create()
        {
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID");
            //return View();
            return View(new AddEventBookingViewModel());
        }

        // POST: EventBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // in the Bind(Include = ...) put those fields only that will be returned by the view 
        //EventBooking_ID,EventBooking_Date,EventBooking_UserID,EventBooking_EventPerformanceID,EventBooking_Deleted
        //public ActionResult Create([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        public ActionResult Create(AddEventBookingViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                //eventBooking.EventBooking_UserID = User.Identity.GetUserId();
              
                var eb = new EventBooking();

                UpdateEventBooking(eb, addviewmodel);

                db.EventBooking.Add(eb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", addviewmodel.EventBooking_EventPerformanceID);
            return View(addviewmodel);
        }

        // GET: EventBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eb = db.EventBooking.Find(id);
            if (eb == null)
            {
                return HttpNotFound();
            }

            var addviewmodel = new AddEventBookingViewModel
            {
                EventBooking_ID = eb.EventBooking_ID,
                EventBooking_Date = eb.EventBooking_Date,
                EventBooking_UserID = eb.EventBooking_UserID,
                EventBooking_EventPerformanceID = eb.EventBooking_EventPerformanceID,
            };

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", addviewmodel.EventBooking_EventPerformanceID);
            //ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login", eventBooking.EventBooking_UserID);
            return View(addviewmodel);
        }

        // POST: EventBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        public ActionResult Edit(AddEventBookingViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                //eventBooking.EventBooking_UserID = User.Identity.GetUserId();
                //db.Entry(eventBooking).State = EntityState.Modified;
                //db.SaveChanges();
                var existingBooking = db.EventBooking.Find(addviewmodel.EventBooking_ID);
                UpdateEventBooking(existingBooking, addviewmodel);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", addviewmodel.EventBooking_EventPerformanceID);

            return View(addviewmodel);
        }

        // GET: EventBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eb = db.EventBooking.Find(id);
            if (eb == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new EventBookingViewModel
            {
                EventBooking_Date = eb.EventBooking_Date,
                EventBooking_ID = eb.EventBooking_ID,
                PerformanceDate = eb.EventPerformance.EventPerformance_Date,
                EventName = eb.EventPerformance.Event.Event_Name,
                UserName = eb.AspNetUsers.UserName
            };

            return View(viewmodel);
        }

        // POST: EventBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventBooking eb = db.EventBooking.Find(id);
            db.EventBooking.Remove(eb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

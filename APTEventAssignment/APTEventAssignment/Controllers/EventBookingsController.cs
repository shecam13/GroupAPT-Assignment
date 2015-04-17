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

namespace APTEventAssignment.Controllers
{
    public class EventBookingsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookings
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<EventBooking> bookings = null;

            if (User.IsInRole("admin"))
            {
                bookings = db.EventBooking.Include(e => e.EventPerformance).ToList(); //get all evnets of all users
            }
            else
            {
                bookings = db.EventBooking.Include(e => e.EventPerformance).Where(e => e.EventBooking_UserID == userId).ToList(); // get all event bookings of a particular user
            }

            return View(bookings); // return the list
        }

        // GET: EventBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = db.EventBooking.Find(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // GET: EventBookings/Create
        public ActionResult Create()
        {
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID");
            return View();
        }

        // POST: EventBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // in the Bind(Include = ...) put those fields only that will be returned by the view 
        //EventBooking_ID,EventBooking_Date,EventBooking_UserID,EventBooking_EventPerformanceID,EventBooking_Deleted
        public ActionResult Create([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                eventBooking.EventBooking_UserID = User.Identity.GetUserId();
                db.EventBooking.Add(eventBooking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            return View(eventBooking);
        }

        // GET: EventBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = db.EventBooking.Find(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            //ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login", eventBooking.EventBooking_UserID);
            return View(eventBooking);
        }

        // POST: EventBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                eventBooking.EventBooking_UserID = User.Identity.GetUserId();
                db.Entry(eventBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);

            return View(eventBooking);
        }

        // GET: EventBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = db.EventBooking.Find(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // POST: EventBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventBooking eventBooking = db.EventBooking.Find(id);
            db.EventBooking.Remove(eventBooking);
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

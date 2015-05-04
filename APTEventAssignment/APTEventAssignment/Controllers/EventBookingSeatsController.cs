using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;
using APTEventAssignment.SeatingPlan;

namespace APTEventAssignment.Controllers
{
    public class EventBookingSeatsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookingSeats
        public ActionResult Index()
        {
            var eventBookingSeat = db.EventBookingSeat.Include(e => e.EventBooking);
            return View(eventBookingSeat.ToList());
        }
       
        public ActionResult SeatingPage()
        {
            var eventBookingSeat = db.EventBookingSeat.Include(e => e.EventBooking);
            return View(eventBookingSeat.ToList());
        }

        String[] seatsArray = new String[10];
        int number = 0;
        public void GetSeats()
        {
            WEB web = new WEB();
            //seatsArray = web.getSeats();
            seatsArray = web.getSeats();

        }

        //int venueID = 1;
        //public void GetRow()
        //{
        //    var query = (from e in db.VenueZone
        //                 where e.VenueZone_VenueID == venueID
        //                 select e.VenueZone_Rows);


        //}

        //public ActionResult ViewSeatingPlan()
        //{
        //    return View("WEB"); //Aspx file Views/Products/WebForm1.aspx
        //}

        public RedirectResult ViewSeatingPlan()
        {
            return Redirect("/SeatingPlan/WEB.aspx");
        }
       
        // GET: EventBookingSeats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBookingSeat eventBookingSeat = db.EventBookingSeat.Find(id);
            if (eventBookingSeat == null)
            {
                return HttpNotFound();
            }
            return View(eventBookingSeat);
        }

        // GET: EventBookingSeats/Create
        public ActionResult Create()
        {
            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_UserID");
            return View();
        }

        // POST: EventBookingSeats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventBookingSeat_ID,EventBookingSeat_EventBookingID,EventBookingSeat_SeatIdentifier,EventBookingSeat_Deleted")] EventBookingSeat eventBookingSeat)
        {
            if (ModelState.IsValid)
            {
                db.EventBookingSeat.Add(eventBookingSeat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_UserID", eventBookingSeat.EventBookingSeat_EventBookingID);
            return View(eventBookingSeat);
        }

        // GET: EventBookingSeats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBookingSeat eventBookingSeat = db.EventBookingSeat.Find(id);
            if (eventBookingSeat == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_UserID", eventBookingSeat.EventBookingSeat_EventBookingID);
            return View(eventBookingSeat);
        }

        // POST: EventBookingSeats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventBookingSeat_ID,EventBookingSeat_EventBookingID,EventBookingSeat_SeatIdentifier,EventBookingSeat_Deleted")] EventBookingSeat eventBookingSeat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventBookingSeat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_UserID", eventBookingSeat.EventBookingSeat_EventBookingID);
            return View(eventBookingSeat);
        }

        // GET: EventBookingSeats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBookingSeat eventBookingSeat = db.EventBookingSeat.Find(id);
            if (eventBookingSeat == null)
            {
                return HttpNotFound();
            }
            return View(eventBookingSeat);
        }

        // POST: EventBookingSeats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventBookingSeat eventBookingSeat = db.EventBookingSeat.Find(id);
            db.EventBookingSeat.Remove(eventBookingSeat);
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

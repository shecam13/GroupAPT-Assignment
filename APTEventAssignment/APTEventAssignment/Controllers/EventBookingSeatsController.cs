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
using APTEventAssignment.ViewModels;

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

        int venueID = 1;
        String venueCode = "L";

        List<VenueZone> venueZones = null;

        public List<SelectListItem> GetPerformances(List<EventPerformance> performances)
        {
            //SelectListItem performanceListItem = new SelectListItem();
            List<SelectListItem> performanceList = new List<SelectListItem>();

            if (performances.Count == 0)
            {
                return performanceList = null;
            }
            else
            {
                foreach (var p in performances)
                {
                    SelectListItem performanceListItem = new SelectListItem { Value = p.EventPerformance_ID.ToString(), Text = p.EventPerformance_Date.ToString() };
                    //performanceListItem = new SelectListItem { 0, "Hello"};
                    performanceList.Add(performanceListItem);
                }

                //performanceList = new SelectList((SelectListItem) performanceListItem, "Value", "Text");

                return performanceList;
            }
            
        }


        public ActionResult SeatingPage()
        {
            var eventDetails = this.Session["EventDetails"] as EventsDetailsViewModel;
            List<EventPerformance> performances = eventDetails.Event_Performances;

            var viewmodel = new EventBookingSeatsViewModel
            {
                Performances = GetPerformances(performances)
            };

            //List<DateTime> dates = new List<DateTime>();
            
            //foreach (var p in performances)
            //{
            //    dates.Add((DateTime)p.EventPerformance_Date);
            //}



            //var viewmodel = new EventBookingSeatsViewModel
            //{
                
            //    Performances = new SelectList(dates)
            //};

            //List<String> rows = new List<String>();
            //List<String> cols = new List<String>();
            //List<String> codes = new List<String>();

            //var query = (from e in db.VenueZone
            //             where e.VenueZone_VenueID == venueID
            //             select e);

            //venueZones = query.ToList();

            //foreach (var v in venueZones)
            //{
            //    rows.Add(v.VenueZone_Rows.ToString());
            //    cols.Add(v.VenueZone_Rows.ToString());
            //    codes.Add(v.VenueZone_Rows.ToString());
            //}

            //WEB web = new WEB();
            //web.setZoneDetails(rows, cols, codes);
            
            //ViewBag.venueZones = query.ToList();

            //var eventBookingSeat = db.EventBookingSeat.Include(e => e.EventBooking);
            //return View(eventBookingSeat.ToList());
            //ViewBag.date = dates;
            
            return View(viewmodel);

        }

        //String[] seatsArray = new String[10];
        //int number = 0;
        //public void GetSeats()
        //{
        //    WEB web = new WEB();
        //    //seatsArray = web.getSeats();
        //    seatsArray = web.getSeats();

        //}

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

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
    [Authorize]
    public class EventBookingSeatsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookingSeats
        public ActionResult Index()
        {
            var eventBookingSeat = db.EventBookingSeat.Include(e => e.EventBooking);
            return View(eventBookingSeat.ToList());
        }

        //int venueID = 1;
        //String venueCode = "L";
        //List<VenueZone> venueZones = null;

        public List<SelectListItem> GetPerformances(List<EventPerformance> performances)
        {
            List<SelectListItem> performanceList = new List<SelectListItem>();

            if (performances.Count == 0)
            {
                return performanceList = null;
            }
            else
            {
                foreach (var p in performances)
                {
                    SelectListItem performanceListItem = new SelectListItem { Value = p.EventPerformance_ID.ToString(), Text = p.EventPerformance_Date.ToShortDateString() };
                                       
                    performanceList.Add(performanceListItem);
                }

                return performanceList;
            }
            
        }

        enum Seating
        {
            Code = 0,
            Title = 1,
            Rows = 2,
            Columns = 3
        }

        public Array getSeatingBookedForShow(long ShowID)
        {
            string[] arrBookings = new string[51];

            arrBookings[0] = "L002";
            arrBookings[1] = "L071";
            arrBookings[2] = "M122";
            arrBookings[3] = "M093";
            arrBookings[4] = "M094";
            arrBookings[5] = "M095";
            arrBookings[6] = "R000";

            return arrBookings;
        }

        public int getSeatingZoneCountForShow(string Code)
        {
            int intZoneCount = 0;

            switch (Code)
            {
                case "A":
                    intZoneCount = 3;
                    break;
                case "B":
                    intZoneCount = 2;
                    break;
                case "C":
                    intZoneCount = 4;
                    break;
            }
            return intZoneCount;
        }


        public Array getSeatingPlanForShow(string Code)
        {
            int intZoneCount = 0;
            string[,] arrSeating = new string[1, 6];
            string[,] arrBookedSeats = new string[1, 3];

            switch (Code)
            {
                case "A":
                    //Define the Number of Vertical Zone 
                    intZoneCount = 3;
                    //Create the Array with the Zone Information
                    arrSeating = new string[intZoneCount, 6];
                    arrSeating[0, (int)Seating.Code] = "L";
                    arrSeating[0, (int)Seating.Title] = "LEFT WING";
                    arrSeating[0, (int)Seating.Rows] = "10";
                    arrSeating[0, (int)Seating.Columns] = "5";

                    arrSeating[1, (int)Seating.Code] = "M";
                    arrSeating[1, (int)Seating.Title] = "MIDDLE";
                    arrSeating[1, (int)Seating.Rows] = "10";
                    arrSeating[1, (int)Seating.Columns] = "15";

                    arrSeating[2, (int)Seating.Code] = "R";
                    arrSeating[2, (int)Seating.Title] = "RIGHT WING";
                    arrSeating[2, (int)Seating.Rows] = "10";
                    arrSeating[2, (int)Seating.Columns] = "5";
                    break;
                case "B":

                    break;
                case "C":

                    break;

            }
            return arrSeating;
        }

        public ActionResult SeatingPage(string Code)
        {
            if (Session["EventDetails"] != null)
            {
                var eventDetails = this.Session["EventDetails"] as EventsDetailsViewModel;
                List<EventPerformance> performances = eventDetails.Event_Performances;

                var viewmodel = new EventBookingSeatsViewModel
                {
                    Event_Name = eventDetails.Event_Name,
                    Event_VenueName = eventDetails.Event_VenueName,
                    Performances = GetPerformances(performances)
                };

                ViewData["PerformanceList"] = performances;

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

                //for seating
                ViewBag.Zones = getSeatingZoneCountForShow("A");
                ViewBag.Layout = getSeatingPlanForShow("A");
                ViewBag.Booked = getSeatingBookedForShow(1);


                return View(viewmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]   
        [ValidateAntiForgeryToken]
        public ActionResult SeatingPage(EventBookingSeatsViewModel viewmodel)
        {
            //IEnumerable<SelectListItem> item = db.EventPerformance.Select(x => new SelectListItem{ Text = x.EventPerformance_Date.ToString() }).Where(?? == viewmodel.SelectPerformanceId);
            var eventDetails = this.Session["EventDetails"] as EventsDetailsViewModel;
            List<EventPerformance> performances = eventDetails.Event_Performances;

            viewmodel.Performances = GetPerformances(performances);           

            // pass the view model to the index booking page
            this.Session["BookingDetails"] = viewmodel;

            return RedirectToAction("IndexBooking", "EventBookings");
            //return View(viewmodel);
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

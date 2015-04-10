using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;

namespace APTEventAssignment.Controllers
{
    public class EventBookingSeatsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookingSeats
        public async Task<ActionResult> Index()
        {
            var eventBookingSeat = db.EventBookingSeat.Include(e => e.EventBooking);
            return View(await eventBookingSeat.ToListAsync());
        }

        // GET: EventBookingSeats/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBookingSeat eventBookingSeat = await db.EventBookingSeat.FindAsync(id);
            if (eventBookingSeat == null)
            {
                return HttpNotFound();
            }
            return View(eventBookingSeat);
        }

        // GET: EventBookingSeats/Create
        public ActionResult Create()
        {
            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_ID");
            return View();
        }

        // POST: EventBookingSeats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventBookingSeat_ID,EventBookingSeat_EventBookingID,EventBookingSeat_SeatIdentifier,EventBookingSeat_Deleted")] EventBookingSeat eventBookingSeat)
        {
            if (ModelState.IsValid)
            {
                db.EventBookingSeat.Add(eventBookingSeat);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_ID", eventBookingSeat.EventBookingSeat_EventBookingID);
            return View(eventBookingSeat);
        }

        // GET: EventBookingSeats/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBookingSeat eventBookingSeat = await db.EventBookingSeat.FindAsync(id);
            if (eventBookingSeat == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_ID", eventBookingSeat.EventBookingSeat_EventBookingID);
            return View(eventBookingSeat);
        }

        // POST: EventBookingSeats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventBookingSeat_ID,EventBookingSeat_EventBookingID,EventBookingSeat_SeatIdentifier,EventBookingSeat_Deleted")] EventBookingSeat eventBookingSeat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventBookingSeat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventBookingSeat_EventBookingID = new SelectList(db.EventBooking, "EventBooking_ID", "EventBooking_ID", eventBookingSeat.EventBookingSeat_EventBookingID);
            return View(eventBookingSeat);
        }

        // GET: EventBookingSeats/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBookingSeat eventBookingSeat = await db.EventBookingSeat.FindAsync(id);
            if (eventBookingSeat == null)
            {
                return HttpNotFound();
            }
            return View(eventBookingSeat);
        }

        // POST: EventBookingSeats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventBookingSeat eventBookingSeat = await db.EventBookingSeat.FindAsync(id);
            db.EventBookingSeat.Remove(eventBookingSeat);
            await db.SaveChangesAsync();
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

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
    public class EventBookingsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookings
        public async Task<ActionResult> Index()
        {
            var eventBooking = db.EventBooking.Include(e => e.EventPerformance).Include(e => e.User);
            return View(await eventBooking.ToListAsync());
        }

        // GET: EventBookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = await db.EventBooking.FindAsync(id);
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
            ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login");
            return View();
        }

        // POST: EventBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventBooking_ID,EventBooking_Date,EventBooking_UserID,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.EventBooking.Add(eventBooking);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login", eventBooking.EventBooking_UserID);
            return View(eventBooking);
        }

        // GET: EventBookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = await db.EventBooking.FindAsync(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login", eventBooking.EventBooking_UserID);
            return View(eventBooking);
        }

        // POST: EventBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventBooking_ID,EventBooking_Date,EventBooking_UserID,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventBooking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login", eventBooking.EventBooking_UserID);
            return View(eventBooking);
        }

        // GET: EventBookings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = await db.EventBooking.FindAsync(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // POST: EventBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventBooking eventBooking = await db.EventBooking.FindAsync(id);
            db.EventBooking.Remove(eventBooking);
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

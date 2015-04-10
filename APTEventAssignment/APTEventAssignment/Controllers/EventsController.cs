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
    public class EventsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: Events
        public async Task<ActionResult> Index()
        {
            var Event = db.Event.Include(E => E.Venue);  //changed event to Event and (@ => @.Venue) to (E => E.Venue) --- orgininal: var event = db.event.Include(@ => @.Venue);  --- i think event is a keyword and so was why it didn't work. 
            return View(await Event.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Event.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Event.Add(@event);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", @event.Event_VenueID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Event.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", @event.Event_VenueID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", @event.Event_VenueID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Event.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Event @event = await db.Event.FindAsync(id);
            db.Event.Remove(@event);
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

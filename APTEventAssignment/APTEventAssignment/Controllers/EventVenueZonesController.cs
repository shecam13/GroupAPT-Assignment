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
    public class EventVenueZonesController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventVenueZones
        public async Task<ActionResult> Index()
        {
            var eventVenueZone = db.EventVenueZone.Include(e => e.Event).Include(e => e.VenueZone);
            return View(await eventVenueZone.ToListAsync());
        }

        // GET: EventVenueZones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventVenueZone eventVenueZone = await db.EventVenueZone.FindAsync(id);
            if (eventVenueZone == null)
            {
                return HttpNotFound();
            }
            return View(eventVenueZone);
        }

        // GET: EventVenueZones/Create
        public ActionResult Create()
        {
            ViewBag.EventVenueZone_EventID = new SelectList(db.Event, "Event_ID", "Event_Name");
            ViewBag.EventVenueZone_VenueZoneID = new SelectList(db.VenueZone, "VenueZone_ID", "VenueZone_Name");
            return View();
        }

        // POST: EventVenueZones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventVenueZone_ID,EventVenueZone_EventID,EventVenueZone_VenueZoneID,EventVenueZone_Price,EventVenueZone_Deleted")] EventVenueZone eventVenueZone)
        {
            if (ModelState.IsValid)
            {
                db.EventVenueZone.Add(eventVenueZone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventVenueZone_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", eventVenueZone.EventVenueZone_EventID);
            ViewBag.EventVenueZone_VenueZoneID = new SelectList(db.VenueZone, "VenueZone_ID", "VenueZone_Name", eventVenueZone.EventVenueZone_VenueZoneID);
            return View(eventVenueZone);
        }

        // GET: EventVenueZones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventVenueZone eventVenueZone = await db.EventVenueZone.FindAsync(id);
            if (eventVenueZone == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventVenueZone_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", eventVenueZone.EventVenueZone_EventID);
            ViewBag.EventVenueZone_VenueZoneID = new SelectList(db.VenueZone, "VenueZone_ID", "VenueZone_Name", eventVenueZone.EventVenueZone_VenueZoneID);
            return View(eventVenueZone);
        }

        // POST: EventVenueZones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventVenueZone_ID,EventVenueZone_EventID,EventVenueZone_VenueZoneID,EventVenueZone_Price,EventVenueZone_Deleted")] EventVenueZone eventVenueZone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventVenueZone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventVenueZone_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", eventVenueZone.EventVenueZone_EventID);
            ViewBag.EventVenueZone_VenueZoneID = new SelectList(db.VenueZone, "VenueZone_ID", "VenueZone_Name", eventVenueZone.EventVenueZone_VenueZoneID);
            return View(eventVenueZone);
        }

        // GET: EventVenueZones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventVenueZone eventVenueZone = await db.EventVenueZone.FindAsync(id);
            if (eventVenueZone == null)
            {
                return HttpNotFound();
            }
            return View(eventVenueZone);
        }

        // POST: EventVenueZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventVenueZone eventVenueZone = await db.EventVenueZone.FindAsync(id);
            db.EventVenueZone.Remove(eventVenueZone);
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

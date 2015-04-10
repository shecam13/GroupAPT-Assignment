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
    public class VenuesController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: Venues
        public async Task<ActionResult> Index()
        {
            var venue = db.Venue.Include(v => v.VenueType);
            return View(await venue.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = await db.Venue.FindAsync(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            return View(venue);
        }

        // GET: Venues/Create
        public ActionResult Create()
        {
            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name");
            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Venue_ID,Venue_Name,Venue_VenueTypeID,Venue_Capacity,Venue_Address,Venue_Deleted")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                db.Venue.Add(venue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", venue.Venue_VenueTypeID);
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = await db.Venue.FindAsync(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", venue.Venue_VenueTypeID);
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Venue_ID,Venue_Name,Venue_VenueTypeID,Venue_Capacity,Venue_Address,Venue_Deleted")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", venue.Venue_VenueTypeID);
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = await db.Venue.FindAsync(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Venue venue = await db.Venue.FindAsync(id);
            db.Venue.Remove(venue);
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

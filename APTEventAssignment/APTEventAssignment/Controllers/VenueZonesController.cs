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
    public class VenueZonesController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: VenueZones
        public async Task<ActionResult> Index()
        {
            var venueZone = db.VenueZone.Include(v => v.Venue);
            return View(await venueZone.ToListAsync());
        }

        // GET: VenueZones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueZone venueZone = await db.VenueZone.FindAsync(id);
            if (venueZone == null)
            {
                return HttpNotFound();
            }
            return View(venueZone);
        }

        // GET: VenueZones/Create
        public ActionResult Create()
        {
            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name");
            return View();
        }

        // POST: VenueZones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VenueZone_ID,VenueZone_Name,VenueZone_VenueID,VenueZone_Code,VenueZone_Rows,VenueZone_Columns,VenueZone_Deleted")] VenueZone venueZone)
        {
            if (ModelState.IsValid)
            {
                db.VenueZone.Add(venueZone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", venueZone.VenueZone_VenueID);
            return View(venueZone);
        }

        // GET: VenueZones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueZone venueZone = await db.VenueZone.FindAsync(id);
            if (venueZone == null)
            {
                return HttpNotFound();
            }
            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", venueZone.VenueZone_VenueID);
            return View(venueZone);
        }

        // POST: VenueZones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VenueZone_ID,VenueZone_Name,VenueZone_VenueID,VenueZone_Code,VenueZone_Rows,VenueZone_Columns,VenueZone_Deleted")] VenueZone venueZone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venueZone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", venueZone.VenueZone_VenueID);
            return View(venueZone);
        }

        // GET: VenueZones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueZone venueZone = await db.VenueZone.FindAsync(id);
            if (venueZone == null)
            {
                return HttpNotFound();
            }
            return View(venueZone);
        }

        // POST: VenueZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VenueZone venueZone = await db.VenueZone.FindAsync(id);
            db.VenueZone.Remove(venueZone);
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

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
    public class EventPerformancesController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventPerformances
        public async Task<ActionResult> Index()
        {
            var eventPerformance = db.EventPerformance.Include(e => e.Event);
            return View(await eventPerformance.ToListAsync());
        }

        // GET: EventPerformances/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventPerformance eventPerformance = await db.EventPerformance.FindAsync(id);
            if (eventPerformance == null)
            {
                return HttpNotFound();
            }
            return View(eventPerformance);
        }

        // GET: EventPerformances/Create
        public ActionResult Create()
        {
            ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name");
            return View();
        }

        // POST: EventPerformances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EventPerformance_ID,EventPerformance_EventID,EventPerformance_Date,EventPerformance_Time,EventPerformance_Deleted")] EventPerformance eventPerformance)
        {
            if (ModelState.IsValid)
            {
                db.EventPerformance.Add(eventPerformance);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", eventPerformance.EventPerformance_EventID);
            return View(eventPerformance);
        }

        // GET: EventPerformances/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventPerformance eventPerformance = await db.EventPerformance.FindAsync(id);
            if (eventPerformance == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", eventPerformance.EventPerformance_EventID);
            return View(eventPerformance);
        }

        // POST: EventPerformances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EventPerformance_ID,EventPerformance_EventID,EventPerformance_Date,EventPerformance_Time,EventPerformance_Deleted")] EventPerformance eventPerformance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventPerformance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", eventPerformance.EventPerformance_EventID);
            return View(eventPerformance);
        }

        // GET: EventPerformances/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventPerformance eventPerformance = await db.EventPerformance.FindAsync(id);
            if (eventPerformance == null)
            {
                return HttpNotFound();
            }
            return View(eventPerformance);
        }

        // POST: EventPerformances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EventPerformance eventPerformance = await db.EventPerformance.FindAsync(id);
            db.EventPerformance.Remove(eventPerformance);
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

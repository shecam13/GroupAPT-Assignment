using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult Index()
        {
            var Event = db.Event.Include(E => E.Venue).Include(E => E.Category);
            return View(Event.ToList());
        }

        public ActionResult EventsDetailsPage(int? id)
        {
            //var Event = db.Event.Include(E => E.Venue).Include(E => E.Category);
            //return View(Event.ToList());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
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
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted,Event_CategoryID,Event_Image")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Event.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", @event.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", @event.Event_CategoryID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", @event.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", @event.Event_CategoryID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted,Event_CategoryID,Event_Image")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", @event.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", @event.Event_CategoryID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Event.Find(id);
            db.Event.Remove(@event);
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

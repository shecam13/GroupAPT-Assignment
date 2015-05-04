using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;
using APTEventAssignment.ViewModels;

namespace APTEventAssignment.Controllers
{
    public class EventsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: Events
        public ActionResult Index()
        {
            //var Event = db.Event.Include(E => E.Venue).Include(E => E.Category);
            //return View(Event.ToList());

            var viewmodel = (from e in db.Event
                             join cid in db.Category on e.Event_CategoryID equals cid.Category_ID
                             join vt in db.Venue on e.Event_VenueID equals vt.Venue_ID
                             select new EventsViewModel()
                             {
                                 Event_ID = e.Event_ID,
                                 Event_Name = e.Event_Name,
                                 Event_VenueName = vt.Venue_Name,
                                 Event_Rating = e.Event_Rating,
                                 Event_CategoryName = cid.Category_Name,
                             });

            return View(viewmodel.ToList());
        }
        private void UpdateEvent(Event e, AddEventViewModel addviewmodel)
        {
            e.Event_ID = addviewmodel.Event_ID;
            e.Event_Name = addviewmodel.Event_Name;
            e.Event_VenueID = addviewmodel.Event_VenueID;
            e.Event_Rating = addviewmodel.Event_Rating;
            e.Event_CategoryID = addviewmodel.Event_CategoryID;
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event e = db.Event.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new EventsViewModel
            {
                Event_ID = e.Event_ID,
                Event_Name = e.Event_Name,
                Event_VenueName = e.Venue.Venue_Name,
                Event_Rating = e.Event_Rating,
                Event_CategoryName = e.Category.Category_Name,
            };
            return View(viewmodel);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name");
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name");
            return View(new AddEventViewModel());
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted,Event_CategoryID,Image")] Event @event)
        public ActionResult Create(AddEventViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                var e = new Event();

                UpdateEvent(e, addviewmodel);

                db.Event.Add(e);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", addviewmodel.Event_CategoryID);
            return View(addviewmodel);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event e = db.Event.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }

            var addviewmodel = new AddEventViewModel
            {
                Event_ID = e.Event_ID,
                Event_Name = e.Event_Name,
                Event_VenueID = e.Event_VenueID,
                Event_Rating = e.Event_Rating,
                Event_CategoryID = e.Event_CategoryID,
            };

            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", e.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", e.Event_CategoryID);
            return View(addviewmodel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddEventViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                var existingEvent = db.Event.Find(addviewmodel.Event_ID);
                UpdateEvent(existingEvent, addviewmodel);

                //db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", addviewmodel.Event_CategoryID);
            return View(addviewmodel);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event e = db.Event.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new EventsViewModel
            {
                Event_ID = e.Event_ID,
                Event_Name = e.Event_Name,
                Event_VenueName = e.Venue.Venue_Name,
                Event_Rating = e.Event_Rating,
                Event_CategoryName = e.Category.Category_Name,
            };
            return View(viewmodel);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event e = db.Event.Find(id);
            db.Event.Remove(e);
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

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
using System.Web.Helpers;
using System.IO;

namespace APTEventAssignment.Controllers
{
    public class EventsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        [Authorize(Roles = "Admin")]
        // GET: Events
        public ActionResult Index(string SearchEvent)
        {
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
                             }).Distinct();


            if (!String.IsNullOrEmpty(SearchEvent))
            {
                viewmodel = viewmodel.Where(s => s.Event_Name.Contains(SearchEvent));
            }

            return View(viewmodel);
            
        }

        // GET: Events
        public ActionResult EventSearchPage(string search, string category)
        {
            var CategoryList = new List<string>();

            var CatQuery = from d in db.Category
                           orderby d.Category_Name
                           select d.Category_Name;

            CategoryList.AddRange(CatQuery.Distinct());
            ViewBag.Category = new SelectList(CategoryList);


            //querying the db to get all data for events 
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
                                    Event_Image = e.Event_Image,
                                });


            if (!String.IsNullOrEmpty(search))
            {
                viewmodel = viewmodel.Where(s => s.Event_Name.Contains(search));
                    
            }

            if (!string.IsNullOrEmpty(category))
            {
                viewmodel = viewmodel.Where(x => x.Event_CategoryName == category);
            }

            return View(viewmodel); 

        }


        //method to update the model with the viewmodel data. Used in create and edit methods. 
        private void UpdateEvent(Event e, AddEventViewModel addviewmodel)
        {
            e.Event_ID = addviewmodel.Event_ID;
            e.Event_Name = addviewmodel.Event_Name;
            e.Event_VenueID = addviewmodel.Event_VenueID;
            e.Event_Rating = addviewmodel.Event_Rating;
            e.Event_CategoryID = addviewmodel.Event_CategoryID;
            e.Event_Image = addviewmodel.Event_Image;
        }

        public ActionResult EventsDetailsPage(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get event according to the id passed
            Event @event = db.Event.Find(id);

            if (@event == null)
            {
                return HttpNotFound();
            }

            //get category name
            var category = db.Category.FirstOrDefault(c => c.Category_ID == @event.Event_CategoryID);
            var categoryName = category.Category_Name;

            // get venue name
            var venue = db.Venue.FirstOrDefault(c => c.Venue_ID == @event.Event_VenueID);
            var venueName = venue.Venue_Name;

            //get prices
            //var price = "€0.00";
            //var eventVenueZones = db.EventVenueZone.FirstOrDefault(c => c.EventVenueZone_EventID == @event.Event_ID);
            //price = eventVenueZones.EventVenueZone_Price.ToString();

            List<EventPerformance> performances = null;

            // get the list of performances for a particular event
            var query = from ep in db.EventPerformance
                        where ep.EventPerformance_EventID == id
                        orderby ep.EventPerformance_Date ascending
                        select ep;

            performances = query.ToList();

            if (@event.Event_Rating == null)
            {
                @event.Event_Rating = "N/A";
            };

            var viewmodel = new EventsDetailsViewModel
            {
                Event_Name = @event.Event_Name,
                Event_VenueName = venueName,
                Event_Image = @event.Event_Image,
                Event_Rating = @event.Event_Rating,
                Event_CategoryName = categoryName,
               // Event_Price = price
            };

            viewmodel.Event_Performances = performances;

            // pass the view model to the seating page
            this.Session["EventDetails"] = viewmodel;

            return View(viewmodel);
        }


        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event e = db.Event.Find(id); //find an event from the database with the ID passed in the parameters 
            if (e == null)
            {
                return HttpNotFound();
            }

            
            List<EventPerformance> performances = null;

            // get the list of performances for a particular event
            var query = from ep in db.EventPerformance
                        where ep.EventPerformance_EventID == id
                        select ep;

            performances = query.ToList();

            //set the viewmodel properties to the data received from the db 

            var viewmodel = new EventsViewModel
            {
                Event_ID = e.Event_ID,
                Event_Name = e.Event_Name,
                Event_VenueID = e.Event_VenueID,
                Event_VenueName = e.Venue.Venue_Name,
                Event_Rating = e.Event_Rating,
                Event_CategoryID = e.Event_CategoryID,
                Event_CategoryName = e.Category.Category_Name,
            };

            viewmodel.Event_Performances = performances;

            return View(viewmodel);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name");
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name");
            return View(new AddEventViewModel());
        }


        [HttpPost]
        public ActionResult Create(AddEventViewModel addviewmodel)
        {


            if (ModelState.IsValid)
            {
                string filename = "";
                byte[] bytes;
                int BytestoRead;
                int numBytesRead;

                if (addviewmodel.Upload != null)
                {

                    filename = Path.GetFileName(addviewmodel.Upload.FileName); //get the filename/path of the upload image 
                    bytes = new byte[addviewmodel.Upload.ContentLength]; //gets the size of the uploaded file in bytes. 
                    BytestoRead = (int)addviewmodel.Upload.ContentLength; //typecasting the byte size to an int
                    numBytesRead = 0;

                    while (BytestoRead > 0)
                    {
                        int n = addviewmodel.Upload.InputStream.Read(bytes, numBytesRead, BytestoRead); //reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read. 
                        if (n == 0) break;

                        numBytesRead += n;
                        BytestoRead -= n;
                    }

                    addviewmodel.Event_Image = bytes; //making the Event_Image attributes equal to the variable bytes.  
                }

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

            //setting the addviewmodel properties to those of the event with the ID found in db 

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddEventViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                var existingEvent = db.Event.Find(addviewmodel.Event_ID);

                UpdateEvent(existingEvent, addviewmodel);

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

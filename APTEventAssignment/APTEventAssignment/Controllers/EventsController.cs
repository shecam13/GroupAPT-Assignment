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
            //var Event = db.Event.Include(E => E.Venue).Include(E => E.Category);
            //return View(Event.ToList());

            var viewmodel = (from e in db.Event 
                             join cid in db.Category on e.Event_CategoryID equals cid.Category_ID
                             join vt in db.Venue on e.Event_VenueID equals vt.Venue_ID
                             //join ep in db.EventPerformance on e.Event_ID equals ep.EventPerformance_EventID
                             //join en in db.Event on ep.EventPerformance_EventID equals en.Event_ID
                             select new EventsViewModel()
                             {
                                 Event_ID = e.Event_ID,
                                 Event_Name = e.Event_Name,
                                 Event_VenueName = vt.Venue_Name,
                                 Event_Rating = e.Event_Rating,
                                 Event_CategoryName = cid.Category_Name,
                                 //EventPerformance_ID = ep.EventPerformance_ID,
                                 //EventPerformance_Date = ep.EventPerformance_Date,
                                 //EventPerformance_Time = ep.EventPerformance_Time,
                                 //EventPerformance_EventID = en.Event_ID,
                             }).Distinct();

            //viewmodel.Distinct().OrderBy(item => item.Event_ID).First();
;


            if (!String.IsNullOrEmpty(SearchEvent))
            {
                viewmodel = viewmodel.Where(s => s.Event_Name.Contains(SearchEvent));
            }

            return View(viewmodel);
            //return View(viewmodel.ToList());
            
        }

        // GET: Events
        public ActionResult EventSearchPage(string search, string category)
        {


            var CategoryList = new List<string>();

            var CatQuery = from d in db.Category
                           orderby d.Category_Name
                           select d.Category_Name;

            CategoryList.AddRange(CatQuery.Distinct());
            ViewBag.Genre = new SelectList(CategoryList);


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
            
        

        private void UpdateEvent(Event e, AddEventViewModel addviewmodel)
        {
            e.Event_ID = addviewmodel.Event_ID;
            e.Event_Name = addviewmodel.Event_Name;
            e.Event_VenueID = addviewmodel.Event_VenueID;
            e.Event_Rating = addviewmodel.Event_Rating;
            e.Event_CategoryID = addviewmodel.Event_CategoryID;
            e.Event_Image = addviewmodel.Event_Image;
            //ep.EventPerformance_EventID = addviewmodel.Event_ID;
            //ep.EventPerformance_ID = addviewmodel.EventPerformance_ID;
            //ep.EventPerformance_Date = (DateTime)addviewmodel.EventPerformance_Date;
            //ep.EventPerformance_Time = (TimeSpan)addviewmodel.EventPerformance_Time;
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
                //Event_VenueID = @event.Event_ID,
                Event_Rating = @event.Event_Rating,
                //Event_CategoryID = @event.Event_CategoryID,
                Event_CategoryName = categoryName
            };

            viewmodel.Event_Performances = performances;

            // pass the view model to the seating page
            this.Session["EventDetails"] = viewmodel;

            return View(viewmodel);
        }

        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{

        //    if (file != null)
        //    {
        //        string ImageName = System.IO.Path.GetFileName(file.FileName);
        //        string physicalPath = Server.MapPath("~/images/" + ImageName);

        //        // save image in folder
        //        file.SaveAs(physicalPath);

        //        //save new record in database
        //        Event newRecord = new Event();
        //        newRecord.Event_Image = ImageName;
        //        db.Event.Add(newRecord);
        //        db.SaveChanges();

        //    }

        //    Display records
        //    return RedirectToAction("../home/Display/");
        //}

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event e = db.Event.Find(id);
            //EventPerformance ep = db.EventPerformance.Find(idEP);
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

            var viewmodel = new EventsViewModel
            {
                Event_ID = e.Event_ID,
                Event_Name = e.Event_Name,
                Event_VenueID = e.Event_VenueID,
                Event_VenueName = e.Venue.Venue_Name,
                Event_Rating = e.Event_Rating,
                Event_CategoryID = e.Event_CategoryID,
                Event_CategoryName = e.Category.Category_Name,
                //EventPerformance_EventID = ep.Event.Event_ID,
                //EventPerformance_ID = ep.EventPerformance_ID,
                //EventPerformance_Date = ep.EventPerformance_Date,
                //EventPerformance_Time = ep.EventPerformance_Time
            };

            viewmodel.Event_Performances = performances;

            return View(viewmodel);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name");
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name");
            //ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name");
            return View(new AddEventViewModel());
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public ActionResult Create([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted,Event_CategoryID,Image")] Event @event)
        //public ActionResult Create(AddEventViewModel addviewmodel, HttpPostedFileBase file)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var e = new Event();

        //        UpdateEvent(e, addviewmodel);

        //        db.Event.Add(e);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.Event_VenueID);
        //    ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", addviewmodel.Event_CategoryID);
        //    return View(addviewmodel);
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Event_ID,Event_Name,Event_VenueID,Event_Rating,Event_Deleted,Event_CategoryID,Image")] Event @event)
        public ActionResult Create(AddEventViewModel addviewmodel)//, HttpPostedFileBase file)
        {
            //var id = addviewmodel.Event_Image;
            //if (Request.Files.Count == 0)
            //{
            //    return View();
            //}

            if (ModelState.IsValid)
            {
                string filename = "";
                byte[] bytes;
                int BytestoRead;
                int numBytesRead;

                if (addviewmodel.Upload != null)
                {

                    filename = Path.GetFileName(addviewmodel.Upload.FileName);
                    bytes = new byte[addviewmodel.Upload.ContentLength];
                    BytestoRead = (int)addviewmodel.Upload.ContentLength;
                    numBytesRead = 0;

                    while (BytestoRead > 0)
                    {
                        int n = addviewmodel.Upload.InputStream.Read(bytes, numBytesRead, BytestoRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        BytestoRead -= n;
                    }

                    addviewmodel.Event_Image = bytes;
                }

                //db.Event.Add(addviewmodel);
                //db.SaveChanges();
                var e = new Event();
                var ep = new EventPerformance();

                UpdateEvent(e, addviewmodel);

                db.Event.Add(e);
                //db.EventPerformance.Add(ep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", addviewmodel.Event_CategoryID);
            //ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", addviewmodel.EventPerformance_EventID);
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
            //EventPerformance ep = db.EventPerformance.Find(id);
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
                //EventPerformance_EventID = ep.Event.Event_ID,
                //EventPerformance_ID = ep.EventPerformance_ID,
                //EventPerformance_Date = ep.EventPerformance_Date,
                //EventPerformance_Time = ep.EventPerformance_Time,
            };

            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", e.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", e.Event_CategoryID);
            //ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", ep.EventPerformance_EventID);
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
                //var existingEP = db.EventPerformance.Find(addviewmodel.EventPerformance_ID);

                UpdateEvent(existingEvent, addviewmodel);

                //db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Event_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.Event_VenueID);
            ViewBag.Event_CategoryID = new SelectList(db.Category, "Category_ID", "Category_Name", addviewmodel.Event_CategoryID);
            //ViewBag.EventPerformance_EventID = new SelectList(db.Event, "Event_ID", "Event_Name", addviewmodel.EventPerformance_EventID);
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
            //EventPerformance ep = db.EventPerformance.Find(id);
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
                //EventPerformance_EventID = ep.Event.Event_ID,
                //EventPerformance_ID = ep.EventPerformance_ID,
                //EventPerformance_Date = ep.EventPerformance_Date,
                //EventPerformance_Time = ep.EventPerformance_Time,
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

            //EventPerformance ep = db.EventPerformance.Find(id);
            //db.EventPerformance.Remove(ep);

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

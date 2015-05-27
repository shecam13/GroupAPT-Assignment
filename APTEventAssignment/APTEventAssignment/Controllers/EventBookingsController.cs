using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;
using Microsoft.AspNet.Identity;
using APTEventAssignment.ViewModels;
using System.Net.Mail;
//using APTEventAssignment.Message;
//using APTEventAssignment.Message;

namespace APTEventAssignment.Controllers
{
    [Authorize]
    public class EventBookingsController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: EventBookings
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();
            //List<EventBooking> bookings = null;

            ////var query = ( from 
                
                
                
            ////    )
                
                
            ////    "SELECT UserName, EventBooking_Date, Event_Name, EventPerformance_Date " +
            ////            "FROM EventBooking "+
            ////            "INNER JOIN EventBooking eb "

            //if (User.IsInRole("admin"))
            //{
            //    bookings = db.EventBooking.Include(e => e.EventPerformance).ToList(); //get all evnets of all users
            //}
            //else
            //{
            //    bookings = db.EventBooking.Include(e => e.EventPerformance).Where(e => e.EventBooking_UserID == userId).ToList(); // get all event bookings of a particular user
            //}

            //return View(bookings); // return the list

            var viewmodel = (from eb in db.EventBooking
                             join pd in db.EventPerformance on eb.EventBooking_EventPerformanceID equals pd.EventPerformance_ID
                             join en in db.EventPerformance on eb.EventPerformance.EventPerformance_EventID equals en.Event.Event_ID
                             join u in db.EventBooking on eb.EventBooking_UserID equals u.AspNetUsers.Id
                             select new EventBookingViewModel()
                             {
                                 EventBooking_Date = eb.EventBooking_Date,
                                 EventBooking_ID = eb.EventBooking_ID,
                                 PerformanceDate = pd.EventPerformance_Date,
                                 EventName = en.Event.Event_Name,
                                 UserName = u.AspNetUsers.UserName

                             });

            return View(viewmodel.ToList());     
        }

        public ActionResult IndexBooking(int? id)
        {
            if (Session["BookingDetails"] != null)
            {
                // Get all the details for the booking
                var bookingDetails = this.Session["BookingDetails"] as EventBookingSeatsViewModel;
                var currentDate = DateTime.Now;
                var userId = User.Identity.GetUserId();
                var username = User.Identity.GetUserName();

                var query = (from ep in db.EventPerformance where ep.EventPerformance_ID == bookingDetails.SelectPerformanceId select ep.EventPerformance_Date).Single();
                DateTime perfDate = Convert.ToDateTime(query);

                var viewmodel = new EventBookingViewModel
                {
                    EventBooking_UserID = userId,
                    EventBooking_EventPerformanceID = bookingDetails.SelectPerformanceId,
                    UserName = username,
                    EventBooking_Date = currentDate,
                    EventName = bookingDetails.Event_Name,
                    PerformanceDate = perfDate
                };

                //List<EventBooking> bookings = null;
                //bookings = db.EventBooking.Include(e => e.EventPerformance).Where(e => e.EventBooking_UserID == userId).ToList();

                return View(viewmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexBooking()
        {
            if (Session["BookingDetails"] != null)
            {
                var bookingDetails = this.Session["BookingDetails"] as EventBookingSeatsViewModel;
                var currentDate = DateTime.Now;
                var userId = User.Identity.GetUserId();
                var username = User.Identity.GetUserName();
                var phoneNo = bookingDetails.PhoneNumber;
                var bookingNo = generateBookingNo();

                // get performance date according to the selected performance ID
                var query = (from ep in db.EventPerformance where ep.EventPerformance_ID == bookingDetails.SelectPerformanceId select ep.EventPerformance_Date).Single();
                DateTime perfDate = Convert.ToDateTime(query);

                // Save changes in the db
                var viewmodel = new AddEventBookingViewModel
                {
                    EventBooking_UserID = userId,
                    EventBooking_EventPerformanceID = bookingDetails.SelectPerformanceId,
                    UserName = username,
                    EventBooking_Date = currentDate,
                    EventName = bookingDetails.Event_Name,
                    PerformanceDate = perfDate
                };

                // create booking record in db
                Create(viewmodel);

                // edit users table to include the phone number
                var existingUser = db.AspNetUsers.Find(userId);
                existingUser.PhoneNumber = phoneNo;

                if (EditUser(existingUser) == false)
                {
                    RedirectToAction("IndexBooking", "EventBookings");
                }

                //Send E-mail with ticket
                var userEmail = User.Identity.GetUserName();

                if (ModelState.IsValid)
                {
                    var body = "<p><h1>{1}</h1><br><h3><u>Booking Details</u></h3><p><b>Booking Number:</b> {3}</p><p><b>Date of Booking:</b> {0}</p><p><b>Performance Date:</b> {2}</p><p><b>Seat numbers:</b> L22, L23, L24</p><p><b>Price:</b> 50 Euros</p><br><b>Thank you for booking with us!</b><br>Events in Malta";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(userEmail));
                    message.Subject = "Event Ticket Details";
                    message.Body = string.Format(body, viewmodel.EventBooking_Date, viewmodel.EventName, viewmodel.PerformanceDate, bookingNo);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(message);
                    }
                }

                //Send SMS if phone number is provided
                if (phoneNo != null)
                {
                    CreateSms cs = new CreateSms();
                    cs.SendSMS(viewmodel.EventBooking_Date.ToString(), viewmodel.EventName, viewmodel.PerformanceDate.ToString());
                }

                // kill session
                Session.Abandon();

                return RedirectToAction("Checkout", "EventBookings");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        public String generateBookingNo()
        {
            Random rnd = new Random();
            int num = rnd.Next(10000, 99999);

            // generate the check digit according to the random number
            int checkdigit = num % 7;

            return "EM"+num+checkdigit;
        }


        // used to include phone number 
        public bool EditUser([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,DOB")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUsers).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }


        // GET: EventBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eb = db.EventBooking.Find(id);
            if (eb == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new EventBookingViewModel
            {
                EventBooking_Date = eb.EventBooking_Date,
                EventBooking_ID = eb.EventBooking_ID,
                PerformanceDate = eb.EventPerformance.EventPerformance_Date,
                EventName = eb.EventPerformance.Event.Event_Name,
                UserName = eb.AspNetUsers.UserName
            };

            return View(viewmodel);
        }

        public ActionResult Checkout()
        {
            //ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.EventBooking.Add(eventBooking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", eventBooking.EventBooking_EventPerformanceID);
            return View(eventBooking);
        }

        private void UpdateEventBooking(EventBooking eb, AddEventBookingViewModel addviewmodel)
        {
            eb.EventBooking_ID = addviewmodel.EventBooking_ID;
            eb.EventBooking_Date = addviewmodel.EventBooking_Date;
            eb.EventBooking_UserID = addviewmodel.EventBooking_UserID;
            eb.EventBooking_EventPerformanceID = addviewmodel.EventBooking_EventPerformanceID;
        }

        // GET: EventBookings/Create
        public ActionResult Create()
        {
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID");
            //return View();
            return View(new AddEventBookingViewModel());
        }

        // POST: EventBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // in the Bind(Include = ...) put those fields only that will be returned by the view 
        //EventBooking_ID,EventBooking_Date,EventBooking_UserID,EventBooking_EventPerformanceID,EventBooking_Deleted
        //public ActionResult Create([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        public ActionResult Create(AddEventBookingViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                //eventBooking.EventBooking_UserID = User.Identity.GetUserId();
              
                var eb = new EventBooking();

                UpdateEventBooking(eb, addviewmodel);

                db.EventBooking.Add(eb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", addviewmodel.EventBooking_EventPerformanceID);
            return View(addviewmodel);
        }

        // GET: EventBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eb = db.EventBooking.Find(id);
            if (eb == null)
            {
                return HttpNotFound();
            }

            var addviewmodel = new AddEventBookingViewModel
            {
                EventBooking_ID = eb.EventBooking_ID,
                EventBooking_Date = eb.EventBooking_Date,
                EventBooking_UserID = eb.EventBooking_UserID,
                EventBooking_EventPerformanceID = eb.EventBooking_EventPerformanceID,
            };

            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", addviewmodel.EventBooking_EventPerformanceID);
            //ViewBag.EventBooking_UserID = new SelectList(db.User, "User_ID", "User_Login", eventBooking.EventBooking_UserID);
            return View(addviewmodel);
        }

        // POST: EventBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EventBooking_Date,EventBooking_EventPerformanceID,EventBooking_Deleted")] EventBooking eventBooking)
        public ActionResult Edit(AddEventBookingViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                //eventBooking.EventBooking_UserID = User.Identity.GetUserId();
                //db.Entry(eventBooking).State = EntityState.Modified;
                //db.SaveChanges();
                var existingBooking = db.EventBooking.Find(addviewmodel.EventBooking_ID);
                UpdateEventBooking(existingBooking, addviewmodel);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.EventBooking_EventPerformanceID = new SelectList(db.EventPerformance, "EventPerformance_ID", "EventPerformance_ID", addviewmodel.EventBooking_EventPerformanceID);

            return View(addviewmodel);
        }

        // GET: EventBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eb = db.EventBooking.Find(id);
            if (eb == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new EventBookingViewModel
            {
                EventBooking_Date = eb.EventBooking_Date,
                EventBooking_ID = eb.EventBooking_ID,
                PerformanceDate = eb.EventPerformance.EventPerformance_Date,
                EventName = eb.EventPerformance.Event.Event_Name,
                UserName = eb.AspNetUsers.UserName
            };

            return View(viewmodel);
        }

        // POST: EventBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventBooking eb = db.EventBooking.Find(id);
            db.EventBooking.Remove(eb);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LFPEvents.Models;
using LFPEvents.Models.ViewModels;

namespace LFPEvents.Controllers
{
    public class BookingsController : Controller
    {
        private LFPDataBContext db = new LFPDataBContext();

        public ActionResult Index(string search)
        {
            ViewBag.Title = "Bookings";

            var q = db.Bookings.Include(b => b.Event.Venue);

            if (!string.IsNullOrEmpty(search))
                q = q.Where(b =>
                    b.BookingID.ToString().Contains(search) ||
                    b.Event.Title.Contains(search));

            var list = q.Select(b => new BookingViewModel
            {
                BookingID = b.BookingID,
                CustomerName = b.CustomerName,
                EventTitle = b.Event.Title,
                VenueName = b.Event.Venue.Name,
                BookingDate = b.BookingDate
            }).ToList();

            return View(list);
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Booking Details";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Include(b => b.Event.Venue).FirstOrDefault(b => b.BookingID == id);
            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Create Booking";
            ViewBag.EventID = new SelectList(db.Events.Include(e => e.Venue), "EventID", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,CustomerName,EventID,BookingDate")] Booking booking)
        {
            ViewBag.Title = "Create Booking";

            try
            {
                if (ModelState.IsValid)
                {
                    var selectedEvent = db.Events
     .Where(e => e.EventID == booking.EventID)
     .Select(e => new { e.EventID, VenueID = e.VenueID })
     .FirstOrDefault();

                    if (selectedEvent == null)
                    {
                        ModelState.AddModelError("", "Selected event not found.");
                    }
                    else
                    {
                        bool isDoubleBooked = db.Bookings
                            .Include(b => b.Event)
                            .Any(b =>
                                b.Event.VenueID == selectedEvent.VenueID &&
                                DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(booking.BookingDate));

                        if (isDoubleBooked)
                        {
                            ModelState.AddModelError("", "This venue is already booked on the selected date.");
                        }
                        else
                        {
                            db.Bookings.Add(booking);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }


                    if (selectedEvent == null)
                    {
                        ModelState.AddModelError("", "Selected event not found.");
                    }
                    else
                    {
                        bool isDoubleBooked = db.Bookings
                            .Include(b => b.Event)
                            .Any(b =>
                                b.Event.VenueID == selectedEvent.VenueID &&
                                DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(booking.BookingDate));

                        if (isDoubleBooked)
                        {
                            ModelState.AddModelError("", "This venue is already booked on the selected date.");
                        }
                        else
                        {
                            db.Bookings.Add(booking);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while creating the booking.");
            }

            ViewBag.EventID = new SelectList(db.Events, "EventID", "Title", booking.EventID);
            return View(booking);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit Booking";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            ViewBag.EventID = new SelectList(db.Events, "EventID", "Title", booking.EventID);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,CustomerName,EventID,BookingDate")] Booking booking)
        {
            ViewBag.Title = "Edit Booking";

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the booking.");
            }

            ViewBag.EventID = new SelectList(db.Events, "EventID", "Title", booking.EventID);
            return View(booking);
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete Booking";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Include(b => b.Event.Venue).FirstOrDefault(b => b.BookingID == id);
            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Title = "Delete Booking";

            try
            {
                Booking booking = db.Bookings.Find(id);
                if (booking == null)
                    return HttpNotFound();

                db.Bookings.Remove(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Cannot delete booking. Please try again or contact support.");
                Booking booking = db.Bookings.Find(id);
                return View(booking);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}

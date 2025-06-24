using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LFPEvents.Models;

namespace LFPEvents.Controllers
{
    public class EventsController : Controller
    {
        private LFPDataBContext db = new LFPDataBContext();

        // GET: Events
        public ActionResult Index()
        {
            ViewBag.Title = "Events";
            var events = db.Events.Include(e => e.Venue);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Event Details";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Event @event = db.Events.Include(e => e.Venue).FirstOrDefault(e => e.EventID == id);
            if (@event == null)
                return HttpNotFound();

            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create Event";
            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Title,Description,StartDate,EndDate,VenueID")] Event @event)
        {
            ViewBag.Title = "Create Event";

            bool isDoubleBooked = db.Events.Any(e =>
                e.VenueID == @event.VenueID &&
                e.EventID != @event.EventID &&
                @event.StartDate <= e.EndDate &&
                @event.EndDate >= e.StartDate
            );

            if (isDoubleBooked)
            {
                ModelState.AddModelError("", "This venue is already booked during the selected date range.");
            }

            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Name", @event.VenueID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit Event";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Event @event = db.Events.Find(id);
            if (@event == null)
                return HttpNotFound();

            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Name", @event.VenueID);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Title,Description,StartDate,EndDate,VenueID")] Event @event)
        {
            ViewBag.Title = "Edit Event";

            bool isDoubleBooked = db.Events.Any(e =>
                e.VenueID == @event.VenueID &&
                e.EventID != @event.EventID &&
                @event.StartDate <= e.EndDate &&
                @event.EndDate >= e.StartDate
            );

            if (isDoubleBooked)
            {
                ModelState.AddModelError("", "This venue is already booked during the selected date range.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VenueID = new SelectList(db.Venues, "VenueID", "Name", @event.VenueID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete Event";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Event @event = db.Events.Include(e => e.Venue).FirstOrDefault(e => e.EventID == id);
            if (@event == null)
                return HttpNotFound();

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Title = "Delete Event";

            if (db.Bookings.Any(b => b.EventID == id))
            {
                TempData["Error"] = "❌ Cannot delete event with active bookings.";
                return RedirectToAction("Index");
            }

            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LFPEvents.Models;
using LFPEvents.Helpers;
using System.Threading.Tasks;


namespace LFPEvents.Controllers
{
    public class VenuesController : Controller
    {
        private LFPDataBContext db = new LFPDataBContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Venues";
            return View(db.Venues.ToList());
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Title = "Venue Details";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Venue venue = db.Venues.Find(id);
            if (venue == null)
                return HttpNotFound();

            return View(venue);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Create Venue";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Venue venue, HttpPostedFileBase imageFile)
        {
            ViewBag.Title = "Create Venue";

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string imageUrl = await BlobHelper.UploadFileAsync(imageFile);
                    venue.ImageURL = imageUrl;
                }

                db.Venues.Add(venue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(venue);
        }


        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit Venue";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Venue venue = db.Venues.Find(id);
            if (venue == null)
                return HttpNotFound();

            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VenueID,Name,Location,Capacity")] Venue venue)
        {
            ViewBag.Title = "Edit Venue";

            if (ModelState.IsValid)
            {
                db.Entry(venue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(venue);
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.Title = "Delete Venue";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Venue venue = db.Venues.Find(id);
            if (venue == null)
                return HttpNotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Title = "Delete Venue";

            Venue venue = db.Venues.Find(id);

            bool isVenueBooked = db.Events.Any(e => e.VenueID == venue.VenueID && db.Bookings.Any(b => b.EventID == e.EventID));
            if (isVenueBooked)
            {
                ModelState.AddModelError("", "Cannot delete this venue because it is linked to a booking.");
                return View(venue);
            }

            db.Venues.Remove(venue);
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
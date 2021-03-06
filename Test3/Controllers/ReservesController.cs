﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Test3.Models;

namespace Test3.Views
{
    [Authorize]
    public class ReservesController : Controller
    {
        private Model1Container db = new Model1Container();
        private int? s;
        // GET: Reserves
        public ActionResult Index()
        {
            //var reserves = db.Reserves.Include(r => r.Room).Include(r => r.Event);
            s = ((User)Session["CurrentUser"]).Society_ID;
            RolePrincipal roles = (RolePrincipal)User;
            String[] role = roles.GetRoles();
            ViewData["role"] = role[0];
            ViewData["society"] = s;
            var reserves = db.Reserves.Include(r => r.Room).Where(a => a.Event.Society.Society_ID == s); ;
            //switch (role[0])
            //{
            //    case "admin": reserves = db.Reserves.Include(r => r.Room).Include(a=>a.Event); break;
            //}
            reserves = db.Reserves.Include(r => r.Room).Include(a => a.Event);

            return View(reserves.ToList());
        }

        // GET: Reserves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserves reserves = db.Reserves.Find(id);
            if (reserves == null)
            {
                return HttpNotFound();
            }
            return View(reserves);
        }

        // GET: Reserves/Create
        public ActionResult Create()
        {
            s = ((User)Session["CurrentUser"]).Society_ID;
            ViewBag.Room_ID = new SelectList(db.Rooms, "Room_ID", "Room_Name");
            ViewBag.Event_ID = new SelectList(db.Events.Where(a => a.Society.Society_ID == s), "Event_ID", "Event_name");
            return View();
        }

        // POST: Reserves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Reserve_ID,Date,Start_Time,End_Time,Room_ID,Event_ID")] Reserves reserves)
        {

            if (ModelState.IsValid)
            {
                if (validReserve(reserves))
                {
                    db.Reserves.Add(reserves);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Reservation");
                }
                
            }
            s = ((User)Session["CurrentUser"]).Society_ID;
            ViewBag.Room_ID = new SelectList(db.Rooms, "Room_ID", "Room_Name", reserves.Room_ID);
            ViewBag.Event_ID = new SelectList(db.Events.Where(a => a.Society.Society_ID == s), "Event_ID", "Event_name", reserves.Event_ID);
            
            return View(reserves);
        }

        private Boolean validReserve(Reserves reserves)
        {
           Reserves temp = db.Reserves.Where(x => ((reserves.Start_Time >= x.Start_Time
                                            && reserves.Start_Time <= x.End_Time)
                                            || (reserves.End_Time >= x.Start_Time
                                            && reserves.End_Time <= x.End_Time)
                                            || (reserves.Start_Time <= x.Start_Time
                                            && reserves.End_Time >= x.End_Time))
                                            && x.Date == reserves.Date
                                            && x.Room_ID == reserves.Room_ID).FirstOrDefault();
            if(temp == null)
            {
                return true;
            }
            return false;
        }

        // GET: Reserves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserves reserves = db.Reserves.Find(id);
            if (reserves == null)
            {
                return HttpNotFound();
            }
            s = ((User)Session["CurrentUser"]).Society_ID;
            ViewBag.Room_ID = new SelectList(db.Rooms, "Room_ID", "Building_Name", reserves.Room_ID);
            ViewBag.Event_ID = new SelectList(db.Events.Where(a => a.Society.Society_ID == s), "Event_ID", "Event_name", reserves.Event_ID);
            return View(reserves);
        }

        // POST: Reserves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Reserve_ID,Date,Start_Time,End_Time,Room_ID,Event_ID")] Reserves reserves)
        {
            if (ModelState.IsValid)
            {
                if (validReserve(reserves))
                {
                    db.Reserves.Add(reserves);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Reservation");
                }

            }
            s = ((User)Session["CurrentUser"]).Society_ID;
            ViewBag.Room_ID = new SelectList(db.Rooms, "Room_ID", "Room_Name", reserves.Room_ID);
            ViewBag.Event_ID = new SelectList(db.Events.Where(a => a.Society.Society_ID == s), "Event_ID", "Event_name", reserves.Event_ID);
            return View(reserves);
        }

        // GET: Reserves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserves reserves = db.Reserves.Find(id);
            if (reserves == null)
            {
                return HttpNotFound();
            }
            return View(reserves);
        }

        // POST: Reserves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserves reserves = db.Reserves.Find(id);
            db.Reserves.Remove(reserves);
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

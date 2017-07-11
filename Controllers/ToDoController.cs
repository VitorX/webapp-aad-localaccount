using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
   
    public class ToDoController : Controller
    {
        // GET: ToDo
        public ActionResult Index()
        {
            Todo[] tasks = {
                 new Todo() { ID=1, Owner="o1", Description="d1" },
                 new Todo() {  ID=2, Owner="o2", Description="d2" },
            };
            return View(tasks);
        }

        // GET: ToDo/Details/5
        public ActionResult Details(int id)
        {
            Todo task = new Todo {
                ID = id,
                Owner = "o1",
                Description = "d1"
            };
            return View(task);
        }

        // GET: ToDo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDo/Edit/5
        public ActionResult Edit(int id)
        {
            Todo task = new Todo
            {
                ID = id,
                Owner = "o1",
                Description = "d1"
            };
            return View(task);
        }

        // POST: ToDo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ToDo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Resumes.Models;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Serialization;
using HeyRed.MarkdownSharp;

namespace Resumes.Controllers
{
    public class JobsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Jobs
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.Person);
            return View(jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", job.PersonId);
            return View(job);
        }

        // POST: Jobs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Date,Description,PersonId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.PersonId = new SelectList(db.People, "Id", "Name", job.PersonId);
            return View(job);
        }

        



        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", job.PersonId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Date,Description,PersonId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", job.PersonId);
            return View(job);
        }

        //[ChildActionOnly] //TODO: Uncomment this line
        public ActionResult EditablePanel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost]
        //[ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult EditablePanel([Bind(Include = "Id,Title,Date,Description,PersonId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                var updatedJob = JsonConvert.SerializeObject(new JobData(job), new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                return Content(updatedJob, "application/json");
                //return RedirectToAction("Index");
            }
            //ViewBag.PersonId = new SelectList(db.People, "Id", "Name", job.PersonId);
            //return View(job);
            var error = JsonConvert.SerializeObject(new { error = "invalid data" });
            return Content(error, "application/json");

        }

        public class JobData
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string DateInput { get; set; }
            public string DateFormatted { get; set; }
            public string Description { get; set; }
            public JobData(Job job)
            {
                this.Id = job.Id;
                this.Description = new Markdown().Transform(job.Description);
                this.DateInput = string.Format("{0:yyyy-MM-dd}", job.Date);
                this.DateFormatted = string.Format("{0:dd MMMM yyyy}", job.Date);
                this.Title = job.Title;
            }


        }

        /*public ActionResult GetJob(int? id)
        {
            var error = JsonConvert.SerializeObject(new { Error = "Bad request" });
            
            if (id == null)
            {
                return Content(JsonConvert.SerializeObject(new { Error = "Bad request" }), "application/json");
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return Content(JsonConvert.SerializeObject(new { Error = "Not found" }), "application/json");
            }
            var y = JsonConvert.SerializeObject(new JobData(job), Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                
            });
            return Content(y, "application/json");
        }*/

        /*public JsonResult UpdateJob(int? id)
        {
            
        }*/

        //[ChildActionOnly]
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            /*int? valuePassedIn = this.ViewData.ContainsKey("PersonId") ? (int?)this.ViewData["PersonId"] : null;
            if (valuePassedIn == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/

            Job job = new Job();
            Person person = db.People.Find(id);

            if (person == null /*&& valuePassedIn != id*/)
            {
                return HttpNotFound();
            }
            job.Person = person;
            job.PersonId = (int)id;
            return View(job);
        }

        /*[ChildActionOnly]
        public ActionResult Add(Person person)
        {
            Job job = new Job();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job.Person = person;
            return PartialView(job);
        }*/

        [HttpPost]
        //[ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Id,Title,Date,Description,PersonId")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();

                /*Job jobNew = new Job()
                {
                    Person = job.Person,
                    PersonId = job.PersonId
                };*/
                return RedirectToAction("Details", "People", new { id = job.PersonId });
                //return RedirectToAction("Index");
            }

            //ViewBag.PersonId = new SelectList(db.People, "Id", "Name", job.PersonId);
            return View(job.PersonId);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("DeleteJob")]
        public ActionResult DeleteJobFromPanel([Bind(Include = "Id")] int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            var success = JsonConvert.SerializeObject(new { success = "data was successfully deleted" });
            return Content(success, "application/json");
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

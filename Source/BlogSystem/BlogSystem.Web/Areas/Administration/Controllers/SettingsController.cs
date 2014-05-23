namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;

    public class SettingsController : AdminController
    {
        private readonly IRepository<Setting> settingsData;

        public SettingsController(IRepository<Setting> settingsRepository)
        {
            this.settingsData = settingsRepository;
        }

        // GET: /Administration/Settings/
        public ActionResult Index()
        {
            return this.View(this.settingsData.All().ToList());
        }

        // GET: /Administration/Settings/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Setting setting = this.settingsData.All().FirstOrDefault(s => s.Name == id);
            if (setting == null)
            {
                return this.HttpNotFound();
            }

            return this.View(setting);
        }

        // POST: /Administration/Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Value")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                this.settingsData.Update(setting);
                this.settingsData.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.settingsData.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

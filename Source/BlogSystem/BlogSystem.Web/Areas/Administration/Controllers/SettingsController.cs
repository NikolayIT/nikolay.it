namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;

    public class SettingsController : Controller
    {
        private readonly IRepository<Setting> settingsData;

        public SettingsController(IRepository<Setting> settingsRepository)
        {
            settingsData = settingsRepository;
        }

        // GET: /Administration/Settings/
        public ActionResult Index()
        {
            return View(settingsData.All().ToList());
        }

        // GET: /Administration/Settings/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = settingsData.All().FirstOrDefault(s => s.Name == id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: /Administration/Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Name,Value")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                settingsData.Update(setting);
                settingsData.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                settingsData.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

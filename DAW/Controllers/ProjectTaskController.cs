using DAW.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace DAW.Controllers
{
    public class ProjectTaskController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult New(int projectId)
        {
            ProjectTask task = new ProjectTask();
            task.UserId = User.Identity.GetUserId();
            task.ProjectId = projectId;
            var project = db.Projects.Find(projectId);
            ViewBag.Users = project.TeamMembers;
            return View(task);
        }

        [HttpPut]
        public ActionResult New(ProjectTask task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    TempData["message"] = "✔ Task created successfully!";
                    return RedirectToRoute("Project/Show/" + task.ProjectId);
                }
                else
                {
                    return View(task);
                }
            }
            catch (Exception e)
            {
                return View(task);
            }
        }
    }
}
using DAW.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DAW.Controllers
{
    public class ProjectTaskController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Show(int id)
        {
            ProjectTask task = db.Tasks.Find(id);
            ViewBag.currentUserId = User.Identity.GetUserId();
            ViewBag.isAdmin = User.IsInRole("Administrator");
            return View(task);
        }

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
                    return Redirect("/Project/Show/" + task.ProjectId);
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

        public ActionResult Edit(int id)
        {
            ProjectTask task = db.Tasks.Find(id);
            List<Status> statuses = new List<Status>() { Status.NotStarted, Status.InProgress, Status.Done};
            statuses.Remove(task.Status);
            ViewBag.AllStatuses = statuses;
            return View(task);
        }

        [HttpPut]
        public ActionResult Edit(int id, ProjectTask requestTask)
        {
            ProjectTask task = db.Tasks.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(task))
                    {
                        task.Title = requestTask.Title;
                        task.Description = requestTask.Description;
                        task.Status = requestTask.NewStatus;
                        db.SaveChanges();
                        TempData["message"] = "✔ Task updated successfully!";
                    }
                    return RedirectToAction("Show/" + task.Id);
                }
                else
                {
                    TempData["message"] = "Invalid task information!";
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "An exception occured!";
            }
            List<Status> statuses = new List<Status>() { Status.NotStarted, Status.InProgress, Status.Done };
            statuses.Remove(task.Status);
            ViewBag.AllStatuses = statuses;
            return View(requestTask);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ProjectTask task = db.Tasks.Find(id);
            int projectId = task.ProjectId;
            db.Tasks.Remove(task);
            db.SaveChanges();
            TempData["message"] = "✔ Task removed successfully!";
            return Redirect("/Project/Show/" + projectId);
        }
    }
}
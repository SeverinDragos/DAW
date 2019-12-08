using DAW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW.Controllers
{
    public class ProjectController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            var projects = db.Projects.Include("TeamMembers");
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult Show(int id)
        {
            Project project = db.Projects.Find(id);

            return View(project);
        }

        public ActionResult New()
        {
            Project project = new Project();

            project.ProjectManagerId = User.Identity.GetUserId();

            return View(project);
        }

        [HttpPost]
        public ActionResult New(Project project)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                if (ModelState.IsValid)
                {
                    /*var allUsers = from user in db.Users
                                   select user;
                    foreach (ApplicationUser userfor in allUsers)
                    {
                        Debug.Print(userfor.Email);
                    }
                    var projectManager = from user in db.Users
                                         where user.Id == project.ProjectManagerId
                                         select user;
                    foreach (ApplicationUser userfor in projectManager)
                    {
                        Debug.Print(userfor.Email);
                        project.TeamMembers.Add(userfor);
                    }*/
                    var projectManager = db.Users.Find(project.ProjectManagerId);
                    db.Projects.Add(project);
                    userManager.AddToRole(project.ProjectManagerId, "ProjectManager");
                    project.TeamMembers.Add(projectManager);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View(project);
            }
        }

        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);

            if (project.ProjectManagerId == User.Identity.GetUserId())
            {
                return View(project);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        public ActionResult Edit(int id, Project requestProject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Project project = db.Projects.Find(id);
                    if (project.ProjectManagerId == User.Identity.GetUserId())
                    {
                        if (TryUpdateModel(project))
                        {
                            project.Title = requestProject.Title;
                            project.Description = requestProject.Description;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestProject);
                }
            }
            catch (Exception e)
            {
                return View(requestProject);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            if (project.ProjectManagerId == User.Identity.GetUserId())
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
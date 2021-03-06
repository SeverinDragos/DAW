﻿using DAW.Models;
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
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var projects = db.Projects.Include("TeamMembers");
            ViewBag.Projects = projects;
            ViewBag.currentuser = User.Identity.GetUserId();
            ViewBag.isAdmin = User.IsInRole("Administrator");
            return View();
        }

        public ActionResult Show(int id)
        {
            Project project = db.Projects.Find(id);
            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
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
                    db.Projects.Add(project);
                    userManager.AddToRole(project.ProjectManagerId, "ProjectManager");
                    var projectManager = db.Users.Find(project.ProjectManagerId);
                    project.TeamMembers.Add(projectManager);
                    db.SaveChanges();
                    TempData["message"] = "✔ Project created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(project);
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
                TempData["message"] = "✘ You do not have the authority to edit this project!";
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
                            TempData["message"] = "✔ Project updated successfully!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "✘ You do not have the authority to edit this project!";
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
                TempData["message"] = "✔ Project removed successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "✘ You do not have the authority to delete this project!";
                return RedirectToAction("Index");
            }
        }


        ///TeamMembers functionality
        
        [NonAction]
        public IEnumerable<SelectListItem> GetAllUsers()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var users = from user in db.Users
                             select user;
            // iteram prin categorii
            foreach (var user in users)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = user.Id,
                    Text = user.UserName
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        public ActionResult ShowTeamMembers(int id)
        {
            Project project = db.Projects.Find(id);
            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            ViewBag.AllUsers = db.Users;

            return View("TeamMembers", project);
        }

        [HttpPut]

        public ActionResult AddTeamMember(int id, Project requestProject)
        {
            Project project = db.Projects.Find(id);
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (requestProject.NewUserId != null)
            {
                ApplicationUser user = db.Users.Find(requestProject.NewUserId);
                if (project.TeamMembers.Contains(user) == false)
                {
                    project.TeamMembers.Add(user);
                    UserManager.RemoveFromRole(user.Id, "User");
                    UserManager.AddToRole(user.Id, "TeamMember");
                    db.SaveChanges();
                    TempData["message"] = "✔ Member added successfully!";
                }
                else
                {
                    TempData["message"] = "This member is already part of the team!";
                }
            }
            else
            {
                TempData["message"] = "Please select a user to be added!";
            }
            ViewBag.AllUsers = db.Users;
            return View("TeamMembers", project);
        }

        [HttpDelete]
        public ActionResult RemoveTeamMember(int id, string userId)
        {
            Project project = db.Projects.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            project.TeamMembers.Remove(user);
            db.SaveChanges();
            if (user.Projects.Count == 0)
            {
                UserManager.RemoveFromRole(userId, "TeamMember");
                UserManager.AddToRole(userId, "User");
            }
            db.SaveChanges();
            TempData["message"] = "✔ Member removed successfully!";
            ViewBag.AllUsers = db.Users;
            return View("TeamMembers", project);
        }
    }
}
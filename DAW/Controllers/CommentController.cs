using DAW.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW.Controllers
{
    public class CommentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(int id)
        {
            Comment comm = new Comment();
            comm.TaskId = id;
            comm.UserId = User.Identity.GetUserId();
            return View(comm);
        }

        [HttpPut]
        public ActionResult New(Comment comm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProjectTask task = db.Tasks.Find(comm.TaskId);
                    task.Comments.Add(comm);
                    db.SaveChanges();
                    TempData["message"] = "✔ Comment added successfully!";
                    ViewBag.currentUserId = User.Identity.GetUserId();
                    ViewBag.isAdmin = User.IsInRole("Administrator");
                    return Redirect("/ProjectTask/Show/" + comm.TaskId);
                }
                else
                {
                    return View(comm);
                }
            }
            catch (Exception e)
            {
                return View(comm);
            }
        }
    }
}
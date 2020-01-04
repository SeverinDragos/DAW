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

        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);

            return View(comment);
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            Comment comment = db.Comments.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(comment))
                    {
                        comment.Content = requestComment.Content;
                        db.SaveChanges();
                        TempData["message"] = "✔ Comment updated successfully!";
                    }
                    return Redirect("/ProjectTask/Show/" + comment.TaskId);
                }
                else
                {
                    TempData["message"] = "Introduced information cannot be saved!";
                    return Redirect("/ProjectTask/Show/" + comment.TaskId);
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "An error occured during comment editing!";
                return Redirect("/ProjectTask/Show/" + comment.TaskId);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            TempData["message"] = "✔ Comment removed successfully!";
            return Redirect("/ProjectTask/Show/" + comment.TaskId);
        }
    }
}
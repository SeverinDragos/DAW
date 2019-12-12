using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAW.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectManagerId { get; set; }
        public string NewUserId { get; set; }


        public virtual ICollection<ApplicationUser> TeamMembers { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
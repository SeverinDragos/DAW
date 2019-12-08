using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAW.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ProjectManagerId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> TeamMembers { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAW.Models
{
    public enum Status { NotStarted = 0, InProgress, Done }

    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }
        public Status NewStatus { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
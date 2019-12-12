﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAW.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        public int TaskId { get; set; }
        public virtual ProjectTask Task { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
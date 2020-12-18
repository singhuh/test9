using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace test9.Models
{
    public class Resume
    {
        [Key]
        public long ResumeID { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }

        [Display(Name = "Upload Resume")]
        public string ResumeFile { get; set; }
    }
}

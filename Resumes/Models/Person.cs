using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Resumes.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name length must be less than 50 characters.")]
        public string Name { get; set; }

        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
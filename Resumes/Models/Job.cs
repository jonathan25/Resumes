using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Resumes.Models
{
    public class Job
    {
        /*Id [Identity Key]
■ Title [Free Text: 255]
■ Date [Date]
■ Description [Text: CommonMark]
■ Person [Foreign Relation to Person]
*/
        [Key]
        public int Id { get; set; }

        [StringLength(255, ErrorMessage = "Title length must be less than 255 characters.")]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int PersonId { get; set; }

        //[ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}
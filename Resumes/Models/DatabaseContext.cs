using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Resumes.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public System.Data.Entity.DbSet<Resumes.Models.Job> Jobs { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace CalendarUI.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(64)]
        public string name { get; set; }

        [Required]
        public DateTime dateOfBirth { get; set; }

    }

    public class UserDBContext : DbContext
    {
        public DbSet<User> Users {get; set;}
    }
}
using Microsoft.EntityFrameworkCore;
using pushNotificationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pushNotificationAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}

using Menu_QRCode;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer
{
    public class Context:IdentityDbContext<User, IdentityRole, string>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        // تعریف DbSet ها
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
 

}

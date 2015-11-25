using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Model;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<LendObject> LendObject { get; set; }

        public DbSet<Lending> Lending { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRUD.Models;

namespace CRUD.Data
{
    public class EFDbContext : DbContext
    {
        public EFDbContext (DbContextOptions<EFDbContext> options)
            : base(options)
        {
        }

        public DbSet<CRUD.Models.Order> Orders { get; set; } = default!;
        public DbSet<CRUD.Models.OrderItem> OrderItems { get; set; } = default!;
        public DbSet<CRUD.Models.Provider> Providers { get; set; } = default!;
    }
}

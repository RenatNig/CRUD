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

        public DbSet<CRUD.Models.Order> Order { get; set; } = default!;
        public DbSet<CRUD.Models.OrderItem> OrderItem { get; set; } = default!;
        public DbSet<CRUD.Models.Provider> Provider { get; set; } = default!;
    }
}

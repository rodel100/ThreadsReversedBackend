using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThreadService.Models;

namespace ThreadService.Data
{
    public class ThreadServiceContext : DbContext
    {
        public ThreadServiceContext (DbContextOptions<ThreadServiceContext> options)
            : base(options)
        {
        }

        public DbSet<ThreadService.Models.ThreadPost> ThreadPost { get; set; } = default!;
        public DbSet<ThreadService.Models.Reply> Reply { get; set; } = default!;
    }
}

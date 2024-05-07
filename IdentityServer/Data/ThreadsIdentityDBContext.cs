using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class ThreadsIdentityDBContext : IdentityDbContext<ThreadUser>
    {
        public ThreadsIdentityDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
          {
        }
    }
}

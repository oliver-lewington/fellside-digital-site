using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FellsideDigital.Data
{
    public class FellsideDigitalDbContext(DbContextOptions<FellsideDigitalDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        DbSet<ApplicationUser> Customers { get; set; } // inherit from IdentityDbContext, but we want to call it Customers instead of Users
        //DbSet<Projects> Projects { get; set; }
    }
}

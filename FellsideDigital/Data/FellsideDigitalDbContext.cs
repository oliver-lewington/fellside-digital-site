using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FellsideDigital.Data
{
    public class FellsideDigitalDbContext(DbContextOptions<FellsideDigitalDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        DbSet<ApplicationUser> Customers { get; set; }
        public DbSet<ClientInvitation> ClientInvitations => Set<ClientInvitation>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ClientInvitation>(e =>
            {
                e.HasIndex(i => i.Token).IsUnique();

                e.HasOne(i => i.CreatedBy)
                    .WithMany()
                    .HasForeignKey(i => i.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.AcceptedUser)
                    .WithMany()
                    .HasForeignKey(i => i.AcceptedUserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Invitation)
                .WithMany()
                .HasForeignKey(u => u.InvitationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

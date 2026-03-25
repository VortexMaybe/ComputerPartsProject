using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ComputerParts.Models;

namespace ComputerParts.Data
{
    public class ComputerPartsContext : IdentityDbContext<IdentityUser>
    {
        public ComputerPartsContext(DbContextOptions<ComputerPartsContext> options)
            : base(options)
        {
        }

        public DbSet<HardwareComponent> HardwareComponent { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
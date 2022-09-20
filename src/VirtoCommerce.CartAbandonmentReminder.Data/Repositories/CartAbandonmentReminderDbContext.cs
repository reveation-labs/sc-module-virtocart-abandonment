using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.CartAbandonmentReminder.Data.Repositories
{
    public class CartAbandonmentReminderDbContext : DbContextWithTriggers
    {
        public CartAbandonmentReminderDbContext(DbContextOptions<CartAbandonmentReminderDbContext> options)
            : base(options)
        {
        }

        protected CartAbandonmentReminderDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<CartAbandonmentReminderEntity>().ToTable("CartAbandonmentReminder").HasKey(x => x.Id);
            //modelBuilder.Entity<CartAbandonmentReminderEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        }
    }
}

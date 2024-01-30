using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sharpcode.CartAbandonmentReminder.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CartAbandonmentReminderDbContext>
    {
        public CartAbandonmentReminderDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CartAbandonmentReminderDbContext>();

            builder.UseSqlServer("Data Source=(local)\\MyServer;Initial Catalog=VirtoCommerce;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new CartAbandonmentReminderDbContext(builder.Options);
        }
    }
}

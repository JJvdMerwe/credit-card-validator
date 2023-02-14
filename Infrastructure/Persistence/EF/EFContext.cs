using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF
{
    public class EFContext : DbContext
    {
        DbSet<CreditCard> CreditCards { get; set; }
        DbSet<CreditCardProvider> CreditCardProviders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options
            .UseSqlite("Data Source=..\\Infrastructure\\Persistence\\app.db")
            .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CreditCard>()
                .Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(16);

            builder.Entity<CreditCardProvider>()
                .Property(x => x.Name)
                .IsRequired();

            builder.Entity<CreditCardProvider>()
                .Property(x => x.CardNumberRegEx)
                .IsRequired();
        }
    }
}

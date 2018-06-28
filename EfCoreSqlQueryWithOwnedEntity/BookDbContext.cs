using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreSqlQueryWithOwnedEntity
{
    public class BookDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseSqlServer(@"Server=.;Database=ReproSqlQueryWithOwnedEntity;Trusted_Connection=True;MultipleActiveResultSets=True")
            .UseConsoleLogging();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Author>().OwnsOne(a => a.Name, an =>
            {
                an.Property(n => n.First).HasColumnName("Firstname");
                an.Property(n => n.Last).HasColumnName("Lastname");
            });
    }

    internal static partial class Extensions
    {
        public static DbContextOptionsBuilder UseConsoleLogging(this DbContextOptionsBuilder @this)
        {
            var lf = new LoggerFactory();
            lf.AddProvider(new LoggerProvider());
            @this.UseLoggerFactory(lf);

            return @this;
        }
    }
}

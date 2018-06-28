using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreSqlQueryWithOwnedEntity
{
    public class BookDbContext : DbContext
    {
        private readonly bool useLogging;

        public DbSet<Author> Authors { get; set; }

        public BookDbContext(bool useLogging = false)
            => this.useLogging = useLogging;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=ReproSqlQueryWithOwnedEntity;Trusted_Connection=True;MultipleActiveResultSets=True");

            if (useLogging)
                optionsBuilder.UseConsoleLogging();
        }

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

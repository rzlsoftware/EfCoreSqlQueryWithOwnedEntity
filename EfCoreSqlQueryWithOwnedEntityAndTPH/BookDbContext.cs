using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreSqlQueryWithOwnedEntityAndTPH
{
    public class BookDbContext : DbContext
    {
        private readonly bool useLogging;

        public DbSet<Person> People { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public BookDbContext(bool useLogging = false)
            => this.useLogging = useLogging;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=ReproSqlQueryWithOwnedEntityAndOwnedEntity;Trusted_Connection=True;MultipleActiveResultSets=True");

            if (useLogging)
                optionsBuilder.UseConsoleLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Person>(e =>
            {
                e.OwnsOne(a => a.Name, an =>
                {
                    an.Property(n => n.First).HasColumnName("Firstname");
                    an.Property(n => n.Last).HasColumnName("Lastname");
                });

                e.HasDiscriminator<char>("Type")
                    .HasValue<Author>('A')
                    .HasValue<Customer>('C');
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

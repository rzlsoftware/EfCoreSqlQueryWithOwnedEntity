using System.Linq;
using Z.EntityFramework.Plus;
using static System.Console;

namespace EfCoreSqlQueryWithOwnedEntityAndTPH
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BookDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var a = new Author { Name = new Name { First = "Michel", Last = "aus Lönneberga" }, Description = "Importantn information" };
                var c = new Customer { Name = new Name { First = "Pippi", Last = "Langstrumpf" }, AmountConsumed = 23455.56 };
                context.People.AddRange(a, c);
                context.SaveChanges();
            }

            QueryWithOwnedTypeTest();

            BatchUpdate();
            ReadKey();
        }

        private static void QueryWithOwnedTypeTest()
        {
            using (var context = new BookDbContext(useLogging: true))
            {
                var people = context.People.ToList();     // Select query asks two times for Id: SELECT [p].[Id], [p].[Type], [p].[Description], [p].[AmountConsumed], [t].[Id], [t].[Firstname], [t].[Lastname]

                people.ForEach(a => WriteLine($"{a.Name.First,-6} {a.Name.Last}"));
            }
        }

        private static void BatchUpdate()
        {
            using (var context = new BookDbContext(useLogging: true))
            {
                // 'System.Data.SqlClient.SqlException' because of Owned Entity
                // The column 'Id' was specified multiple times for 'B'.
                context.Authors.Where(a => a.Name.First.Length < 6).Update(a => new Author { Description = "Some very important information" });
            }
        }
    }
}

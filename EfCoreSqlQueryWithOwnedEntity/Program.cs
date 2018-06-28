using System.Linq;
using static System.Console;

namespace EfCoreSqlQueryWithOwnedEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BookDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var a1 = new Author { Name = new Name { First = "Michel", Last = "aus Lönneberga" } };
                var a2 = new Author { Name = new Name { First = "Pippi", Last = "Langstrumpf" } };
                context.Authors.AddRange(a1, a2);
                context.SaveChanges();
            }

            QueryWithOwnedTypeTest();
            ReadKey();
        }

        private static void QueryWithOwnedTypeTest()
        {
            using (var context = new BookDbContext(useLogging: true))
            {
                var authors = context.Authors.ToList();     // Select query asks two times for Id: SELECT [a].[Id], [a].[Id], [a].[Firstname], [a].[Lastname]

                authors.ForEach(a => WriteLine($"{a.Name.First,-6} {a.Name.Last}"));
            }
        }
    }
}

namespace EfCoreSqlQueryWithOwnedEntityAndTPH
{
    public abstract class Person
    {
        public int Id { get; set; }
        public Name Name { get; set; }
    }
}

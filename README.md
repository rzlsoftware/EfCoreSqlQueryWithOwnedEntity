# EfCoreSqlQueryWithOwnedEntity

When querying tables with [Owned Entities](https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities) Ef Core 2.1.1 creates a SQL statement that asks twice for Id column.

```csharp
public class Name
{
    public string First { get; set; }
    public string Last { get; set; }
}
public class Author
{
    public int Id { get; set; }
    public Name Name { get; set; }
    public string Description { get; set; }
}
```

`context.Authors.ToList()` creates this SQL query:
```sql
SELECT [a].[Id], [a].[Id], [a].[Firstname], [a].[Lastname], [a].[Description]
FROM [Authors] AS [a]
```
with two times `[a].[Id]`

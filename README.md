# EfCoreSqlQueryWithOwnedEntity

Sample project for [Ef Core Issue 12491](https://github.com/aspnet/EntityFrameworkCore/issues/12491)  
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

### Same problem when also using TPH

```csharp
public class Name
{
    public string First { get; set; }
    public string Last { get; set; }
}
public abstract class Person
{
    public int Id { get; set; }
    public Name Name { get; set; }
}
public class Author : Person
{
    public string Description { get; set; }
}
public class Customer : Person
{
    public double AmountConsumed { get; set; }
}
```

`context.People.ToList()` creates this SQL query:
```sql
SELECT [p].[Id], [p].[Type], [p].[Description], [p].[AmountConsumed], [t].[Id], [t].[Firstname], [t].[Lastname]
FROM [People] AS [p]
LEFT JOIN (
    SELECT [p.Name].*
    FROM [People] AS [p.Name]
    WHERE [p.Name].[Type] IN (N'C', N'A')
) AS [t] ON [p].[Id] = [t].[Id]
WHERE [p].[Type] IN (N'C', N'A')
```

with once `[p].[Id]` and once `[t].[Id]` where both are the same.

# StrongSyntax

StrongSyntax is a C# library that lets you execute ad hoc queries from your data layer very easily. It lets you write your entire DAL (Data Access Layer) in one language: C#. Then, it generates the required SQL queries and runs them against your database. StongSyntax doesn't entirely replace SQL, but at least it lets you write the queries more easily.

Here are some examples:

```C#
  var items = Syntax
    .GetQuery()
    .Select(
        "InvItems.ID"
        , "InvItems.Code"
        , "InvItems.Name"
        , "InvItems.Description"
        ,"UnitOfMeasures.ID"
        , "UnitOfMeasures.Name"
    ).From("InvItems")
    .InnerJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID")
    .PrepareReader<InvItem>()
    .Read();
```

The result will be

------------------------------------------------------------------
| ID     |   Code   |   Name   |   Description |  ID   |  Name   |
-------------------------------------------------------------------

# StrongSyntax

StrongSyntax is a C# library that lets you execute ad hoc queries from your data layer very easily. It lets you write your entire DAL (Data Access Layer) in one language: C#. Then, it generates the required SQL queries and runs them against your database. StongSyntax doesn't entirely replace SQL, but at least it lets you write the queries more easily.

Configuration:

```
  // Initialize the syntax container. you can store it somewhere statically for convinience.
  var syntax = new Syntax("YourConnectionString");
  
  // Configure.
  syntax.Timout = 60000; // or your desired timeout for the queries.
```

Build the SQL query.

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
    .InnerJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID");
```

This will generate: 

```SQL
  SELECT
  	InvItems.ID
  	,InvItems.Code
  	,InvItems.Name
  	,InvItems.Description
  	,UnitOfMeasures.ID
  	,UnitOfMeasures.Name
  FROM InvItems
  INNER JOIN UnitOfMeasures
      ON UnitOfMeasures.ID = InvItems.UOMID
```

The result will be

| ID     |   Code   |   Name   |   Description |  ID   |  Name   |
---------|:---------|:---------|:--------------|:------|:--------|
|  ...   |   ...    |    ...   |      ....     | ...   |   ...   |



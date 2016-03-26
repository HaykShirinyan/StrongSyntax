# StrongSyntax

StrongSyntax is a C# library that lets you execute ad hoc queries from your data layer very easily. It lets you write your entire DAL (Data Access Layer) in one language: C#. Then, it generates the required SQL queries and runs them against your database. StongSyntax doesn't entirely replace SQL, but at least it lets you write the queries more easily.

Configuration:

```C#
  // Initialize the syntax container. you can store it somewhere statically for convinience.
  var syntax = new Syntax("YourConnectionString");
  
  // Configure.
  syntax.Timout = 60000; // or your desired timeout for the queries.
```

Build the SQL query:

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

You can see the underlying query by calling ToString() method of the returned object. The generated query will not be executed until you execute it either with help of ADO.NET or call Read() method of the returned object. This will read the from database and populate you POCO class with the retarned dataset:

```C#
  var items = Syntax
    .GetQuery()
    .Select(
        "InvItems.ID"
        , "InvItems.Code"
        , "InvItems.Name"
        , "InvItems.Description"
        , "UnitOfMeasures.ID"
        , "UnitOfMeasures.Name"
    ).From("InvItems")
    .InnerJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID")
    .PrepareReader<InvItem>()
    .Read();
    
    //Then use the returned collection of objects (in this case InvItem type objects).
    foreach(var i in items)
    {
        Console.WriteLine("ID = {0}, Name = {1}", i.ID, i.Name);
    }
```

In case if you need to map you Models to ViewModels or DTOs, you can call Project() method passing it a delegate that does the mapping:

```#
  // Method that maps the Model to DTO.
  private InvItemDTO MapToDTO(InvItem model)
  {
      InvItemDTO dto = new InvItemDTO();
  
      dto.ID = model.ID;
      dto.OnHandQty = model.OnHandQty;
  
      if (model.UOM != null)
      {
          dto.UOM = new UnitOfMeasureDTO();
  
          dto.UOM.ID = model.UOM.ID;
          dto.UOM.Name = model.UOM.Name;
      }
  
      return dto;
  }
  
  var items = Syntax
     .GetQuery()
     .Select(
         "InvItems.ID"
         , "InvItems.Code"
         , "InvItems.Name"
         , "InvItems.Description"
         , "InvItems.UnitPrice"
         , "UnitOfMeasures.ID"
         , "UnitOfMeasures.Name"
     ).From("InvItems")
     .LeftJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID")
     .Where("InvItems.UnitPrice > @0 AND InvItems.Name LIKE @1", unitPrice, "%17")
     .PrepareReader<InvItem>()
     .Project(MapToDTO); //pass our method that does the mapping to the reader.
```

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
    foreach (var i in items)
    {
        Console.WriteLine("ID = {0}, Name = {1}", i.ID, i.Name);
    }
```

In case if you need to map you Models to ViewModels or DTOs, you can call Project() method passing it a delegate that does the mapping:

```C#
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
  
  // Execute the query.
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

Inserting records: 

```C#
  var query = Syntax.GetInsert();

  // This will insert a new record in InvItems table and will return the number of rows affected by the query.
  var rows = query
    .Insert
    (
        "Code"
        , "Name"
        , "Description"
        , "Status"
    ).Into("InvItems")
    .Values
    (
        "1111"
        , "Name 1111"
        , "Description 1111"
        , 0
    ).Execute();
```

This will run the following query:

```SQL
  exec sp_executesql 
  	N'INSERT INTO InvItems
  	(
  		Code
  		,Name
  		,Description
  		,Status
  	)
  	VALUES
  	(
  		@0
  		,@1
  		,@2
  		,@3
  	)
  ',N'@0 nvarchar(4),@1 nvarchar(9),@2 nvarchar(16),@3 int',@0=N'1111',@1=N'Name 1111',@2=N'Description 1111',@3=0
```

In case if you need to insert multiple records in one batch, you can do:

```C#
  var query = Syntax.GetInsert();

  // Prepare the insert clause
  var into = query
      .Insert
      (
          "Code"
          , "Name"
          , "Description"
          , "Status"
      ).Into("InvItems");

  // Add the values
  for (int i = 2000; i < 2100; i++)
  {
      string code = i.ToString();

      into = into
          .Values
          (
              code
              , "Name " + code
              , "Description " + code
              , 0
          );
  }

  // This will insert 100 records into InvItems table in one batch.
  var rows = into.Execute();
```

Updates:

```C#
  // Notice we use async version of this query. All the queries support async methods.
  var rows = await Syntax.GetUpdate()
    .Update("InvItems")
    .Set(new SetClause()
    {
        {"Code", "1111" }
        , {"Name", "Changed Name" }
        , {"Description", "Changed Description" }
        , {"UnitPrice", 1000.00M }
    }).Where("Code = @0", "1111")
    .ExecuteAsync();
```

Deletes:

```C#
   var rows = Syntax.GetDelete()
    .Delete()
    .From("InvItems")
    .Where("Code = @0", "1111")
    .Execute();
```

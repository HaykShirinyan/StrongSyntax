# StrongSyntax

StrongSyntax is a C# library that lets you execute ad hoc queries from your data layer very easily.

ORM frameworks are great, but they have their limitations. They may generate rather slow queries in certain cases, or you may struggle to build complex queries with their help. Those limitation are usually compromised by help of stored procedures, but stored procedures have their own disadvantages. Modern web applications may be consisted of number of different languages: a server-side language such as C#, JavaScript for the client-side, some JavaScript framework such as AngualJS, and of course HTML and CSS. If you have to write stored procedures, SQL will be added to those languages. Also, my personal opinion is that in many cases stored procedures have too much logic in a place where having that much logic is not appropriate.

StrongSyntax may solve some of those problems. It lets you write your entire DAL (Data Access Layer) in one language: C#. 

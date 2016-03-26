using StrongSyntax;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using StrongSyntaxTests.Resources;

namespace StrongSyntaxTests
{
    public class TestBase
    {
        public static Syntax Syntax { get; set; }

        public TestBase()
        {
            Database.SetInitializer(new DbInitializer());

            using (var db = new TestDbContext())
            {
                Syntax = new Syntax(db.Database.Connection.ConnectionString);

                db.Database.Initialize(true);
            }
        }
    }
}

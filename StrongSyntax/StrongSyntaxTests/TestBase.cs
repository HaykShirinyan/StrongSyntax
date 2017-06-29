using StrongSyntax;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using StrongSyntaxTests.Resources;
using StrongSyntaxTests.Resources.Models;
using System.Collections.Generic;

namespace StrongSyntaxTests
{
    public class TestBase
    {
        public static Syntax Syntax { get; set; }

        protected IEnumerable<InvItem> GetItems()
        {
            List<InvItem> itemList = new List<InvItem>();
            Random r = new Random();

            for (int i = 0; i < 500; i++)
            {
                InvItem item = new InvItem()
                {
                    Code = i.ToString(),
                    Name = string.Format("Item {0} from temp table", i),
                    Description = string.Format("Description for Item {0}", i),
                    UnitPrice = r.Next(10, 500),
                };

                itemList.Add(item);
            }

            return itemList;
        }

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

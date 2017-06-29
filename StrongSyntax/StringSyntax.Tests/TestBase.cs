using StrongSyntax;
using System;
using StrongSyntax.Tests.Resources;
using StrongSyntax.Tests.Resources.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StrongSyntax.Tests
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
            using (var db = new TestDbContext())
            {
                var initializer = new DbInitializer();

                initializer.Seed(db);

                Syntax = new Syntax(db.Database.GetDbConnection().ConnectionString);
            }
        }
    }
}

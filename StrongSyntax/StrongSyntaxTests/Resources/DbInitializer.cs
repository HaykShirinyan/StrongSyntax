using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StrongSyntaxTests.Resources.Models;

namespace StrongSyntaxTests.Resources
{
    class DbInitializer : DropCreateDatabaseIfModelChanges<TestDbContext>
    {
        private ICollection<InvItem> InitInventory(TestDbContext context)
        {
            UnitOfMeasure uof1 = new UnitOfMeasure()
            {
                Code = "1",
                Name = "PCS"
            };

            UnitOfMeasure uof2 = new UnitOfMeasure()
            {
                Code = "2",
                Name = "Gram"
            };

            context.UnitOfMeasures.AddRange(new[] { uof1, uof2 });

            List<InvItem> itemList = new List<InvItem>();
            Random r = new Random();

            for (int i = 0; i < 500; i++)
            {
                InvItem item = new InvItem()
                {
                    Code = i.ToString(),
                    Name = string.Format("Item {0}", i),
                    Description = string.Format("Description for Item {0}", i),
                    UnitPrice = r.Next(10, 500),

                    UOM = uof1
                };

                context.InvItems.Add(item);
                itemList.Add(item);
            }

            return itemList;
        }

        private ICollection<Warehouse> InitWarehouses(TestDbContext context, ICollection<InvItem> itemList)
        {
            Random r = new Random();
            ICollection<Warehouse> warehouseList = new List<Warehouse>();

            for (int i = 0; i < 50; i++)
            {
                Warehouse w = new Warehouse();

                string strNum = (i + 1).ToString();

                w.Code = "WH-" + strNum;
                w.Name = "Warehouse " + strNum;
                w.Description = "Description for Warehouse " + strNum;

                w.InvItems = itemList.Skip(r.Next(300)).Take(r.Next(50))
                    .Select(item => new WarehouseItem()
                    {
                        Warehouse = w,
                        WarehouseID = w.ID,
                        InvItem = item,
                        InvItemID = item.ID,
                        OnHandQty = r.Next(50)
                    }).ToList();

                warehouseList.Add(w);
                context.Warehouses.Add(w);
            }

            return warehouseList;
        }

        protected override void Seed(TestDbContext context)
        {
            base.Seed(context);

            var invItems = InitInventory(context);
            var warehouses = InitWarehouses(context, invItems);

            context.SaveChanges();
        }
    }
}

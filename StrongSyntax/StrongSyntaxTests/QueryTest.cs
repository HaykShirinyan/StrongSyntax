using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongSyntax;
using System.Linq;
using StrongSyntaxTests.Resources.DTOs;
using StrongSyntaxTests.Resources.Models;

namespace StrongSyntaxTests
{
    [TestClass]
    public class QueryTest : TestBase
    {
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

        [TestMethod]
        public void Select()
        {
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

            Assert.IsTrue(items.Count > 0);
            Assert.IsTrue(items.All(i => i.ID != null && i.UOM.ID != Guid.Empty));
        }

        [TestMethod]
        public void Project()
        {
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
                .LeftJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID")
                .PrepareReader<InvItem>()
                .Project(MapToDTO);

            Assert.IsTrue(items.Count > 0);
            Assert.IsTrue(items.Any(i => i.ID != null && i.UOM.ID != Guid.Empty));
        }

        [TestMethod]
        public void Where()
        {
            decimal? unitPrice = 50.00M;

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
               .Where("InvItems.UnitPrice = @0 AND InvItems.Name LIKE @1", unitPrice, "%17")
               .PrepareReader<InvItem>()
               .Read();

            Assert.IsTrue(items.Count > 0);
            Assert.IsTrue(items.All(i => i.ID != null && i.UnitPrice == unitPrice));
        }

        [TestMethod]
        public void GroupBy()
        {
            var items = Syntax
                .GetQuery()
                .Select(
                    "InvItems.ID"
                    , "SUM(WarehouseItems.OnHandQty) AS [InvItems.OnHandQty]"
                ).From("InvItems")
                .InnerJoin("WarehouseItems", "WarehouseItems.InvItemID = InvItems.ID")
                .GroupBy(
                    "InvItems.ID"
                ).PrepareReader<InvItem>()
                .Project(MapToDTO);

            Assert.IsTrue(items.Count > 0);
            Assert.IsTrue(items.All(i => i.ID != Guid.Empty && i.OnHandQty != null));
        }
    }
}

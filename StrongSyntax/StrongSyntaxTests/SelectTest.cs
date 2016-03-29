using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongSyntax;
using System.Linq;
using StrongSyntaxTests.Resources.DTOs;
using StrongSyntaxTests.Resources.Models;
using System.Threading.Tasks;

namespace StrongSyntaxTests
{
    [TestClass]
    public class SelectTest : TestBase
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
                    , "UnitOfMeasures.ID"
                    , "UnitOfMeasures.Name"
                ).From("InvItems")
                .InnerJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID")
                .PrepareReader<InvItem>()
                .Read();

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.All(i => i.ID != null && i.UOM.ID != Guid.Empty), "Inner join didn't work.");
        }

        [TestMethod]
        public async Task Project()
        {
            var items = await Syntax
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
                .ProjectAsync(MapToDTO);

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.Any(i => i.ID != null && i.UOM.ID != Guid.Empty), "Left join didn't work.");
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
               .Where("InvItems.UnitPrice > @0 AND InvItems.Name LIKE @1", unitPrice, "%17")
               .PrepareReader<InvItem>()
               .Read();

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.All(i => i.ID != null && i.UnitPrice > unitPrice), "Where clause didn't work.");
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

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.All(i => i.ID != Guid.Empty && i.OnHandQty != null), "Sum didn't work.");
        }

        [TestMethod]
        public void OrderBy()
        {
            var items = Syntax
                .GetQuery()
                .Select(
                   "InvItems.ID"
                   , "InvItems.Code"
                   , "InvItems.Name"
                   , "InvItems.Description"
                   , "InvItems.UnitPrice"
               ).From("InvItems")
               .OrderBy(
                    "InvItems.UnitPrice"
                ).PrepareReader<InvItem>()
                .Read();

            var first = items.FirstOrDefault();
            var last = items.ElementAt(items.Count - 1);

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(last.UnitPrice > first.UnitPrice, "Order by failed.");
        }

        [TestMethod]
        public void OrderByDescending()
        {
            var items = Syntax
                .GetQuery()
                .Select(
                   "InvItems.ID"
                   , "InvItems.Code"
                   , "InvItems.Name"
                   , "InvItems.Description"
                   , "InvItems.UnitPrice"
               ).From("InvItems")
               .OrderByDescending(
                    "InvItems.UnitPrice"
                ).PrepareReader<InvItem>()
                .Read();

            var first = items.FirstOrDefault();
            var last = items.ElementAt(items.Count - 1);

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(first.UnitPrice > last.UnitPrice, "Order by failed.");
        }

        [TestMethod]
        public async Task OffsetFetch()
        {
            int rowsToSkip = 5;
            int rowsToTake = 5;

            var items = await Syntax
                .GetQuery()
                .Select(
                   "InvItems.ID"
                   , "InvItems.Code"
                   , "InvItems.Name"
                   , "InvItems.Description"
                   , "InvItems.UnitPrice"
               ).From("InvItems")
               .OrderBy(
                    "InvItems.Code"
                ).Offset(rowsToSkip)
                .Fetch(rowsToTake)
                .PrepareReader<InvItem>()
                .ReadAsync();

            var first = items.FirstOrDefault();
            var last = items.ElementAt(items.Count - 1);

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.Count == rowsToTake, "Returned wrong number of rows.");
        }

        private ICompleteQuery GetSubSelect()
        {
            var items = Syntax
                .GetQuery()
                .Select(
                   "InvItems.ID"
                   , "InvItems.Code"
                   , "InvItems.Name"
                   , "InvItems.Description"
                   , "InvItems.UnitPrice"
                   , "InvItems.UOMID"
               ).From("InvItems");

            return items;
        }

        [TestMethod]
        public async Task SubSelect()
        {
            var subSelect = GetSubSelect();

            var items = await Syntax
                .GetQuery()
                .Select(
                    "I.ID AS [InvItems.ID]"
                    , "I.Code AS [InvItems.Code]"
                    , "I.UnitPrice AS [InvItems.UnitPrice]"
                ).From(subSelect, "I")
                .PrepareReader<InvItem>()
                .ReadAsync();

            Assert.IsTrue(items.Count > 0, "No records were returned.");
        }

        [TestMethod]
        public void SubSelectLeftJoin()
        {
            var subSelect = GetSubSelect();

            var items = Syntax.GetQuery()
                .Select(
                    "InvItems.ID"
                    , "InvItems.Code"
                    , "UnitOfMeasures.ID"
                    , "UnitOfMeasures.Code"
                ).From("UnitOfMeasures")
                .LeftJoin(subSelect, "InvItems.UOMID = UnitOfMeasures.ID", "InvItems")
                .PrepareReader<InvItem>()
                .Read();

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.Any(i => i.ID != null && i.UOM.ID != Guid.Empty), "Left join didn't work.");
        }

        [TestMethod]
        public void SubSelectInnerJoin()
        {
            var subSelect = GetSubSelect();

            var items = Syntax.GetQuery()
                .Select(
                    "InvItems.ID"
                    , "InvItems.Code"
                    , "UnitOfMeasures.ID"
                    , "UnitOfMeasures.Code"
                ).From("UnitOfMeasures")
                .InnerJoin(subSelect, "InvItems.UOMID = UnitOfMeasures.ID", "InvItems")
                .PrepareReader<InvItem>()
                .Read();

            Assert.IsTrue(items.Count > 0, "No records were returned.");
            Assert.IsTrue(items.All(i => i.ID != null && i.UOM.ID != Guid.Empty), "Left join didn't work.");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongSyntax;
using System.Threading.Tasks;
using StrongSyntax.DbHelpers;
using StrongSyntax.Tests.Resources.Models;
using System.Linq;

namespace StrongSyntax.Tests
{
    [TestClass]
    public class UpdateTest : TestBase
    {
        [TestMethod]
        public async Task Update()
        {
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

            Assert.AreNotEqual(0, rows, "No records were updated.");
        }

        [TestMethod]
        public void UpdateFromTempTable()
        {
            var itemList = this.GetItems().Take(2);
            var tempTable = Syntax.GetTempTable(itemList);

            var rows = Syntax.GetUpdate()
                .Update("InvItems")
                .Set(new SetClause()
                {
                    { "Name", "Temp.Name" }
                }).From(tempTable, "Temp")
                .Where("InvItems.Code = Temp.Code")
                .Execute();


            Assert.AreNotEqual(0, rows, "No records were updated.");
        }

        [TestMethod]
        public void UpdateFromTempTable_WithoutParams()
        {
            var itemList = this.GetItems();
            var tempTable = Syntax.GetTempTable(itemList, false);

            var rows = Syntax.GetUpdate()
                .Update("InvItems")
                .Set(new SetClause()
                {
                    { "Name", "Temp.Name" }
                }).From(tempTable, "Temp")
                .Where("InvItems.Code = Temp.Code")
                .Execute();

            Assert.AreNotEqual(0, rows, "No records were updated.");
        }
         
        [TestMethod]
        public void UpdatFrom()
        {
            var rows = Syntax.GetUpdate()
                .Update("InvItems")
                .Set(new SetClause()
                {
                    {"InvItems.UnitPrice", 50 }
                }).From("WarehouseItems")
                .InnerJoin("Warehouses", "Warehouses.ID = WarehouseItems.WarehouseID")
                .Where("WarehouseItems.InvItemID = InvItems.ID AND Warehouses.Code = @0", "WH-1")
                .Execute();

            Assert.AreNotEqual(0, rows, "No records were updated.");
        }

        [TestMethod]
        public void UpdateFrom_SubSelect()
        {
            var subSelect = Syntax
                .GetQuery()
                .Select(
                    "WarehouseItems.InvItemID"
                    , "SUM(WarehouseItems.OnHandQty) AS [OnHandQty]"
                ).From("WarehouseItems")
                .GroupBy("WarehouseItems.InvItemID");

            var rows = Syntax.GetUpdate()
                .Update("InvItems")
                .Set(new SetClause()
                {
                    { "InvItems.OnHandQty", "M.OnHandQty" }
                }).From(subSelect, "M")
                .Execute();

            Assert.AreNotEqual(0, rows, "No records were updated.");
        }
    }
}

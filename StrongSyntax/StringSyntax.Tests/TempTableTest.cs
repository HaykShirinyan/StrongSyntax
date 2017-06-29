using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using StrongSyntax.QueryBuilders;
using StrongSyntax.Tests.Resources.Models;
using StrongSyntax.DbHelpers;
using StrongSyntax;

namespace StrongSyntax.Tests
{
    [TestClass]
    public class TempTableTest : TestBase
    {
        [TestMethod]
        public void CreateTable()
        {
            var tempTable = new TempTable<InvItem>();

            //tempTable.CreateTable();

            Assert.IsNotNull(tempTable.Query, "query is null.");
            Assert.IsFalse(string.IsNullOrEmpty(tempTable.RawQuery), "query is empty string.");
        }

        [TestMethod]
        public void Insert()
        {
            var itemList = GetItems();

            var tempTable = new TempTable<InvItem>();            

            int rowsAffected = tempTable.FillTable(itemList);

            Assert.AreNotEqual(0, rowsAffected);
            Assert.IsFalse(string.IsNullOrEmpty(tempTable.RawQuery), "query is empty string.");
        }
    }
}

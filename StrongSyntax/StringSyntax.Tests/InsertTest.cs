using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrongSyntax.Tests
{
    [TestClass]
    public class InsertTest : TestBase
    {
        [TestMethod]
        public void Insert()
        {
            var query = Syntax.GetInsert();

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

            Assert.AreNotEqual(0, rows, "No records were inserted.");
        }

        [TestMethod]
        public void InsertLoop()
        {
            var query = Syntax.GetInsert(false);

            var into = query
                .Insert
                (
                    "Code"
                    , "Name"
                    , "Description"
                    , "Status"
                ).Into("InvItems");

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

            var rows = into.Execute();

            Assert.AreNotEqual(0, rows, "No records were inserted.");
            Assert.AreEqual(rows, 100, "Wrong number of records were inserted.");
        }
    }
}

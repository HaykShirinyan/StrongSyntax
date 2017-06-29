using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrongSyntax.Tests
{
    [TestClass]
    public class DeleteTest : TestBase
    {
        [TestMethod]
        public void Delete()
        {
            var rows = Syntax.GetDelete()
                .Delete()
                .From("InvItems")
                .Where("Code = @0", "1111")
                .Execute();

            Assert.AreNotEqual(0, rows, "No records were deleted.");
        }
    }
}

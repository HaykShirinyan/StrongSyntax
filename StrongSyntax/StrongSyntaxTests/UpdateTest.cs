using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongSyntax;
using System.Threading.Tasks;

namespace StrongSyntaxTests
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

            Assert.AreNotEqual(0, rows);
        }
    }
}

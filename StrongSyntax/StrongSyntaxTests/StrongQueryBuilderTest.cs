using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongSyntax;
using StrongSyntaxTests.Resources.Models;
using StrongSyntaxTests.Resources.DTOs;

namespace StrongSyntaxTests
{
    [TestClass]
    public class StrongQueryBuilderTest : TestBase
    {
        public StrongQueryBuilderTest()
        {
        }

        private InvItemDTO MapToDTO(InvItem model)
        {
            InvItemDTO dto = new InvItemDTO();

            dto.Code = model.Code;

            return dto;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var items = Syntax
                .GetQuery()
                .Select(
                    "InvItems.ID"
                    , "InvItems.Code"
                    , "InvItems.Name"
                    , "InvItems.Description"
                    , "UnitOfMeasures.Name"
                ).From("InvItems")
                .LeftJoin("UnitOfMeasures", "UnitOfMeasures.ID = InvItems.UOMID")
                .PrepareReader<InvItem>()
                .Project(MapToDTO);

            Assert.IsFalse(string.IsNullOrEmpty(items.ToString()));
        }

        [TestMethod]
        public void Select()
        {
            var items = Syntax
                .GetStrongQuery<InvItem>()
                .Select(i => new object[]
                {
                    i.ID
                    ,i.Code
                    ,i.Name
                    ,i.Description
                    ,i.WarehouseItems.Sum(s => s.OnHandQty)
                }).From()
                .LeftJoin<UnitOfMeasure>((i, u) => i.UOMID == u.ID)
                .LeftJoin<WarehouseItem>((i, w) => i.ID == w.InvItemID)
                .GroupBy(i => new object[]
                {
                    i.ID
                    ,i.Code
                    ,i.Name
                    ,i.Description
                });

            Assert.IsNotNull(items);
        }
    }
}

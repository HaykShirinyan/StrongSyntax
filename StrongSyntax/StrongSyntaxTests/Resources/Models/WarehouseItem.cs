using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests.Resources.Models
{
    class WarehouseItem
    {
        public Guid WarehouseID { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        public Guid InvItemID { get; set; }
        public virtual InvItem InvItem { get; set; }

        public decimal? InTransferQty { get; set; }
        public decimal? OnHandQty { get; set; }
    }
}

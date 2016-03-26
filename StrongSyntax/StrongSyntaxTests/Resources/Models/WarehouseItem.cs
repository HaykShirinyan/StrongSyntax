using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests.Resources.Models
{
    class WarehouseItem
    {
        [Key]
        [Column(Order = 0)]
        public Guid WarehouseID { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InvItemID { get; set; }
        public virtual InvItem InvItem { get; set; }

        public decimal? InTransferQty { get; set; }
        public decimal? OnHandQty { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace StrongSyntax.Tests.Resources.Models
{
    public class InvItem : ModelBase
    {      
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? OnSoQty { get; set; }

        public decimal? OnPoQty { get; set; }

        public decimal? OnTransferQty { get; set; }

        public decimal? InWarehouseQty { get; set; }

        public decimal? InStoreQty { get; set; }

        public decimal? OnHandQty { get; set; }

        public decimal? Weight { get; set; }

        public decimal? AvgWeight { get; set; }

        public decimal? Cost { get; set; }

        public decimal? UnitPrice { get; set; }

        public virtual Guid? UOMID { get; set; }

        public virtual UnitOfMeasure UOM { get; set; }

        public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}

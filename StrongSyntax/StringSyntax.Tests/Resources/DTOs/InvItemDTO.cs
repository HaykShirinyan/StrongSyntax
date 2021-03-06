﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntax.Tests.Resources.DTOs
{
    public class InvItemDTO
    {
        public Guid ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? InTranferQty { get; set; }

        public decimal? OnHandQty { get; set; }

        public decimal? OnSoQty { get; set; }

        public decimal? OnPoQty { get; set; }

        public decimal? OnTransferQty { get; set; }

        public decimal? InWarehouseQty { get; set; }

        public decimal? InStoreQty { get; set; }

        public decimal? Weight { get; set; }

        public decimal? AvgWeight { get; set; }

        public decimal? Cost { get; set; }

        public decimal? UnitPrice { get; set; }

        public Guid? UOMID { get; set; }
        public UnitOfMeasureDTO UOM { get; set; }
    }
}

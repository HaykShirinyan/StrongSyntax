﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests.Resources.Models
{
    class Warehouse : ModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<WarehouseItem> InvItems { get; set; }
    }
}

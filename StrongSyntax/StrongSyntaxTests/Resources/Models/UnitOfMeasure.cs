using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests.Resources.Models
{
    class UnitOfMeasure
    {
        public Guid ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<InvItem> InvItems { get; set; }
    }
}

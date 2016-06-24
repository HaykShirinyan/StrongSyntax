using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests.Resources.Models
{
    public class UnitOfMeasure : ModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<InvItem> InvItems { get; set; }
    }
}

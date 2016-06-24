using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongSyntaxTests.Resources.Models
{
    public class ModelBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        [StringLength(100)]
        [Index("IX_Code_UK", IsUnique = true)]
        public string Code { get; set; }

        /// <summary>
        /// In case if we don't delete data from DB for auditing purposes, we set appropriate status.
        /// </summary>
        public RecordStatus Status { get; set; }

        public ModelBase()
        {
            this.Status = RecordStatus.Active;
        }
    }

    public enum RecordStatus
    {
        Active,
        Inactive,
        Deleted
    }
       
}

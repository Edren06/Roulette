using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.DatabaseModels
{
    [Table("NumberAttribute")]
    public class NumberAttribute
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumberAttributeId { get; set; }

        [Required, Index("IDX_Name", IsUnique = true), Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Required, Column(TypeName = "decimal(10, 2)")]
        public decimal PayoutValue { get; set; }
    }
}

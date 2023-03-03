using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.DatabaseModels
{
    [Table("TableItemAttribute")]
    public class TableItemAttribute
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableItemAttributeId { get; set; }

        [ForeignKey("TableItem"), Required]
        public int TableItemId { get; set; }

        [ForeignKey("NumberAttribute"), Required]
        public int NumberAttributeId { get; set; }

    }
}

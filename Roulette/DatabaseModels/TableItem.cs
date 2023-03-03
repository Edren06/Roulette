using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.DatabaseModels
{
    [Table("TableItem")]
    public class TableItem
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableItemId { get; set; }

        [Required, Index("IDX_Name", IsUnique = true), Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Required, Column(TypeName = "bit")]
        public bool IsLandable { get; set; }

        [ForeignKey("NumberAttributeId"), Required]
        public int NumberAttributeId { get; set; }

    }
}

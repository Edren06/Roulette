using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.DatabaseModels
{
    [Table("Spin")]
    public class Spin
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpinId { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime SpinDate { get; set; }

        [ForeignKey("TableItemId")]
        public int TableItemId { get; set; }
    }
}

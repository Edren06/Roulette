using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.DatabaseModels
{
    [Table("Bet")]
    public class Bet
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BetId { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime BetDate { get; set; }

        [Required, Column(TypeName = "decimal(10, 2)")]
        public Decimal Amount { get; set; }

        [ForeignKey("TableItemId")]
        public int TableItemId { get; set; }

        [ForeignKey("SpinId")]
        public int? SpinId { get; set; }

        [Required, Column(TypeName = "decimal(10, 2)")]
        public Decimal Payout { get; set; }
    }
}

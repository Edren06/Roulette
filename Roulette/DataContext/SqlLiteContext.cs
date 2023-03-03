
using Roulette.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Roulette.DataContext
{
    public class SqlLiteContext : DbContext
    {
        public SqlLiteContext() { }
        public SqlLiteContext(DbContextOptions<SqlLiteContext> options) : base(options) { }
        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<NumberAttribute> NumberAttributes { get; set; }
        public virtual DbSet<Spin> Spins { get; set; }
        public virtual DbSet<TableItem> TableItems { get; set; }
        public virtual DbSet<TableItemAttribute> TableItemAttributes { get; set; }
    }
}

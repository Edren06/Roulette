using Microsoft.EntityFrameworkCore;
using Roulette.DatabaseModels;
using Roulette.DataContext;
using Roulette.Interfaces;
using Roulette.Models;

namespace Roulette.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly SqlLiteContext _dbContext;

        public RouletteService()
        {

        }
        public RouletteService(SqlLiteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BetModel> PlaceBet(BetModel bet)
        {
            var tableItem = await this.GetTableItemByName(bet.TableItemName);

            if (tableItem == null)
                throw new ApplicationException("There is no such space to place a bet.");

            if (bet == null || !(bet.Amount > 0))
                throw new ApplicationException("Invalid bet amount");

            Bet newBet = new Bet()
            {
                Amount = bet.Amount,
                SpinId = null,
                TableItemId = tableItem.TableItemId,
                BetDate = DateTime.Now
            };

            _dbContext.Bets.Add(newBet);
            _dbContext.SaveChanges();

            return bet;
        }

        public async Task<Spin> Spin()
        { 
            Spin spin = new Spin()
            {
                SpinDate = DateTime.Now,
                TableItemId = this.GetWhereBallLanded().Result.TableItemId
            };

            _dbContext.Spins.Add(spin);
            _dbContext.SaveChanges(); 

            return spin;
        }

        public async Task<TableItem> GetWhereBallLanded()
        {
            Random rand = new Random();
            var validSpaces = await this.GetValidLandingSpots();
            int skipNumber = rand.Next(0, validSpaces.Count());

            return validSpaces.Skip(skipNumber).Take(1).First();
        }

        public async Task<List<Bet>> GetPayoutsFromLastSpin()
        {
            var lastBets = GetLatestBets().Result;
            var spin = GetLatestSpin().Result;
            
            foreach (var selectedBet in lastBets ?? new List<Bet>())
            {
                this.CalculateBetPayout(selectedBet, spin);
            }

            _dbContext.SaveChanges();

            return lastBets;
        }

        public Bet CalculateBetPayout(Bet bet, Spin spin)
        {
            var betValues = this.GetAttributeFromTableItem(bet.TableItemId).Result;
            var tableValue = this.GetAttributeFromTableItemAttributes(spin.TableItemId, betValues.NumberAttributeId).Result;

            bet.SpinId = spin.SpinId;
            bet.Payout = 0;

            if (betValues != null && tableValue != null)
            {
                if (bet.TableItemId == spin.TableItemId)
                {
                    bet.Payout = bet.Amount * this.GetNumberAttribute(tableValue.NumberAttributeId).Result.PayoutValue;
                }
                else if (bet.TableItemId != spin.TableItemId && tableValue.NumberAttributeId == betValues.NumberAttributeId && tableValue.NumberAttributeId != 1)
                {
                    bet.Payout = bet.Amount * this.GetNumberAttribute(betValues.NumberAttributeId).Result.PayoutValue;
                }
                else
                {
                    bet.Payout = 0;
                }
            }

            return bet;
        }

        public async Task<List<Spin>> GetSpinHistory()
        {
            return await _dbContext.Spins.OrderByDescending(x => x.SpinDate).ToListAsync();
        }

        public async Task<TableItem> GetTableItemByName(string tableItemName)
        {
            return await _dbContext.TableItems.Where(x => x.Name == tableItemName).FirstOrDefaultAsync();
        }

        public async Task<List<TableItem>> GetValidLandingSpots()
        {
            return await _dbContext.TableItems.Where(x => x.IsLandable == true).ToListAsync();
        }

        public async Task<TableItemAttribute> GetAttributeFromTableItemAttributes(int tableItemId, int numberAttributeId)
        {
            return await _dbContext.TableItemAttributes.Where(x => x.TableItemId == tableItemId && x.NumberAttributeId == numberAttributeId).FirstOrDefaultAsync();
        }

        public async Task<TableItem> GetAttributeFromTableItem(int tableItemId)
        {
            return await _dbContext.TableItems.Where(x => x.TableItemId == tableItemId).FirstOrDefaultAsync();
        }

        public async Task<NumberAttribute> GetNumberAttribute(int numberAttributeId)
        {
            return await _dbContext.NumberAttributes.Where(x => x.NumberAttributeId == numberAttributeId).SingleOrDefaultAsync();
        }

        public async Task<Spin> GetLatestSpin()
        {
            return await _dbContext.Spins.OrderByDescending(x => x.SpinId).Take(1).SingleOrDefaultAsync();
        }

        public async Task<List<Bet>> GetLatestBets()
        {
            return await _dbContext.Bets.Where(x => x.SpinId == null).ToListAsync();
        }
    }
}

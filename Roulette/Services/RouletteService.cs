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

        /// <summary>
        /// Placing the bet will pass through a bet model object from the API which will save into the bet table
        /// </summary>
        /// <param name="bet"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
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

        /// <summary>
        /// Spin will randomly select table items where the ball is land-able and save it into the database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        //  Function to find where the ball has landed. 
        /// </summary>
        /// <returns></returns>
        public async Task<TableItem> GetWhereBallLanded()
        {
            Random rand = new Random();
            var validSpaces = await this.GetValidLandingSpots();
            int skipNumber = rand.Next(0, validSpaces.Count());

            return validSpaces.Skip(skipNumber).Take(1).First();
        }

        /// <summary>
        /// This function will check all bets with a null spin id (Bets placed before a spin) and assign a payout value to the bet.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Function to calculate the payout
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="spin"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all the records in the spin history
        /// </summary>
        /// <returns></returns>
        public async Task<List<Spin>> GetSpinHistory()
        {
            return await _dbContext.Spins.OrderByDescending(x => x.SpinDate).ToListAsync();
        }

        /// <summary>
        /// We get the tableItemId by the table item name
        /// </summary>
        /// <param name="tableItemName"></param>
        /// <returns></returns>
        public async Task<TableItem> GetTableItemByName(string tableItemName)
        {
            return await _dbContext.TableItems.Where(x => x.Name == tableItemName).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Function to get all the valid places where the ball can land
        /// </summary>
        /// <returns></returns>
        public async Task<List<TableItem>> GetValidLandingSpots()
        {
            return await _dbContext.TableItems.Where(x => x.IsLandable == true).ToListAsync();
        }

        /// <summary>
        /// Get's the attributes of a table item, whether it fits into a particular category this is used for the non singular numbers
        /// </summary>
        /// <param name="tableItemId"></param>
        /// <param name="numberAttributeId"></param>
        /// <returns></returns>
        public async Task<TableItemAttribute> GetAttributeFromTableItemAttributes(int tableItemId, int numberAttributeId)
        {
            return await _dbContext.TableItemAttributes.Where(x => x.TableItemId == tableItemId && x.NumberAttributeId == numberAttributeId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the attributes of the table item, whether it fits into a particular category, this is used for singular numbers
        /// </summary>
        /// <param name="tableItemId"></param>
        /// <returns></returns>
        public async Task<TableItem> GetAttributeFromTableItem(int tableItemId)
        {
            return await _dbContext.TableItems.Where(x => x.TableItemId == tableItemId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get an attribute of a number
        /// </summary>
        /// <param name="numberAttributeId"></param>
        /// <returns></returns>
        public async Task<NumberAttribute> GetNumberAttribute(int numberAttributeId)
        {
            return await _dbContext.NumberAttributes.Where(x => x.NumberAttributeId == numberAttributeId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// This gets the last spin that occured.
        /// </summary>
        /// <returns></returns>
        public async Task<Spin> GetLatestSpin()
        {
            return await _dbContext.Spins.OrderByDescending(x => x.SpinId).Take(1).SingleOrDefaultAsync();
        }

        /// <summary>
        /// This gets all the bets that have not been assigned to a spin (considered the latest bets).
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bet>> GetLatestBets()
        {
            return await _dbContext.Bets.Where(x => x.SpinId == null).ToListAsync();
        }
    }
}

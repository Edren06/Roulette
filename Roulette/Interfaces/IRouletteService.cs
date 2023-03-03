using Roulette.DatabaseModels;
using Roulette.Models;

namespace Roulette.Interfaces
{
    /// <summary>
    /// Interface for the roulette service in order for it to be injected.
    /// </summary>
    public interface IRouletteService
    {
        Task<BetModel> PlaceBet(BetModel bet);
        Task<Spin> Spin();
        Task<List<Spin>> GetSpinHistory();
        Task<List<Bet>> GetPayoutsFromLastSpin();
        Bet CalculateBetPayout(Bet bet, Spin spin);
        Task<TableItem> GetTableItemByName(string tableItemName);
        Task<List<TableItem>> GetValidLandingSpots();
        Task<TableItemAttribute> GetAttributeFromTableItemAttributes(int tableItemId, int numberAttributeId);
        Task<TableItem> GetAttributeFromTableItem(int tableItemId);
        Task<NumberAttribute> GetNumberAttribute(int numberAttributeId);
        Task<Spin> GetLatestSpin();
        Task<List<Bet>> GetLatestBets();

    }
}

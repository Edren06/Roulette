using Microsoft.AspNetCore.Mvc;
using Roulette.DataContext;
using Roulette.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Roulette.Models;
using Roulette.Interfaces;

namespace Roulette.Controllers
{
    /// <summary>
    /// All the api calls required for the roulette machine
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteService _rouletteService;

        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService;
        }

        [Route("PlaceBet")]
        [HttpPost]
        public async Task<ActionResult<BetModel>> PlaceBet(BetModel bet)
        {
            return Ok(await _rouletteService.PlaceBet(bet));
        }

        [Route("Spin")]
        [HttpGet]
        public async Task<ActionResult<Spin>> Spin()
        {
            return Ok(await _rouletteService.Spin());
        }

        [Route("GetSpinHistory")]
        [HttpGet]
        public async Task<ActionResult<List<Spin>>> GetSpinHistory()
        {
            return Ok(await _rouletteService.GetSpinHistory());
        }

        [Route("Payout")]
        [HttpGet]
        public async Task<ActionResult<List<Bet>>> Payout()
        {
            return Ok(await _rouletteService.GetPayoutsFromLastSpin());
        }
    }
}

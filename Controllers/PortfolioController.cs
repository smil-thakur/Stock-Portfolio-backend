using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Extensions;
using SampleAPI.Interfaces;
using SampleAPI.Models;
using SampleAPI.Repository;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository) : ControllerBase
    {
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly UserManager<AppUser> _userManager = userManager;

        private readonly IPortfolioRepository _portfolioRepository = portfolioRepository;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPotfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string Symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(Symbol);

            if (stock == null)
            {
                return BadRequest("Stock not found");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol.Equals(stock.Symbol, StringComparison.CurrentCultureIgnoreCase)))
            {
                return BadRequest("Stock is already added");
            }

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id,
            };

            await _portfolioRepository.CreateAsync(portfolioModel);
            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create the portfolio");
            }
            else
            {
                return Ok("created");
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var Username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(Username);

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol == symbol).ToList();

            if (filteredStock.Count != 0)
            {
                await _portfolioRepository.DeleteAsync(appUser, symbol);
            }
            else
            {
                return BadRequest("stock not in you portfolio");
            }

            return Ok();

        }
    }
}
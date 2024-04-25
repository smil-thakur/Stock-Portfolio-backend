using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Data;
using SampleAPI.Interfaces;
using SampleAPI.Models;

namespace SampleAPI.Repository
{
    public class PorfolioRepository(ApplicationDBContext context) : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol == symbol);
            if (portfolioModel == null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id).Select(Stock => new Stock
            {
                Id = Stock.StockId,
                Symbol = Stock.Stock.Symbol,
                CompanyName = Stock.Stock.CompanyName,
                Purchase = Stock.Stock.Purchase,
                LastDiv = Stock.Stock.LastDiv,
                Industry = Stock.Stock.Industry,
                MarketCap = Stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}
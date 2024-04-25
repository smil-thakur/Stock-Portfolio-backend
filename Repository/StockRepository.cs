using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Data;
using SampleAPI.DTOs.Stock;
using SampleAPI.Helpers;
using SampleAPI.Interfaces;
using SampleAPI.Mappers;
using SampleAPI.Models;

namespace SampleAPI.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int Id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == Id);
            if (stock == null)
            {
                return null;
            }
            _context.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;


        }

        public async Task<List<Stock>> GetAllAysnc(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.appUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int Id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Stock?> GetBySymbolAsync(string Symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == Symbol);
        }

        public Task<bool> StockExists(int Id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == Id);
        }

        public async Task<Stock?> UpdateAsync(int Id, UpdateStockRequestDto stockRequestDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == Id);
            if (stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = stockRequestDto.Symbol;
            stockModel.CompanyName = stockRequestDto.CompanyName;
            stockModel.Purchase = stockRequestDto.Purchase;
            stockModel.LastDiv = stockRequestDto.LastDiv;
            stockModel.Industry = stockRequestDto.Industry;
            stockModel.MarketCap = stockRequestDto.MarketCap;

            await _context.SaveChangesAsync();

            return stockModel;

        }
    }
}
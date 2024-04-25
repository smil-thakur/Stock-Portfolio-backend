using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.DTOs.Stock;
using SampleAPI.Helpers;
using SampleAPI.Models;

namespace SampleAPI.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAysnc(QueryObject query);

        Task<Stock?> GetByIdAsync(int Id);

        Task<Stock?> GetBySymbolAsync(string Symbol);

        Task<Stock> CreateAsync(Stock stockModel);

        Task<Stock?> UpdateAsync(int Id, UpdateStockRequestDto stockRequestDto);

        Task<Stock?> DeleteAsync(int Id);

        Task<bool> StockExists(int Id);

    }
}
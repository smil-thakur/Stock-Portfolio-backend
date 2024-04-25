using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleAPI.DTOs.Stock;
using SampleAPI.Models;

namespace SampleAPI.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Commments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDto(this CreatestockRequestDto stockModel)
        {
            return new Stock
            {
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                Purchase = stockModel.Purchase,
                MarketCap = stockModel.MarketCap,
            };
        }


    }
}
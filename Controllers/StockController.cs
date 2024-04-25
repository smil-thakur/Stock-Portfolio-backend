using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SampleAPI.Data;
using SampleAPI.DTOs.Stock;
using SampleAPI.Helpers;
using SampleAPI.Interfaces;
using SampleAPI.Mappers;

namespace SampleAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController(IStockRepository stockRepository) : ControllerBase
    {
        private readonly IStockRepository _stockRepository = stockRepository;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var stocks = await _stockRepository.GetAllAysnc(query);
            var stocksDto = stocks.Select(stk => stk.ToStockDto()).ToList();

            if (stocks.Count == 0)
            {
                return NotFound();
            }
            return Ok(stocksDto);
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatestockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var stockModel = stockDto.ToStockFromCreateDto();
            var stock = await _stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new
            {
                id = stock.Id
            },
            stock.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var stockModel = await _stockRepository.DeleteAsync(Id);

            if (stockModel == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}

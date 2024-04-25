using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.DTOs.Stock
{
    public class CreatestockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be more than 10 letter long")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Company cannot be more than 100 letter long")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10 letters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 5000000000000)]
        public long MarketCap { get; set; }
    }
}
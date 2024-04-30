using MicroserviceDatabase.DTO;
using MicroserviceDatabase.Extensions;
using MicroserviceDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MicroserviceDatabase.Controllers
{
    [Route("api/Prices")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private SahkoDbContext _dbContext;
        private ILogger<DbController> _logger;

        public DbController(SahkoDbContext dbContext, ILogger<DbController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PriceDTO content)
        {
            if (content == null)
            {
                return BadRequest("Dataa ei vastaanotettu");
            }

            try
            {
                foreach (var x in content.prices)
                {
                    _dbContext.Prices.Add(x.ToEntity());
                }

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ei toimi");

                return StatusCode(StatusCodes.Status500InternalServerError, "No bueno");
                
            }

            return Ok("Data vastaanotettu ja käsitelty");
        }

        [HttpDelete("{startDate}")]
        public async Task<IActionResult> Delete(DateTime startDate)
        {
            var result = await _dbContext.Prices.FindAsync(startDate);
            if (result == null)
            {
                return NotFound();
            }

            _dbContext.Prices.Remove(result);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetPrices")]
        public async Task<ActionResult> GetPrices()
        {
            var results = await _dbContext.Prices.ToListAsync();
            if (results == null)
            {
                return NotFound();
            }
            return Ok(results);
        }

        [HttpGet("GetPricesFromRange")]
        public async Task<ActionResult> GetPricesFromRange([FromQuery] DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var results = await _dbContext.Prices
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (results == null)
            {
                return NotFound();
            }
            return Ok(results);
        }

        [HttpPut("{startDate}")]
        public async Task<IActionResult> Put(DateTime startDate, [FromBody] ElectricityData data)
        {
            if (startDate != data.StartDate)
            {
                return BadRequest();
            }

            var result = await _dbContext.Prices.FindAsync(startDate);
            if (result == null)
            {
                return NotFound();
            }

            _dbContext.Entry(result).CurrentValues.SetValues(data);
            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }

        [HttpGet("GetPriceDifferenceFromRange")]
        public async Task<IActionResult> GetPriceDifferenceFromRange([FromQuery] DateTime startDate, DateTime endDate, decimal fixedPrice)
        {
            var marketPrices = _dbContext.Prices
                .Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();

            decimal avg = marketPrices.Average(x => x.Price);

            decimal difference = avg - fixedPrice;

            if (difference < 0)
            {
                return Ok($"Pörssi sähkö on {Math.Abs(difference)}€ halvempi valitulla aikavälillä");
            }
            else 
            {
                return Ok($"Pörssi sähkö on {Math.Abs(difference)}€ kalliimpi valitulla aikavälillä");
            }

        }
    }
}

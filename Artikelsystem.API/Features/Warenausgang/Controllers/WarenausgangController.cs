using Artikelsystem.API.Features.Warenausgang.Service;
using Artikelsystem.API.Shared.Controllers;
using Artikelsystem.Shared;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Filter;
using Artikelsystem.Shared.DTOs.Warenausgang.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Artikelsystem.API.Features.Warenausgang.Controllers
{
    public class WarenausgangController : BaseController
    {
        private readonly ILogger<WarenausgangController> _logger;
        private readonly IWarenausgangService _service;

        public WarenausgangController(ILogger<WarenausgangController> logger, IWarenausgangService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDTO<WarenausgangDto>>> GetWarenausgaengeAsync([FromQuery] WarenausgangFilterDto filter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            _logger.LogInformation($"GetWarenausgaengeAsync wurde Aufgerufen mit dem Filter: {@filter}, pageNumber: {pageNumber}, pageSize: {pageSize}", filter, pageNumber, pageSize);
            var result = await _service.GetWarenausgaengeAsync(filter, pageNumber, pageSize);
            if(result.Items == null || !result.Items.Any())
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WarenausgangDto>> GetWarenausgangByIdAsync(int id)
        {
            _logger.LogInformation($"GetWarenausgangByIdAsync wurde Aufgerufen mit der ID: {id}", id);
            var result = await _service.GetWarenausgangByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
    }
}

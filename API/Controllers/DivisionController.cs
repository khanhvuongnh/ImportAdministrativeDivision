using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DivisionController : ControllerBase
    {
        private readonly IDivisionService _divisionService;

        public DivisionController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        [HttpGet("Provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var result = await _divisionService.GetProvinces();
            return Ok(result);
        }

        [HttpGet("Districts")]
        public async Task<IActionResult> GetDistricts([FromQuery] string provinceID)
        {
            var result = await _divisionService.GetDistricts(provinceID);
            return Ok(result);
        }

        [HttpGet("Wards")]
        public async Task<IActionResult> GetWards([FromQuery] string districtID)
        {
            var result = await _divisionService.GetWards(districtID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile excelFile)
        {
            var result = await _divisionService.UploadExcel(excelFile);
            return Ok(result);
        }
    }
}
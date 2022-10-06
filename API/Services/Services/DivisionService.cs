using API.Data;
using API.Helpers.Utilities;
using API.Models;
using API.Services.Interfaces;
using Aspose.Cells;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Services
{
    public class DivisionService : IDivisionService
    {
        private readonly DBContext _context;
        private readonly IFunctionUtility _functionUtility;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DivisionService(
            IFunctionUtility functionUtility,
            IWebHostEnvironment webHostEnvironment,
            DBContext context)
        {
            _context = context;
            _functionUtility = functionUtility;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<KeyValuePair<string, string>>> GetDistricts(string provinceID)
        {
            if (string.IsNullOrEmpty(provinceID))
                return new List<KeyValuePair<string, string>>();

            var result = await _context.District
                .Where(x => x.ProvinceID == provinceID)
                .OrderBy(x => x.DistrictName)
                .Select(x => new KeyValuePair<string, string>(x.DistrictID, x.DistrictName))
                .ToListAsync();

            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetProvinces()
        {
            var result = await _context.Province
                .OrderBy(x => x.ProvinceName)
                .Select(x => new KeyValuePair<string, string>(x.ProvinceID, x.ProvinceName))
                .ToListAsync();

            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetWards(string districtID)
        {
            if (string.IsNullOrEmpty(districtID))
                return new List<KeyValuePair<string, string>>();

            var result = await _context.Ward
                .Where(x => x.DistrictID == districtID)
                .OrderBy(x => x.WardName)
                .Select(x => new KeyValuePair<string, string>(x.WardID, x.WardName))
                .ToListAsync();

            return result;
        }

        public async Task<OperationResult> UploadExcel(IFormFile excelFile)
        {
            if (excelFile == null)
                return new OperationResult(false, "DATA_NOT_FOUND");

            var subfolder = Path.Combine("uploaded", "excel");
            var fileName = await _functionUtility.UploadAsync(excelFile, subfolder, "default");

            if (fileName == null)
                return new OperationResult(false, "UPLOAD_FAILED");

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, subfolder, fileName);

            var designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(filePath);
            var ws = designer.Workbook.Worksheets[0];
            int rows = ws.Cells.MaxDataRow;
            var excelModels = new List<ExcelModel>();

            for (int i = 1; i <= rows; i++)
            {
                var model = new ExcelModel
                {
                    ProvinceName = ws.Cells[i, 0]?.StringValue,
                    ProvinceID = ws.Cells[i, 1]?.StringValue,
                    DistrictName = ws.Cells[i, 2]?.StringValue,
                    DistrictID = ws.Cells[i, 3]?.StringValue,
                    WardName = ws.Cells[i, 4]?.StringValue,
                    WardID = ws.Cells[i, 5]?.StringValue,
                    WardLevel = ws.Cells[i, 6]?.StringValue,
                    WardEnglishName = ws.Cells[i, 7]?.StringValue,
                };

                excelModels.Add(model);
            }

            var wards = excelModels
                .Where(x => !string.IsNullOrEmpty(x.WardID))
                .Select(x => new Ward
                {
                    WardID = x.WardID!,
                    WardName = x.WardName!,
                    WardLevel = x.WardLevel,
                    WardEnglishName = x.WardEnglishName,
                    DistrictID = x.DistrictID!
                }).ToList();

            var districts = excelModels
                .Where(x => !string.IsNullOrEmpty(x.DistrictID))
                .GroupBy(x => x.DistrictID)
                .Select(x => new District
                {
                    DistrictID = x.Key!,
                    DistrictName = x.FirstOrDefault()?.DistrictName!,
                    ProvinceID = x.FirstOrDefault()?.ProvinceID!
                });

            var provinces = excelModels
                .Where(x => !string.IsNullOrEmpty(x.ProvinceID))
                .GroupBy(x => x.ProvinceID)
                .Select(x => new Province
                {
                    ProvinceID = x.Key!,
                    ProvinceName = x.FirstOrDefault()?.ProvinceName!
                });

            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Ward");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE District");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Province");

            _context.Ward.AddRange(wards);
            _context.District.AddRange(districts);
            _context.Province.AddRange(provinces);

            await _context.SaveChangesAsync();

            return new OperationResult(true);
        }
    }
}
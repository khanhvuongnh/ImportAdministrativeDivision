using API.Helpers.Utilities;

namespace API.Services.Interfaces
{
    public interface IDivisionService
    {
        Task<List<KeyValuePair<string, string>>> GetDistricts(string provinceID);
        Task<List<KeyValuePair<string, string>>> GetProvinces();
        Task<List<KeyValuePair<string, string>>> GetWards(string districtID);
        Task<OperationResult> UploadExcel(IFormFile excelFile);
    }
}
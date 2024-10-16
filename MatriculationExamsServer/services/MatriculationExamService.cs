using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MatriculationExamsServer.services
{
    public class MatriculationExamService
    {
        private readonly GoogleSheetApiService _googleSheetApiService;
        public MatriculationExamService(GoogleSheetApiService googleSheetApiService)
        {
            _googleSheetApiService = googleSheetApiService;
        }
        //public Task<IActionResult> GetExamScores(string userId)
        //{
        //    string spreadsheetId = "1_ujxbpru42Pb0NU9kN7y-YyrheMzapDiTE0uSR--k5M";
        //    var data = await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, );
        //    data = data.ToList();
        //    var row = data[0];
        //    int g = row.ToList().FindIndex(x => x != null && x.ToString().Contains("אנגלית"));
        //    return data;
        //}
    }
}

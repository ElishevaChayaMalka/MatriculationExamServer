using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Util;
using MatriculationExamsServer.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using static MatriculationExamsServer.Types.Enums;
using System;

namespace MatriculationExamsServer.Controllers
{
    [Route("Matriculation")]
    [ApiController]
    public class MatriculationExamController : ControllerBase
    {
        private readonly GoogleSheetApiService _googleSheetApiService;
        private readonly string _spreadsheetId = "1_ujxbpru42Pb0NU9kN7y-YyrheMzapDiTE0uSR--k5M";

        public MatriculationExamController(GoogleSheetApiService googleSheetApiService)
        {
            _googleSheetApiService = googleSheetApiService;
        }

        [HttpGet("GetFirstColumn")]
        public async Task<IActionResult> GetFirstColumn()
        {

            string sheetName = Enum.GetName(typeof(SheetClassesName), SheetClassesName.TwelfthGrade);
            string description = typeof(SheetClassesName).GetField(sheetName).GetCustomAttribute<DescriptionAttribute>().Description;

            string range = $"{description}!A:A";
            var data = await _googleSheetApiService.GetSheetDataAsync(_spreadsheetId, range);

            if (data != null && data.Count > 0)
            {
                return Ok(data);
            }

            return NotFound("No data found.");
        }
        [HttpGet("getClasses")]
        public async Task<IActionResult> getListClassesSheet()
        {
            var sheetNames = await _googleSheetApiService.GetAllSheetNamesAsync(_spreadsheetId);
            return Ok(sheetNames);
        }




    }
}

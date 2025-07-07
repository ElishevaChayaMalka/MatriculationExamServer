using Google.Apis.Sheets.v4.Data;
using MatriculationExamsServer.DBs;
using MatriculationExamsServer.DTO;
using static MatriculationExamsServer.Types.Enums;
using System.ComponentModel;
using System;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections.Immutable;
using Google.Apis.Sheets.v4;
using System.Drawing;


namespace MatriculationExamsServer.services
{
    public class LoginService
    {
        private readonly GoogleSheetApiService _googleSheetApiService;
        private readonly ColorService _colorService;

        public LoginService(GoogleSheetApiService googleSheetApiService, ColorService colorService)
        {
            _googleSheetApiService = googleSheetApiService;
            _colorService = colorService;
        }
        //public async Task<User> GetExamination10th(UserDTO user)
        //{
        //    string spreadsheetId = "1_ujxbpru42Pb0NU9kN7y-YyrheMzapDiTE0uSR--k5M";

        //    // if (user.ClassName == "י")

        //    string sheetName = Enum.GetName(typeof(SheetClassesName), SheetClassesName.TenthGradeAlternativeAssessments);
        //    string sheet = typeof(SheetClassesName).GetField(sheetName).GetCustomAttribute<DescriptionAttribute>().Description;
        //    string range = $"{sheet}!B:B";
        //    var data = await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, range);
        //    int index = data.ToList().FindIndex(x => x.Contains(user.Id)) + 1;
        //    range = $"{sheet}!C{index}:D{index}";
        //    var name = await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, range);
        //    var b = name.ToList()[0];
        //    string n = string.Join(" ", b);
        //    User u = new User() { Id = user.Id, Name = n };
        //    return   u;
        //}

        public async Task<User> GetUser(UserDTO user ,string sheet,string rangeId,string rangeData)
        {
            try
            {

                string spreadsheetId = "1_ujxbpru42Pb0NU9kN7y-YyrheMzapDiTE0uSR--k5M";
                var data = await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, rangeId);
                string index = (data.ToList().FindIndex(x => x.Contains(user.Id)) + 1).ToString();
                rangeData = rangeData.Replace("1", index);
                var userDetails = await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, rangeData);
                var fullName= userDetails.ToList()[0];
                string name = string.Join(" ", fullName);
                User userData = new User() { Id = user.Id, Name = name };
                return userData;
            }
             catch (Google.GoogleApiException ex) { 
                return null;
            }
        }

        public async Task<IList<Exam>> GetExamResults(string rangeData,string rangDataExam, Dictionary<string, string> ranges,string id, string classNameSheet)
        {

            string spreadsheetId = "1_ujxbpru42Pb0NU9kN7y-YyrheMzapDiTE0uSR--k5M";
            var currentSubject = "";
            var subjects =(await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, rangeData)).ToList();
            var rangeUserRow = classNameSheet+ ranges["RangeIDGrade"];
            var row = (await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, rangeUserRow)).ToList();
            string index = (row.ToList().FindIndex(x => x.Contains(id)) + 1).ToString();
            var examsRange = rangDataExam.Replace("1", index);
            var scores =  (await _googleSheetApiService.GetSheetDataAsync(spreadsheetId, examsRange)).ToList();
            var colorCells =await _googleSheetApiService.GetRangeBackgroundColorsAsync(spreadsheetId, examsRange);
            List<Exam> exams = new List<Exam>();
        

            for (int i = 0; i < subjects[0].Count()&&i<colorCells.Count();i++)
            {
                if (subjects[0][i] != "")
                {
                    currentSubject = subjects[0][i].ToString();

                }

                exams.Add(new Exam(i < scores[0].Count() ? scores[0][i].ToString() : "", currentSubject, _colorService.GetColorName(colorCells[i].Red, colorCells[i].Green, colorCells[i].Blue) ,subjects[1][i].ToString()));
        }

                   return exams;
        }
    public static string GetColumnName(int columnNumber)
    {
        string columnName = "";
        while (columnNumber > 0)
        {
            int remainder = (columnNumber - 1) % 26;
            columnName = (char)(remainder + 'A') + columnName;
                columnNumber = (columnNumber - 1) / 26;
            }
            return columnName;
        }

        public static void IterateColumns(string startColumn, string endColumn)
        {
            int start = GetColumnIndex(startColumn);
            int end = GetColumnIndex(endColumn);

            for (int i = start; i <= end; i++)
            {
                string columnName = GetColumnName(i);
                Console.WriteLine($"Processing column: {columnName}");
           
            }
        }

        public static int GetColumnIndex(string columnName)
        {
            int columnIndex = 0;
            for (int i = 0; i < columnName.Length; i++)
            {
                columnIndex = columnIndex * 26 + (columnName[i] - 'A' + 1);
            }
            return columnIndex;
        }


    }
}

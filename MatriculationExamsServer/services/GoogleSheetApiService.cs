using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using System.Net;

using System.IO;

namespace MatriculationExamsServer.services
{
    public class GoogleSheetApiService
    {
      private readonly SheetsService _sheetsService;

        public GoogleSheetApiService()
        {
            // var credential = GoogleCredential.FromFile(credentialsPath).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
            string jsonCredentials = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS");
            if (string.IsNullOrEmpty(jsonCredentials))
            {
                throw new InvalidOperationException("GOOGLE_CREDENTIALS environment variable is missing or empty.");
            }
            GoogleCredential credential = GoogleCredential.FromJson(jsonCredentials)
                .CreateScoped(SheetsService.Scope.Spreadsheets);

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "matriculationexams",
            });
        }

        public async Task<IList<IList<object>>> GetSheetDataAsync(string spreadsheetId, string range)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = await request.ExecuteAsync();
            return response.Values;
        }
        public async Task<List<Color>> GetRangeBackgroundColorsAsync(string spreadsheetId, string range)
        {
            var request = _sheetsService.Spreadsheets.Get(spreadsheetId);
            request.Ranges = new List<string> { range };
            request.IncludeGridData = true;

            var response = await request.ExecuteAsync();
            var colors = new List<Color>();

            if (response.Sheets.Count > 0 && response.Sheets[0].Data.Count > 0)
            {
                var rows = response.Sheets[0].Data[0].RowData;

                foreach (var row in rows)
                {
                    if (row.Values != null)
                    {
                        foreach (var cell in row.Values)
                        {
                            var backgroundColor = cell.EffectiveFormat?.BackgroundColor;
                            if (backgroundColor != null)
                            {
                                colors.Add(backgroundColor);
                            }
                            else
                            {
                                colors.Add(new Color { Red = 1, Green = 1, Blue = 1, Alpha = 1 });
                            }
                        }
                    }
                }
            }

            return colors;
        }



    }

}

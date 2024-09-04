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

        public GoogleSheetApiService(string credentialsPath, string applicationName)
        {
            var credential = GoogleCredential.FromFile(credentialsPath).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        public async Task<IList<IList<object>>> GetSheetDataAsync(string spreadsheetId, string range)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();
            return response.Values;
        }

    }

}

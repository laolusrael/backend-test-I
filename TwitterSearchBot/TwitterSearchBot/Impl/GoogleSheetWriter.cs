using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterSearchBot.Contracts;
using TwitterSearchBot.Types;

namespace TwitterSearchBot.Impl
{
    public class GoogleSheetWriter : IProfileSheet
    {
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string _appName = "TwitterSearchBot";
        private readonly string _spreadSheetId;

        public GoogleSheetWriter(string spreadsheetId)
        {
            _spreadSheetId = spreadsheetId;
        }


        private UserCredential _GetCredential()
        {

            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, @".credentials\sheets.googleapis.com-dotnet-twitter-followers.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }


            return credential;

        }

        public void WriteProfile(FollowData rowValue)
        {
            if (rowValue == null)
                return;


            var data =  new List<object> { rowValue.TwitterUsername, rowValue.NumOfFollowers};

            var sheetService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = _GetCredential(),
                ApplicationName = _appName
            });

            var range = "A2:B";

            var valueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            var insertOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;

            ValueRange body = new ValueRange();

            body.MajorDimension = "ROWS";
            body.Range = range;
            body.Values = new List<IList<object>> { data };

            SpreadsheetsResource.ValuesResource.AppendRequest request = sheetService.Spreadsheets.Values.Append(body, _spreadSheetId, range);

            request.ValueInputOption = valueInputOption;

            request.InsertDataOption = insertOption;

            AppendValuesResponse response = request.Execute();
        }
        public void WriteProfile(List<FollowData> rows)
        {
            if (rows == null)
                return;


            if (rows.Any() == false)
                return;

            var data = rows.Select(r => new { r.TwitterUsername, r.NumOfFollowers }).ToList(); 

            var sheetService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = _GetCredential(),
                ApplicationName = _appName
            });

            var range = "A2:B";

            var valueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            var insertOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;

            ValueRange body = new ValueRange();

            body.MajorDimension = "ROWS";
            body.Range = range;
            body.Values = new List<IList<object>> { data.ToList<object>() };

            SpreadsheetsResource.ValuesResource.AppendRequest request = sheetService.Spreadsheets.Values.Append(body, _spreadSheetId, range);

            request.ValueInputOption = valueInputOption;
            
            request.InsertDataOption = insertOption;

            AppendValuesResponse response = request.Execute();
        }
    }
}

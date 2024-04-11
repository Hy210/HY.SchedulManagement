using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ScheduleHelper
{
    public class GoogleCalendar
    {
        private string[] _scopes { get; set; } = { CalendarService.Scope.Calendar };
        private string _credPath = "token.json";
        public List<ScheduleItem> GetCalendarItems()
        {

            //Utility.GetHolidays();
            var credential = GetUserCredential();

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Scheduler"
            });
            var list = service.CalendarList.List().Execute();
            var items = list.Items;
            var results = new List<ScheduleItem>();
            foreach (var item in items)
            {
                var request = service.Events.List(item.Id);
                request.TimeMin = Utility.GetFirstDayByCurrentCalendar();
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = request.Execute();
                results.Add(new ScheduleItem() { Summury = events.Summary, Items = events.Items });
            }
            return results;
        }

        private UserCredential GetUserCredential()
        {
            using (var stream = new FileStream(@"..\credentials.json", FileMode.Open, FileAccess.Read))
            {
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                 GoogleClientSecrets.Load(stream).Secrets,
                 _scopes,
                 "user",
                 CancellationToken.None,
                 new FileDataStore(_credPath, true)).Result;
            }
        }
    }
}

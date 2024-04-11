
using MVVM;
using ScheduleHelper;
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
using System.Linq;

//ID: 780541997867-kqm7396aqr4pl4qdnibjokd6jjppbovb.apps.googleusercontent.com
//PW: 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
namespace Main
{
    public class MainViewModel : NotifiableObject
    {
        public ObservableCollection<Day> Days { get; } = new ObservableCollection<Day>();
        public int Year { get; set; }
        public Months Month { get; set; }

        public GoogleCalendar Calendar { get; set; }
        public ICommand TESTCommand { get; }


        
        public MainViewModel()
        {
            Calendar = new GoogleCalendar();
            TESTCommand = new RelayCommand(OnExecuteTESTCommand);
            Initialize();
        }

        private void OnExecuteTESTCommand(object obj)
        {
            
        }

        private void Initialize()
        {
            InitializeDays();
            var items = Calendar.GetCalendarItems();

            foreach (var item in items)
            {
                UpdateSchedule(item);
            }

            //TODO: 스케줄별 색상 배경 설정 하기, color id? 있던것으로 기억함 api 재확인 ㄱ
        }

        private void InitializeDays()
        {
            var now = DateTime.Now;
            var first = now.MonthFirstDay();
            var last = now.MonthLastDay();

            var day = first.AddDays(-(int)first.DayOfWeek);

            while (true)
            {
                var isCurrentMonth = now.Month == day.Month;

                if (isCurrentMonth && Month == Months.None)
                {
                    Month = Utility.ConvertMonth(day.Month);
                    Year = day.Year;
                }

                Days.Add(new Day()
                {
                    DateTime = day,
                    IsCurrentMonth = isCurrentMonth,
                    //IsHoliday = day.DayOfWeek == DayOfWeek.Sunday,

                });
                day = day.AddDays(1);
                if (day > last && day.DayOfWeek == DayOfWeek.Sunday)
                {
                    break;
                }
            }
        }

        private void UpdateSchedule(ScheduleItem item)
        {
            foreach (var schedule in item.Items)
            {
            
                var date = schedule.Start.Date;
                var day = Days.Where(x => x.Date == date).FirstOrDefault();

                if(day is null)
                {
                    return;
                }
                day.Schedules.Add(new Schedule() {Title = schedule.Summary, Description = schedule.Description });

            }
        }
    }
}
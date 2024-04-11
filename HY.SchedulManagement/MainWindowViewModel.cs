using System;
using Microsoft.Xaml.Behaviors;
using System.Collections.ObjectModel;

namespace HY.SchedulManagement
{
    public class MainWindowViewModel : NotifiableObject
    {
        private readonly int _maxCount = 42; // 총 42칸

        public RelayCommand GenerateCommand { get; set; }

        public RelayCommand SelectCommand { get; set; }
        public RelayCommand ChangeColorcommand { get; set; }
        public ObservableCollection<Day> Days
        {
            get => Get<ObservableCollection<Day>>();
            set => Set(value);
        }

        public bool IsGenerate { get => Get<bool>(); set => Set(value); }
        public DateTime ToDate { get => Get<DateTime>(); set => Set(value); }
        public DateTime FromDate { get => Get<DateTime>(); set => Set(value); }
        public bool IsToChecked { get => Get<bool>(); set => Set(value); }
        public bool IsFromChecked { get => Get<bool>(); set => Set(value); }
        public string Description { get => Get<string>(); set => Set(value); }
        public MainWindowViewModel()
        {
            GenerateCommand = new RelayCommand(OnExecuteButtonCommand);
            SelectCommand = new RelayCommand(OnExecuteSelectCommand);
            ChangeColorcommand = new RelayCommand(OnExecuteChangeColorCommand);
        }

        private void OnExecuteChangeColorCommand(object obj)
        {
            // popup 팔레트
            
        }

        private void OnExecuteSelectCommand(object arg)
        {
            if (IsToChecked)
            {
                ToDate = (arg as Day).Date;
            }
            if (IsFromChecked)
            {
                FromDate = (arg as Day).Date;
            }
        }

        private void OnExecuteButtonCommand(object arg)
        {


            ToDate = (arg as Day).Date;
            FromDate = (arg as Day).Date;
        }

        internal void Initialize()
        {
            Days = new ObservableCollection<Day>();
            UpdateDays();
        }

        private void UpdateDays()
        {
            var now = DateTime.Now;
            
            var first = GetFirstDayOfMonth(now);
            

            // 42칸 기준 앞으로 몇개 뒤로 몇개 나와야하는지 계산 필요

            //var need = _maxCount - count; // 총 추가 필요 일 수
            // 요일 일~토 0~6
            var previous = (int)first.DayOfWeek;
            //var after = (int)last.DayOfWeek;

            var preDate = first.AddDays(-previous);
            
            for(int i = 0; i < _maxCount; i++)
            {
                var date = preDate.AddDays(i);

                var day = new Day()
                {
                    Date = date,
                    Week = date.DayOfWeek,
                    IsContain = GetIsContain(date),
                    IsNow = GetIsNow(date.Month, date.Day)
                };
                Days.Add(day);
            }
        }

        private bool GetIsNow(int month, int day)
        {
            return DateTime.Now.Month == month && DateTime.Now.Day == day;
        }

        private bool GetIsContain(DateTime date)
        {
            return DateTime.Now.Month == date.Month;
        }

        private DateTime GetFirstDayOfMonth(DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1);
        }
        private DateTime GetLastDayOfMonth(DateTime now)
        {
            return GetFirstDayOfMonth(now).AddMonths(1).AddDays(-1);
        }
        
    }
}

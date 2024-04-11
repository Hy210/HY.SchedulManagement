using System;


namespace HY.SchedulManagement
{
    public class Day : NotifiableObject
    {
        public DateTime Date { get => Get<DateTime>(); set => Set(value); }
        public DayOfWeek Week { get => Get<DayOfWeek>(); set => Set(value); } //요일
        public bool IsNow { get => Get<bool>(); set => Set(value); } // 오늘?
        public bool IsContain { get => Get<bool>(); set => Set(value); } // 이번달?
    }
}

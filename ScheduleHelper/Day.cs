using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScheduleHelper
{
    public class Day
    {
        public DateTime DateTime { get; set; }
        public string Date { get => DateTime.ToString("yyyy-MM-dd"); }
        
        public bool IsCurrentMonth { get; set; }

        public bool IsHoliday { get => DateTime.DayOfWeek == DayOfWeek.Sunday || Schedules.Any(x=>x.Description=="공휴일"); }

        public string DefaultInfo { get; set; }
        public ObservableCollection<Schedule> Schedules { get; } = new ObservableCollection<Schedule>();
    }

    public class Schedule
    {
        public string Title { get; set; }
        public string Description { get; set; }

    }
}

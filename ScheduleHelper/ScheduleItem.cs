using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class ScheduleItem
    {
        public string Summury { get; set; }
        public IList<Event> Items { get; internal set; }
    }
}

using System;
using System.IO;
using System.Net;
using System.Xml;


namespace ScheduleHelper
{
    public static class Utility
    {
        public static DateTime MonthFirstDay(this DateTime date) => date.AddDays(1 - date.Day).Date;
        public static DateTime MonthLastDay(this DateTime date) => date.MonthFirstDay().AddMonths(1).AddDays(-1);

        public static void GetHolidays()
        {
            /*
             * http://
             * apis.data.go.kr/B090041/openapi/service/SpcdeInfoService
             * /getRestDeInfo?
             * solYear=2019&
             * solMonth=03&
             * ServiceKey=서비스키
             */

            var key = "9mFlkcNuA80dYelDsGw2bjsI8vH0LvBmJRYiskhAke5%2BWyna%2BoKKX%2BbTRNRjeDjY%2BaDB%2BRSGvobKC8SzoazBcw%3D%3D";
            var year = "2024";
            var month = $"{5:00}";
            var url = $"http://apis.data.go.kr/B090041/openapi/service/SpcdeInfoService/getRestDeInfo?solYear={year}&solMonth={month}&ServiceKey={key}";


            var client = new WebClient();
            using (var data = client.OpenRead(url))
            {
                using (var reader = new StreamReader(data))
                {
                    var content = reader.ReadToEnd();
                    var xml = new XmlDocument();
                    xml.LoadXml(content);
                    var list = xml.GetElementsByTagName("item");

                    foreach (var item in list)
                    {
                        var node = (XmlNode)item;
                        var name = node["dateName"];
                        var isHoliday = node["isHoliday"].InnerText == "Y";
                        var date = ParseDateTime(node["locdate"].InnerText);

                    }
                }
            }
        }

        public static DateTime ParseDateTime(string date)
        {
            if(DateTime.TryParse(date, out var result))
            {
                return result;
            }
            try
            {
                return new DateTime(int.Parse(date.Substring(0, 4)),
                                    int.Parse(date.Substring(4, 2)),
                                    int.Parse(date.Substring(6, 2)));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static bool ConvertBoolean(string isHoliday)
        {
            return isHoliday == "Y";
        }

        public static DateTime GetFirstDayByCurrentCalendar()
        {
            var first = DateTime.Now.MonthFirstDay();
            return first.AddDays(-(int)first.DayOfWeek);
        }

        public static Months ConvertMonth(int number) => (Months)number;
    }
}

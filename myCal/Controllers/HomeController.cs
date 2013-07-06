using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace myCal.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        private static DateTime FromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private static long ToUnixTimestamp(DateTime date)
        {
            TimeSpan ts = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return (long) Math.Truncate(ts.TotalSeconds);
        }

        // using ScheduleWidget.Enums; 
        // using ScheduleWidget.ScheduledEvents; 
        private static Schedule BuildSchedule()
        {
            var aEvent = new Event
                {
                    FrequencyTypeOptions = FrequencyTypeEnum.Weekly,
                    DaysOfWeekOptions = DayOfWeekEnum.Tue | DayOfWeekEnum.Thu
                };
            return new Schedule(aEvent);
        }


        public JsonResult GetEvents(double start, double end)
        {
            var list = new List<object>();
            var startHour = new TimeSpan(DateTime.Now.Hour, 0, 0);
            var endHour = new TimeSpan(DateTime.Now.Hour + 1, 0, 0);
            var range = new DateRange {StartDateTime = FromUnixTimestamp(start), EndDateTime = FromUnixTimestamp(end)};
            Schedule schedule = BuildSchedule();
            foreach (DateTime date in schedule.Occurrences(range))
            {
                list.Add(new
                        {
                            id = 1,
                            title = "Event 1",
                            start = ToUnixTimestamp(date + startHour),
                            end = ToUnixTimestamp(date + endHour),
                            allDay = false
                        });
            }
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
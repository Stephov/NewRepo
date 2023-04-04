using MaratukAdmin.Entities;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MaratukAdmin.Utils
{
    public  static class HandleWeekDays
    {
        public static string GetWeekDayNames(string dayOfWeek)
        {
            string[] weekDayIndexArray = dayOfWeek.Split(',');
            string weekDayNames = "";

            foreach (string index in weekDayIndexArray)
            {
                int weekDayIndex = int.Parse(index);

                switch (weekDayIndex)
                {
                    case 0:
                        weekDayNames += "Monday, ";
                        break;
                    case 1:
                        weekDayNames += "Tuesday, ";
                        break;
                    case 2:
                        weekDayNames += "Wednesday, ";
                        break;
                    case 3:
                        weekDayNames += "Thursday, ";
                        break;
                    case 4:
                        weekDayNames += "Friday, ";
                        break;
                    case 5:
                        weekDayNames += "Saturday, ";
                        break;
                    case 6:
                        weekDayNames += "Sunday, ";
                        break;
                    default:
                        weekDayNames += "";
                        break;
                }
            }
            weekDayNames = weekDayNames.TrimEnd(' ', ',');

            return weekDayNames;

        }
    }
}

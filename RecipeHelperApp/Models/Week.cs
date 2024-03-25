using RecipeHelperApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHelperApp.Models
{
    public class Week
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? WeekName { get; set; }
        public string? Description { get; set; }
        public ICollection<Day> Days { get; set; }

        public Week()
        {
            Days = new List<Day>();
            // InitializeDays();
        }
        public Week(string weekName)
        {
            Days = new List<Day>();
            WeekName = weekName;
            InitializeDays();
        }
        public void InitializeDays()
        {
            Console.WriteLine("Called initialize");
            string[] weekdaysList = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            foreach (var weekDay in weekdaysList)
            {
                Day day = new Day
                {
                    WeekDay = weekDay
                };

                Days.Add(day);
            }
        }
    }
}
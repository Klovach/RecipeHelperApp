using RecipeHelperApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHelperApp.Models
{
    public class Week
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string? WeekName {  get; set; }
        public string? Description { get; set; }
        public virtual List<Day> Days { get; set; }

        public Week()
        {
            Days = new List<Day>();
            // InitializeDays();
        }
        public Week(string weekName)
        {
            WeekName = weekName;
            InitializeDays();
        }
        public void InitializeDays()
        {
            Days = new List<Day>();
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
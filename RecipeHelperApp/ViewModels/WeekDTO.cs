using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeHelperApp.Data;

namespace RecipeHelperApp.ViewModels
{
    public class WeekDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string WeekName { get; set; }
        public string Description { get; set; }
    }
}

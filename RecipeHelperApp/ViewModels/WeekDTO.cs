using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeHelperApp.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.ViewModels
{
    public class WeekDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [DisplayName("Name")]
        public string WeekName { get; set; }
        public string Description { get; set; }
    }
}

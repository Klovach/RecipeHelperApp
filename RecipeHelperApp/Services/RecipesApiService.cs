using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using RecipeHelperApp.Responses;
using RecipeHelperApp.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http.Description;


// This is an API Client class which retrieves recipe data. 
// 
namespace RecipeHelperApp.Controllers
{
    [ApiController]
    [Route("test/recipes")]
    public class RecipesApiController : ControllerBase
    {
        static HttpClient client = new HttpClient();

        static readonly string spoonacularApiBaseUrl = "https://api.spoonacular.com/recipes/";
        static readonly string spoonacularApiKey = "21edf5f318994458b141e316bf7a5900";

        [HttpGet]
        public async Task<ActionResult<GetRecipeByIdResult200>> Index()
        {
          //  NutritionFormModel nutritionForm = new NutritionFormModel();

            // Call GetRecipeByIdAsync method
        return  await GetRecipeByIdAsync(1);

            // Create an example NutritionFormModel & call ComplexSearchAsync method with example parameters


            // Call GetRecipesByIdAsync method with example list of IDs
       //  return await GetRecipesByIdAsync(new List<int> { 1,2,3 });
        }


  
        [HttpGet]
        [Route("complexSearch/{number}")]
        public async Task<ActionResult<ComplexRecipeResult200>> ComplexSearchAsync(int number)
        {

            number = 2; 
            // Adjust the query parameters and URL according to the Spoonacular API documentation & nutritionForm
            string parameters = $"?addRecipeInformation=True&maxFat=25&number=2";
     /*       if (nutritionForm.IncludeIngredients)
            {
                parameters += $"&includeIngredients={string.Join(",", nutritionForm.IncludedIngredients)}";
            }

            if (nutritionForm.ExcludeIngredients)
            {
                parameters += $"&excludeIngredients={string.Join(",", nutritionForm.ExcludedIngredients)}";
            } 

            if (nutritionForm.Nutrients != null && nutritionForm.Nutrients.Count > 0)
            {
                foreach (var nutrient in nutritionForm.Nutrients)
                {
                    parameters += $"&{nutrient.Name}={nutrient.Amount}";
                }
                parameters = parameters.TrimEnd(',');
            } 

            parameters += "&number=" + number;*/
            var url = $"{spoonacularApiBaseUrl}complexSearch";
            var recipes = await GetComplexRecipeAsync(url, parameters);
            if (recipes == null)
            {
                return NotFound(); // Return 404 if the recipe is not found
            }
            return Ok(recipes); // Return 200 OK with the recipe
        }

        [HttpGet]
        [Route("GetRecipeById/{id}")]
        public async Task<ActionResult<GetRecipeByIdResult200>> GetRecipeByIdAsync(int id)
        {
            string parameters = "?instructionsRequired=True&includeNutrition=True";

            // Adjust the query parameters and URL according to the Spoonacular API documentation
            var url = $"{spoonacularApiBaseUrl}{id}/information";

            GetRecipeByIdResult200 recipe = await GetRecipeAsync(url, parameters);
            if (recipe == null)
            {
                return NotFound(); // Return 404 if the recipe is not found
            }

            Console.WriteLine("Success:" + recipe.ToString());
            return recipe; // Return 200 OK with the recipe
        }

        [HttpGet]
        [Route("GetRecipesById/{ids}")]
        public async Task<ActionResult<GetRecipeByIdResult200>> GetRecipesByIdAsync(List<int> ids)
        {
            // Adjust the query parameters and URL according to the Spoonacular API documentation
            if (ids == null || ids.Count == 0)
            {
                // Handle the case where ids is empty
                return null;
            }


            string parameters = "?instructionsRequired=True&includeNutrition=True&ids=";
            // Create a comma-separated string of ids
          
            var idList = string.Join(",", ids);
            parameters += idList; 
            var url = $"{spoonacularApiBaseUrl}informationBulk";
            var recipes = await GetRecipesAsync(url, parameters);
            if (recipes == null)
            {
                return NotFound(); // Return 404 if the recipe is not found
            }

            Console.WriteLine("Success:" + recipes.ToString());
            return Ok(recipes); // Return 200 OK with the recipe
        }

        static async Task<List<GetRecipeByIdResult200>> GetRecipesAsync(string url, string parameters)
        {
            List<GetRecipeByIdResult200> recipes = null;
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", spoonacularApiKey);

                url = (url + parameters);

                Console.WriteLine(url);

                Console.WriteLine("Request Headers:");

                foreach (var header in client.DefaultRequestHeaders)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                Console.WriteLine($"Request URL: {url}");
                
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                 //   List<RecipeResultModel> recipe = await response.Content.ReadAsAsync<List<RecipeResultModel>>();
                    Console.WriteLine("test");
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Raw JSON Response:");
                    Console.WriteLine(jsonResponse);

                    // Deserialize the list of recipes
                    recipes = JsonConvert.DeserializeObject<List<GetRecipeByIdResult200>>(jsonResponse);

                }
                else
                {
                    // Log unsuccessful response
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return recipes;
        }


        static async Task<ComplexRecipeResult200> GetComplexRecipeAsync(string url, string parameters)
        {
            ComplexRecipeResult200 recipes = null;
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", spoonacularApiKey);

                url = (url + parameters);

                Console.WriteLine(url);

                Console.WriteLine("Request Headers:");

                foreach (var header in client.DefaultRequestHeaders)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                Console.WriteLine($"Request URL: {url}");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    //   List<RecipeResultModel> recipe = await response.Content.ReadAsAsync<List<RecipeResultModel>>();
                    Console.WriteLine("test");
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Raw JSON Response:");
                    Console.WriteLine(jsonResponse);

                    // Deserialize the list of recipes
                    recipes = JsonConvert.DeserializeObject<ComplexRecipeResult200> (jsonResponse);

                }
                else
                {
                    // Log unsuccessful response
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return recipes;
        }

        static async Task<List<ComplexRecipeResult200>> GetComplexRecipesAsync(string url, string parameters)
        {
            List<ComplexRecipeResult200> recipes = null;
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", spoonacularApiKey);

                url = (url + parameters);

                Console.WriteLine(url);

                Console.WriteLine("Request Headers:");

                foreach (var header in client.DefaultRequestHeaders)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                Console.WriteLine($"Request URL: {url}");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    //   List<RecipeResultModel> recipe = await response.Content.ReadAsAsync<List<RecipeResultModel>>();
                    Console.WriteLine("test");
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Raw JSON Response:");
                    Console.WriteLine(jsonResponse);

                    // Deserialize the list of recipes
                    recipes = JsonConvert.DeserializeObject<List<ComplexRecipeResult200>>(jsonResponse);

                }
                else
                {
                    // Log unsuccessful response
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return recipes;
        }



        static async Task<GetRecipeByIdResult200> GetRecipeAsync(string url, string parameters)
        {
            GetRecipeByIdResult200 recipe = null;
      
            // Include the API key in the request headers
            //   client.BaseAddress = new Uri("http://localhost:44370/");
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", spoonacularApiKey);

                url = (url + parameters);

                Console.WriteLine(url); 
               

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    
                    recipe = await response.Content.ReadAsAsync<GetRecipeByIdResult200>();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Raw JSON Response:");
                    Console.WriteLine(response);
                    recipe = JsonConvert.DeserializeObject<GetRecipeByIdResult200>(jsonResponse);
                }
                else
                {
                    // Log unsuccessful response
                    Console.WriteLine($"Error: {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception: {ex.Message}");
            }



            return recipe;
        }
    }
    }


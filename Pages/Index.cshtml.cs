using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using CosmosOdyssey.Models;
using Newtonsoft.Json;

namespace CosmosOdyssey.Pages
{
    public class IndexModel : PageModel
    {
        private HttpClient _httpClient;

        [BindProperty] public static TravelRouteModel? TravelRoute { get; set; } = null;
        
        public async Task<IActionResult> OnGet() { //Initial OnGet load
            Console.WriteLine("OnGet called!");
            return Page();
        }
        
        public async Task<IActionResult> TravelSubmitHandler() {
            Console.WriteLine("TravelSubmitHandler called!");
            // Initialize with default values
            _httpClient = new HttpClient();
            TravelRoute = new TravelRouteModel();

            string from = Request.Query["from"];
            string to = Request.Query["to"];
            
            // Make an API request to get route information based on the selected planets
            string apiUrl = $"https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices/?from={from}&to={to}"; // Replace with the actual API URL
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string apiResponseJson = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response manually
                TravelRoute = JsonConvert.DeserializeObject<TravelRouteModel>(apiResponseJson);

                return Page();
            }
            else
            {
                // Handle API request failure
                Console.WriteLine("API error?");
                return Page(); // You can customize this to display an error message
            }
        }

        public IActionResult FindRouteModel()
        {
            Console.WriteLine("FindRouteModel called!");
            _httpClient = new HttpClient();
            TravelRoute = new TravelRouteModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("OnPostAsync called!");
            var from = Request.Form["from"];
            var to = Request.Form["to"];

            // Make an API request to get route information based on the selected planets
            string apiUrl = $"https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices/?from={from}&to={to}"; // Replace with the actual API URL
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string apiResponseJson = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response manually
                TravelRoute = JsonConvert.DeserializeObject<TravelRouteModel>(apiResponseJson);

                return Page();
            }
            else
            {
                // Handle API request failure
                return Page(); // You can customize this to display an error message
            }
        }
    }
}
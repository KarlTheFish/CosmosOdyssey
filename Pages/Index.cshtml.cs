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

        [BindProperty]
        public TravelRouteModel TravelRoute { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string fromSelection { get; set; }
        [BindProperty(SupportsGet = true)]
        public string toSelection { get; set; }
        
        public async Task<IActionResult> OnGet() { //Initial OnGet load
            fromSelection = Request.Query["from"];
            toSelection = Request.Query["to"];
            if (fromSelection != toSelection) {
                await TravelSubmitted(fromSelection, toSelection);
            }
            return Page();
        }

        public async Task<IActionResult> TravelSubmitted(string from, string to) {
            await TravelSubmitHandler(from, to);
            return RedirectToPage("index", new { from = from, to = to });
        }
        
        public async Task<IActionResult> TravelSubmitHandler(string from, string to) {
            // Initialize with default values
            _httpClient = new HttpClient();
            TravelRoute = new TravelRouteModel();
            
            // Make an API request to get route information based on the selected planets
            string apiUrl = $"https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices/?from={from}&to={to}"; // Replace with the actual API URL
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            Console.WriteLine(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("API call success");
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
    }
}
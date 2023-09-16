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
        public TravelRouteModel? chosenRoute = null;
        
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
            
            // Make an API request to get route information based on the selected planets
            string apiUrl = $"https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices/";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("API call success");
                string apiResponseJson = await response.Content.ReadAsStringAsync();
                
                //Deserialize the JSON response into local travelroutelist model
                Pricelist priceList = JsonConvert.DeserializeObject<Pricelist>(apiResponseJson);
                
                //Find the matching route
                foreach (leg leg in priceList.legs)
                {
                    routeInfo routeInfo = leg.routeInfo;
                    if (routeInfo.from.name == fromSelection && routeInfo.to.name == toSelection) {
                        chosenRoute.From = fromSelection;
                        chosenRoute.To = toSelection;
                        chosenRoute.Distance = routeInfo.distance;
                        chosenRoute.Providers = leg.providers;
                        break;
                    }
                }
                
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
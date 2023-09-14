using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using CosmosOdyssey.Models;
using Newtonsoft.Json;

public class index : PageModel
{
    private HttpClient _httpClient;

    [BindProperty]
    public TravelRouteModel? TravelRoute { get; set; }
    
    public void OnGet()
    {
        // Initialize with default values
        TravelRoute = new TravelRouteModel();
    }

    public void FindRouteModel()
    {
        _httpClient = new HttpClient();
        TravelRoute = new TravelRouteModel();
    }

    public async Task<IActionResult> OnPostAsync()
    {
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
using CosmosOdyssey.Models;
using CosmosOdyssey.Pages;
using CosmosOdyssey.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CosmosOdyssey.Data;

//Sends API request, gets the data and binds it to a model

public class RoutesAPIrequest {
    private HttpClient httpClient;
    public Pricelist pricelist = new Pricelist();
    private TravelRouteDataService travelRouteDataService;
    private string apiURL = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices/";
    public TravelRouteModel ChosenRoute = new TravelRouteModel();
    List<List<TravelRouteModel>> AllFullRoutes = new List<List<TravelRouteModel>>();
    private string fromString;
    private string toString;
    private readonly IndexModel indexModel;

    public RoutesAPIrequest(TravelRouteDataService routeDataService, IndexModel indexModel) {
        travelRouteDataService = routeDataService;
        this.indexModel = indexModel;
    }
    
    public async Task<IActionResult> MakeRequest(string from, string to) {
        httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(apiURL);
        if (response.IsSuccessStatusCode) {
            //Deserialize Json response into priceList model
            string apiResponseJson = await response.Content.ReadAsStringAsync();
            pricelist = new Pricelist();
            pricelist = JsonConvert.DeserializeObject<Pricelist>(apiResponseJson);
            //TODO: Check for validUntil, store last 15 lists
            
            //Find a route with the current chosen values from the current pricelist
            travelRouteDataService.routeOptions = CalculateRoute.FindAvailableRoute(pricelist, from, to);
            
        }
        else
        {
            Console.WriteLine("API error");
            //TODO: Add some kind of visual error message
        }
        return indexModel.RedirectToIndex();
    }
}
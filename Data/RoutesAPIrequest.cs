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
            //Get a direct route with current chosen values
            DirectRouteModel(pricelist, from, to);
            //If DirectRouteModel returns a model with empty values, there is no direct route avalaible.
            if (ChosenRoute.Distance < 10) {
                travelRouteDataService.longRouteOptions = CalculateRoute.FindAvalaibleRoute(pricelist, from, to);
            }
        }
        else
        {
            Console.WriteLine("API error");
            //TODO: Add some kind of visual error message
        }
        return indexModel.RedirectToIndex();
    }

    //Binds one of the routes from API request data to a model
    public TravelRouteModel DirectRouteModel(Pricelist PriceList, string from, string to) {
        ChosenRoute = new TravelRouteModel();
        foreach (Leg leg in pricelist.legs) {
            ChosenRoute.From = from;
            ChosenRoute.To = to;
            if (leg.routeInfo.from.name == from && leg.routeInfo.to.name == to) {
                ChosenRoute.Distance = leg.routeInfo.distance;
                ChosenRoute.Providers = leg.providers;
                break;
            }
        }
        //Once the binding is done, give the value to the TravelRouteDataService service
        travelRouteDataService.chosenRoute = ChosenRoute;
        return ChosenRoute;
    }
}
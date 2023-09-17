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
            Console.WriteLine("API call success");
            //Deserialize Json response into priceList model
            string apiResponseJson = await response.Content.ReadAsStringAsync();
            pricelist = new Pricelist();
            pricelist = JsonConvert.DeserializeObject<Pricelist>(apiResponseJson);
            //TODO: Check for validUntil, store last 15 lists
            Console.WriteLine((pricelist != null) + " from " + (from) + " to " +(to) );
            BindRouteDataToModel(pricelist, from, to);
        }
        else
        {
            Console.WriteLine("API error");
            //TODO: Add some kind of visual error message
        }
        return indexModel.RedirectToIndex();
    }

    //Binds one of the routes from API request data to a model
    public TravelRouteModel BindRouteDataToModel(Pricelist PriceList, string from, string to) {
        ChosenRoute = new TravelRouteModel();
        foreach (Leg leg in pricelist.legs) {
            if (leg.routeInfo.from.name == from && leg.routeInfo.to.name == to) {
                ChosenRoute.From = from;
                ChosenRoute.To = to;
                ChosenRoute.Distance = leg.routeInfo.distance;
                ChosenRoute.Providers = leg.providers;
                break;
            }
        }
        //Once the binding is done, give the value to the TravelRouteDataService service
        //TravelRouteDataService travelRouteDataService = new TravelRouteDataService();
        travelRouteDataService.chosenRoute = ChosenRoute;
        return ChosenRoute;
    }
}
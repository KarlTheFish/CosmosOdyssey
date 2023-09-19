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
    public Pricelist currentPricelist;
    private TravelRouteDataService travelRouteDataService;
    private string apiURL = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices/";
    private readonly IndexModel indexModel;
    

    public RoutesAPIrequest(TravelRouteDataService routeDataService, IndexModel indexModel) {
        travelRouteDataService = routeDataService;
        this.indexModel = indexModel;
    }

    public async Task<IActionResult> GetPriceList(string from, string to) {
        if (travelRouteDataService.Last15Pricelists.Count == 0) { //If the count of current pricelists is 0, get a new one
            await MakeAPIrequest();
        }
        currentPricelist = travelRouteDataService.Last15Pricelists[0];
        if (currentPricelist.validUntil < DateTime.Now) { //If the current time is newer than validUntil datetime
            Console.WriteLine("Time to change lol");
            await MakeAPIrequest();
        }

        travelRouteDataService.routeOptions = CalculateRoute.FindAvailableRoute(currentPricelist, from, to);
        
        return indexModel.RedirectToIndex();
    }

    public async Task MakeAPIrequest() {
        httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(apiURL);
        if (response.IsSuccessStatusCode) {
            //Deserialize Json response into priceList model
            string apiResponseJson = await response.Content.ReadAsStringAsync();
            currentPricelist = new Pricelist();
            currentPricelist = JsonConvert.DeserializeObject<Pricelist>(apiResponseJson);
            //Add the received pricelist into the list and remove the last one if there are more than 15 items
            travelRouteDataService.Last15Pricelists.Insert(0, currentPricelist);
            if (travelRouteDataService.Last15Pricelists.Count > 15) {
                travelRouteDataService.Last15Pricelists.RemoveAt(15); //Remove the 16th item at index 15
            }
        }
        else
        {
            Console.WriteLine("API error");
            //TODO: Add some kind of visual error message
        }
    } 
}
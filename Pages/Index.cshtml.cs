using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmosOdyssey.Data;
using CosmosOdyssey.Models;
using CosmosOdyssey.Services;
using HttpClient = System.Net.Http.HttpClient;

namespace CosmosOdyssey.Pages
{
    public class IndexModel : PageModel
    {
        public List<List<TravelRouteModel>>? RouteOptions = null;
        
        [BindProperty(SupportsGet = true)]
        public string fromSelection { get; set; }
        [BindProperty(SupportsGet = true)]
        public string toSelection { get; set; }

        public string visibleTable;
        private bool requestMade;

        private TravelRouteDataService travelRouteDataService;
        public String[] Planets = {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};

        public IndexModel(TravelRouteDataService routeDataService) {
            travelRouteDataService = routeDataService;
        }
        
        public IActionResult OnGet() { //OnGet load; initiated every time the page is loaded
            //Get data if the request has already been made
            if (requestMade == true) {
                RouteOptions = travelRouteDataService.routeOptions;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost() {
            RoutesAPIrequest APIrequest = new RoutesAPIrequest(travelRouteDataService, this);
            if (fromSelection != "" && toSelection != "") {
                await APIrequest.MakeRequest(fromSelection, toSelection);
                requestMade = true;
                RouteOptions = travelRouteDataService.routeOptions;
                foreach (var option in RouteOptions) {
                    FullRouteToText(option);
                }
            }
            return Page();
        }

        public IActionResult RedirectToIndex() {
            return RedirectToPage("/index");
        }

        public void SetTableVisibility(string TableID) {
            visibleTable = TableID;
        }

        public string FullRouteToText(List<TravelRouteModel> fullRoute) {
            string FullRouteText = "";
            foreach (var route in fullRoute) {
                FullRouteText += route.From + " - ";
            }
            FullRouteText += fullRoute[^1].To;
            return FullRouteText;
        }
    }
}
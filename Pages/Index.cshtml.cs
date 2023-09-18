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
        public TravelRouteModel? chosenRoute = null;
        public List<List<TravelRouteModel>>? longRouteOptions = null;
        
        [BindProperty(SupportsGet = true)]
        public string fromSelection { get; set; }
        [BindProperty(SupportsGet = true)]
        public string toSelection { get; set; }

        private bool requestMade;

        private TravelRouteDataService travelRouteDataService;
        public String[] Planets = {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};

        public IndexModel(TravelRouteDataService routeDataService) {
            travelRouteDataService = routeDataService;
        }
        
        public IActionResult OnGet() { //OnGet load; initiated every time the page is loaded
            //Get data if the request has already been made
            if (requestMade == true) {
                chosenRoute = travelRouteDataService.chosenRoute;
                longRouteOptions = travelRouteDataService.longRouteOptions;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost() {
            RoutesAPIrequest APIrequest = new RoutesAPIrequest(travelRouteDataService, this);
            if (fromSelection != "" && toSelection != "") {
                await APIrequest.MakeRequest(fromSelection, toSelection);
                requestMade = true;
                chosenRoute = travelRouteDataService.chosenRoute;
                longRouteOptions = travelRouteDataService.longRouteOptions;
            }
            return Page();
        }

        public IActionResult RedirectToIndex() {
            return RedirectToPage("/index");
        }
    }
}
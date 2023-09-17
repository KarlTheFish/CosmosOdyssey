using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using CosmosOdyssey.Data;
using CosmosOdyssey.Models;
using CosmosOdyssey.Services;
using Newtonsoft.Json;
using HttpClient = System.Net.Http.HttpClient;

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

        private bool requestMade = false;

        private TravelRouteDataService travelRouteDataService;
        private readonly RoutesAPIrequest APIrequest;

        public IndexModel(TravelRouteDataService routeDataService) {
            travelRouteDataService = routeDataService;
        }
        
        public IActionResult OnGet() { //Initial OnGet load
            //Get data if the request has already been made
            if (requestMade == true) {
                chosenRoute = travelRouteDataService.chosenRoute;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost() {
            RoutesAPIrequest APIrequest = new RoutesAPIrequest(travelRouteDataService, this);
            if (fromSelection != "" && toSelection != "") {
                await APIrequest.MakeRequest(fromSelection, toSelection);
                requestMade = true;
                chosenRoute = travelRouteDataService.chosenRoute;
            }
            return Page();
        }

        public IActionResult RedirectToIndex() {
            return RedirectToPage("/index");
        }
    }
}
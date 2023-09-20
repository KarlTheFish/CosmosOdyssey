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
        public List<List<TravelRouteModel>>? RouteOptions { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string fromSelection { get; set; }
        [BindProperty(SupportsGet = true)]
        public string toSelection { get; set; }
        [BindProperty(SupportsGet = true)]
        public string companyFilter { get; set; }
        [BindProperty]
        public List<string> SelectedProviders { get; set; }
        private bool requestMade;
        public HashSet<String> availableCompanies;

        private TravelRouteDataService travelRouteDataService;
        public String[] Planets = {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};

        public IndexModel(TravelRouteDataService routeDataService) {
            travelRouteDataService = routeDataService;
        }
        
        public IActionResult OnGet() { //OnGet load; loads first time the page is loaded
            SelectedProviders = new List<string>();
            if (requestMade == true) {
                RouteOptions = travelRouteDataService.routeOptions;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost() {
                RoutesAPIrequest APIrequest = new RoutesAPIrequest(travelRouteDataService, this); //Filtering still doesn't work
                Console.WriteLine("From " + fromSelection + " to " + toSelection);
                await APIrequest.GetPriceList(fromSelection, toSelection, companyFilter);
                requestMade = true;
                RouteOptions = travelRouteDataService.routeOptions;
                availableCompanies = new HashSet<string>();
                foreach (var option in RouteOptions) {
                    FullRouteToText(option);
                    foreach (var route in option) {
                        foreach (var provider in route.Providers) {
                            availableCompanies.Add(provider.company.name);
                        }
                    }
                }
            return Page();
        }

        public IActionResult OnPostBookFlight() {
            Console.WriteLine("OnPostBookFlight from index called");
            return RedirectToPage("/BookFlight");
        }

        public IActionResult RedirectToIndex() {
            return RedirectToPage("/index");
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmosOdyssey.Pages;

public class BookFlightModel : PageModel {
    
    [BindProperty(SupportsGet = true)]
    public List<string> SelectedProviders { get; set; }

    //[BindProperty]
    //public string Routes { get; set; }
    

    public void OnGet() {
        Console.WriteLine("onGet from BookFlight page called");
    }

    public IActionResult OnPost() {
        //SelectedProviders = new List<string>();
        Console.WriteLine(SelectedProviders.Count);
        Console.WriteLine("OnPost from BookFlight page called");
        return Page();
    }
}
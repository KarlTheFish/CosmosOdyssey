namespace CosmosOdyssey.Models;

//Made for my own convenience and slightly improved human readability
public class TravelRouteModel {
    public string From { get; set; }
    public string To { get; set; }
    public int Distance { get; set; }
    public List<Provider> Providers { get; set; }
}

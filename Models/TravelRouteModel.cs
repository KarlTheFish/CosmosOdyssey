namespace CosmosOdyssey.Models;

public class TravelRouteModel {
    public int ID { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int Distance { get; set; }
    public List<Provider> Providers { get; set; }
}
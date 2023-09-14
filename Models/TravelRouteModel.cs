namespace CosmosOdyssey.Models;

public class TravelRouteModel {
    public static int ID { get; set; }
    public static string From { get; set; }
    public static string To { get; set; }
    public static int Distance { get; set; }
    public static List<Provider> Providers { get; set; }

    public TravelRouteModel() {
        ID = -1;
    }
}
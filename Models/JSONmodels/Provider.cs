namespace CosmosOdyssey.Models;

public struct Provider {
    public company company { get; set; }
    public float price { get; set; }
    public DateTime flightStart { get; set; }
    public DateTime flightEnd { get; set; }
}
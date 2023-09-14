namespace CosmosOdyssey.Models;

public struct Provider {
    public int ID { get; set; }
    public string CompanyName { get; set; }
    public float Price { get; set; }
    public TimeOnly FlightTime { get; set; }
}
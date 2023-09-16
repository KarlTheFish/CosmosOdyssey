namespace CosmosOdyssey.Models;

public class Pricelist {
    public string ID { get; }
    public DateTime validUntil { get; }
    public List<leg> legs { get; }
}

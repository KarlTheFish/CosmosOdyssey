namespace CosmosOdyssey.Models;

public class Pricelist {
    public string id { get; set; }
    public DateTime validUntil { get; set; }
    public List<Leg> legs { get; set; }
}

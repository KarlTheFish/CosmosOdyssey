using CosmosOdyssey.Models;

namespace CosmosOdyssey.Services;

public class TravelRouteDataService
{
    public List<Pricelist> Last15Pricelists = new List<Pricelist>();
    public List<List<TravelRouteModel>> routeOptions;
}
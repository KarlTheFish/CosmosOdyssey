using CosmosOdyssey.Models;

namespace CosmosOdyssey.Data;

public class CalculateRoute
{
    public static List<List<TravelRouteModel>> FindAvalaibleRoute(Pricelist pricelist, string from, string to, List<List<TravelRouteModel>> outputList)
    {
        FindRouteFromTo(from, to, FindFirstRoute(pricelist, from), pricelist, outputList);
        
        //Finally, return list of possible travel routes
        return outputList;
    }
    
    //Function that looks at all available flights from specific location
    public static List<TravelRouteModel> FindFirstRoute(Pricelist pricelist, string from) {
        List<TravelRouteModel> CandidateRoutes = new List<TravelRouteModel>();
        foreach (Leg leg in pricelist.legs) {
            TravelRouteModel candidateRoute = new TravelRouteModel {
                From = from,
                To = leg.routeInfo.to.name,
                Distance = leg.routeInfo.distance,
                Providers = leg.providers
            };
            CandidateRoutes.Add(candidateRoute);
        }
        return CandidateRoutes;
    }

    //Function for searching for routes from to specific locations
    public static void FindRouteFromTo(string from, string to, List<TravelRouteModel> candidateRoutes, Pricelist pricelist, List<List<TravelRouteModel>> output) {
        foreach (TravelRouteModel candidate in candidateRoutes) {
            //Find a route from all routes that starts with candidate destination, ends with user destination
            foreach (Leg leg in pricelist.legs) {
                if (leg.routeInfo.from.name == candidate.To && leg.routeInfo.to.name == to) {
                    TravelRouteModel foundRoute = new TravelRouteModel {
                        From = candidate.To,
                        To = to,
                        Distance = leg.routeInfo.distance,
                        Providers = leg.providers
                    };
                    //Because the route matches, we can put candidate(the first leg) and foundRoute(second leg) as full flight
                    List<TravelRouteModel> FullRoute = new List<TravelRouteModel> {
                        candidate, foundRoute
                    };
                    output.Add(FullRoute);
                }
            }
        }

        if (output.Count == 0) {
            foreach (TravelRouteModel candidate in candidateRoutes) {
                FindRouteFromTo(candidate.To, to, FindFirstRoute(pricelist, candidate.To), pricelist, output);
            }
        }
    }
}
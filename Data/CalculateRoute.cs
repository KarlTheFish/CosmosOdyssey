using CosmosOdyssey.Models;

namespace CosmosOdyssey.Data;

public class CalculateRoute
{
    public static List<List<TravelRouteModel>> FindAvalaibleRoute(Pricelist pricelist, string from, string to, List<List<TravelRouteModel>> outputList)
    {
        List<TravelRouteModel> FullRoute = new List<TravelRouteModel>();
        
        //Finally, return list of possible travel routes
        return outputList;
    }
    
    //Function that looks at all available flights from specific location
    public static List<TravelRouteModel> FindFirstRoute(Pricelist pricelist, string from, string to) {
        List<List<TravelRouteModel>> FullTravelRoutes = new List<List<TravelRouteModel>>();
        List<TravelRouteModel> CandidateRoutes = new List<TravelRouteModel>();
        //Does the "from" destination match? If yes, add to possible route matches
        foreach (Leg leg in pricelist.legs) {
            if (leg.routeInfo.from.name == from) {
                TravelRouteModel candidateRoute = new TravelRouteModel {
                    From = leg.routeInfo.from.name,
                    To = leg.routeInfo.to.name,
                    Distance = leg.routeInfo.distance,
                    Providers = leg.providers
                };
                //Add the new candidateRoute into a list of possibilities? idk 
            }
        }
        return CandidateRoutes;
    }
    //TODO FOR TOMORROW: Find first route. Is one of them with right destination? If yes, all good. If no, do it again
    //with the new destinations. Repeat all good? Until you find the one with the right destination.
    //How to store them?
    //Store all found routes. When you find the right one, delete everything else
    //How?
    //Store every route as a list of routes. With every additional search, just add to the existing routes. Then
    //once you find the right one, you can store it

    //Function for searching for routes from to specific locations
    public static void FindRouteFromTo(string from, string to, List<TravelRouteModel> candidateRoutes, Pricelist pricelist, List<TravelRouteModel> FullRoute, List<List<TravelRouteModel>> output) {
        foreach (TravelRouteModel candidate in candidateRoutes) {
            //Add the candidate into a list
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
                    FullRoute.Add(candidate);
                    FullRoute.Add(foundRoute);
                    output.Add(FullRoute);
                }
            }
        }

        if (output.Count == 0) {
            foreach (TravelRouteModel candidate in candidateRoutes) {
                FindRouteFromTo(candidate.To, to, FindFirstRoute(pricelist, candidate.To), pricelist, FullRoute, output);
            }
        }
    }
}
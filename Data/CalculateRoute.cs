using CosmosOdyssey.Models;

namespace CosmosOdyssey.Data;

public class CalculateRoute
{
    public static List<List<TravelRouteModel>> FindAvalaibleRoute(Pricelist pricelist, string from, string to) {
        List<List<TravelRouteModel>> AllFullRoutes = new List<List<TravelRouteModel>>();
        List<TravelRouteModel> CandidateRoutes = new List<TravelRouteModel>();
        //If direct flight is not found, get all options for flights from destination
        foreach (Leg leg in pricelist.legs) {
            //First find the routes with the right departure point
            if (leg.routeInfo.from.name == from) {
                TravelRouteModel candidateRoute = new TravelRouteModel
                {
                    From = from,
                    To = leg.routeInfo.to.name,
                    Distance = leg.routeInfo.distance,
                    Providers = leg.providers
                };
                CandidateRoutes.Add(candidateRoute);
            }
        }
        //For debugging purposes, REMOVE LATER!
        Console.WriteLine("Routes with the right departure point: " + CandidateRoutes.Count);
        //Now go through all the candidate routes
        foreach (TravelRouteModel candidate1 in CandidateRoutes) {
            //Find a route from all routes that starts with candidate destination, ends with user destination
            foreach (Leg leg in pricelist.legs) {
                if (leg.routeInfo.from.name == candidate1.To && leg.routeInfo.to.name == to) {
                    TravelRouteModel candidateRoute = new TravelRouteModel
                    {
                        From = candidate1.From,
                        To = to,
                        Distance = leg.routeInfo.distance,
                        Providers = leg.providers
                    };
                    //Because it matches, we can put candidate1(the first leg) and candidateRoute(the second leg) as the full flight
                    List<TravelRouteModel> FullRoute = new List<TravelRouteModel>
                    {
                        candidate1,
                        candidateRoute
                    };
                    
                    //Add the FullRoute to possible full routes for user to choose from later
                    AllFullRoutes.Add(FullRoute);
                }
            }
        }
        //Finally, return list of possible travel routes
        return AllFullRoutes;
    }
}
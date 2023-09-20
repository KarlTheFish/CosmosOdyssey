using CosmosOdyssey.Models;

namespace CosmosOdyssey.Data;

public class CalculateRoute {
    public static List<List<TravelRouteModel>> FindAvailableRoute(Pricelist pricelist, string from, string to, string? companyName) {
        List<List<TravelRouteModel>> allRoutes = new List<List<TravelRouteModel>>();
        List<TravelRouteModel> currentRoute = new List<TravelRouteModel>();
        HashSet<string> visited = new HashSet<string>(); //Using a string hashset to make sure that no place will be visited twice - this helps to avoid loops
        
        if (!string.IsNullOrEmpty(companyName)) {
            CompanyBasedRouteDFS(pricelist, from, to, allRoutes, currentRoute, visited, companyName);
        }
        else {
            FindRoutesDFS(pricelist, from, to, allRoutes, currentRoute, visited);
        }

        return allRoutes;
    }

    //Using Depth First Search algorithm for search 
    private static void FindRoutesDFS(Pricelist pricelist, string currentLocation, string destination, List<List<TravelRouteModel>> allRoutes, List<TravelRouteModel> currentRoute, HashSet<string> visited) {
        visited.Add(currentLocation);
        
        //Go through every possible route 
        foreach (Leg candidate in pricelist.legs) {
            if (candidate.routeInfo.from.name == currentLocation && !visited.Contains(candidate.routeInfo.to.name)) { //If the route departs from current location and the destination has not been visited yet
                TravelRouteModel candidateModel = new TravelRouteModel { 
                    From = currentLocation,
                    To = candidate.routeInfo.to.name,
                    Distance = candidate.routeInfo.distance,
                    Providers = candidate.providers
                };

                currentRoute.Add(candidateModel);

                if (candidateModel.To == destination) { //Does the candidate have the correct destination? If yes, add current route to all routes
                    allRoutes.Add(new List<TravelRouteModel>(currentRoute));
                }
                else { //Continue searching for routes, with the new current location being destination of the candidate model
                    FindRoutesDFS(pricelist, candidateModel.To, destination, allRoutes, currentRoute, visited);
                }

                // Remove the last element from current route so we can explore the other routes
                currentRoute.RemoveAt(currentRoute.Count - 1);
            }
        }
        //Finally, the visited list is empty and we can reuse the function
        visited.Remove(currentLocation);
    }

    private static void CompanyBasedRouteDFS(Pricelist pricelist, string currentLocation, string destination, List<List<TravelRouteModel>> allRoutes, List<TravelRouteModel> currentRoute, HashSet<string> visited, string companyName) {
        visited.Add(currentLocation);
        
        //Go through every possible route 
        foreach (Leg candidate in pricelist.legs) {
            if (candidate.routeInfo.from.name == currentLocation && !visited.Contains(candidate.routeInfo.to.name)) { //If the route departs from current location and the destination has not been visited yet
                TravelRouteModel candidateModel = new TravelRouteModel { 
                    From = currentLocation,
                    To = candidate.routeInfo.to.name,
                    Distance = candidate.routeInfo.distance,
                    Providers = candidate.providers
                };

                currentRoute.Add(candidateModel);

                if (candidateModel.To == destination) { //Does the candidate have the correct destination?
                    foreach (var provider in candidateModel.Providers) { //Is one of the providers the correct company?
                        if (provider.company.name == companyName) {
                            allRoutes.Add(new List<TravelRouteModel>(currentRoute));
                        }
                    }
                }
                else { //Continue searching for routes, with the new current location being destination of the candidate model
                    FindRoutesDFS(pricelist, candidateModel.To, destination, allRoutes, currentRoute, visited);
                }

                // Remove the last element from current route so we can explore the other routes
                currentRoute.RemoveAt(currentRoute.Count - 1);
            }
        }
        //Finally, the visited list is empty and we can reuse the function
        visited.Remove(currentLocation);
    }
}
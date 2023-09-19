using CosmosOdyssey.Models;

namespace CosmosOdyssey.Data;

public class CalculateRoute {
    public static List<List<TravelRouteModel>> AllPossibleRoutes;
    public static List<List<TravelRouteModel>> AllPossibleRoutesHold;
    
    public static List<List<TravelRouteModel>> FindAvalaibleRoute(Pricelist pricelist, string from, string to, List<List<TravelRouteModel>> outputList)
    {
        List<List<TravelRouteModel>> AllGoodRoutes = new List<List<TravelRouteModel>>();
        AllPossibleRoutes = new List<List<TravelRouteModel>>();
        AllPossibleRoutesHold = new List<List<TravelRouteModel>>();

        outputList = FindAvailableRoutes2(pricelist, from, to, AllGoodRoutes, null);
        
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
    
    //List for storing all possible routes
    public static List<List<TravelRouteModel>> AllFoundRoutes;
    public static void FindAvalaibleRoutes(Pricelist pricelist, string from, string to) {
        //Go through each possible route to find the ones departing from chosen location
        foreach (Leg candidate in pricelist.legs) {
            if (candidate.routeInfo.from.name == from) {
                TravelRouteModel candidateModel = new TravelRouteModel {From = from, To = candidate.routeInfo.to.name, Distance = candidate.routeInfo.distance, Providers = candidate.providers};
                List<TravelRouteModel> possibleRoute = new List<TravelRouteModel> { candidateModel };
                AllPossibleRoutes.Add(possibleRoute);
            }
        }
        //Go through all possible routes to see if any of them have fitting destination
        foreach (var TravelRoute in AllPossibleRoutes) {
            if (TravelRoute[^1].To == to) { //if TravelRoute[TravelRoute.count - 1].To == to
                AllFoundRoutes.Add(TravelRoute);
            }
        }
        //If there are no suitable candidates, do the first check again, but now with items in the list. Add new list for every route, do not append.
        if (AllFoundRoutes.Count == 0) {
            foreach (var TravelRoute in AllPossibleRoutes) { //For every possible route in allpossibleroutes list
                foreach (Leg candidate in pricelist.legs) { //Go through all possible legs
                    if (candidate.routeInfo.from.name == TravelRoute[TravelRoute.Count - 1].To) { //If there are any routes that start from the destination of an already stored route
                        TravelRouteModel candidateModel = new TravelRouteModel {From = TravelRoute[TravelRoute.Count - 1].To, To = candidate.routeInfo.to.name, Distance = candidate.routeInfo.distance, Providers = candidate.providers};
                        //Make a copy of the TravelRoute list that is being looked at right now, add new element to the end of it
                        List<TravelRouteModel> possibleRoute = new List<TravelRouteModel>(TravelRoute);
                        possibleRoute.Add(candidateModel);
                        AllPossibleRoutes.Add(possibleRoute);
                    }
                }
            }
            foreach (var TravelRoute in AllPossibleRoutes) {
                if (TravelRoute[TravelRoute.Count - 1].To == to) {
                    AllFoundRoutes.Add(TravelRoute);
                }
            }
        }
    }

    public static List<List<TravelRouteModel>> FindAvailableRoutes2(Pricelist pricelist, string from, string to, List<List<TravelRouteModel>> AllGoodRoutes, List<TravelRouteModel>? possibleRoute) {
        foreach (Leg candidate in pricelist.legs) {
            if (candidate.routeInfo.from.name == from) {
                TravelRouteModel candidateModel = new TravelRouteModel {
                    From = from, To = candidate.routeInfo.to.name, Distance = candidate.routeInfo.distance,
                    Providers = candidate.providers };
                if (possibleRoute == null) {
                    possibleRoute = new List<TravelRouteModel> { candidateModel };
                }
                else {
                    possibleRoute = new List<TravelRouteModel>(possibleRoute);
                    possibleRoute.Add(candidateModel);
                }
                AllPossibleRoutes.Add(possibleRoute);
            }
        }
        //Look through all possible routes and see if any of them match the chosen destination
        foreach (var TravelRoute in AllPossibleRoutes) {
            if (TravelRoute[TravelRoute.Count - 1].To == to) {
                AllGoodRoutes.Add(TravelRoute);
            }
        }
        //If there are no suitable candidates, call the function again, but now with the items in the list.
        if (AllGoodRoutes.Count == 0) {
            List<List<TravelRouteModel>> AllPossibleRoutesCopy = new List<List<TravelRouteModel>>(AllPossibleRoutes);
            foreach (List<TravelRouteModel> TravelRoute in AllPossibleRoutesCopy) {
                FindAvailableRoutes2(pricelist, TravelRoute[^1].To, to, AllGoodRoutes, possibleRoute);
            }
        }
        return AllGoodRoutes;
    }
    
    public static List<List<TravelRouteModel>> FindAvailableRoutes3(Pricelist pricelist, string from, string to, List<TravelRouteModel>? possibleRoute)
    {
        List<List<TravelRouteModel>> AllGoodRoutes = new List<List<TravelRouteModel>>();

        foreach (Leg candidate in pricelist.legs)
        {
            if (candidate.routeInfo.from.name == from)
            {
                TravelRouteModel candidateModel = new TravelRouteModel
                {
                    From = from,
                    To = candidate.routeInfo.to.name,
                    Distance = candidate.routeInfo.distance,
                    Providers = candidate.providers
                };
                if (possibleRoute == null)
                {
                    possibleRoute = new List<TravelRouteModel> { candidateModel };
                }
                else
                {
                    possibleRoute = new List<TravelRouteModel>(possibleRoute);
                    possibleRoute.Add(candidateModel);
                }

                if (possibleRoute[possibleRoute.Count - 1].To == to)
                {
                    // If the destination is reached, add the route to AllGoodRoutes
                    AllGoodRoutes.Add(new List<TravelRouteModel>(possibleRoute));
                }
                else
                {
                    // Continue exploring routes recursively
                    List<List<TravelRouteModel>> subRoutes = FindAvailableRoutes3(pricelist, possibleRoute[possibleRoute.Count - 1].To, to, possibleRoute);
                    AllGoodRoutes.AddRange(subRoutes);
                }
            }
        }

        return AllGoodRoutes;
    }



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
    }
}
using CosmosOdyssey.Models;

namespace CosmosOdyssey.Data;

public class CalculateRoute {
    public static List<List<TravelRouteModel>> AllPossibleRoutes;
    public static string firstFrom;
    public static string firstTo;
    
    public static List<List<TravelRouteModel>> FindAvalaibleRoute(Pricelist pricelist, string from, string to, List<List<TravelRouteModel>> outputList)
    {
        firstFrom = from;
        firstTo = to;
        List<List<TravelRouteModel>> AllGoodRoutes = new List<List<TravelRouteModel>>();
        AllPossibleRoutes = new List<List<TravelRouteModel>>();

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
public static int Protector = 0;
    public static List<List<TravelRouteModel>> FindAvailableRoutes2(Pricelist pricelist, string from, string to, List<List<TravelRouteModel>> AllGoodRoutes, List<TravelRouteModel>? possibleRoute) {
        Protector = Protector + 1;
        bool possibleRouteCreated = false;
        foreach (Leg candidate in pricelist.legs) { //At the very start assume that the route does not have possibleRoute created that contains it
            possibleRouteCreated = false;
            if (candidate.routeInfo.from.name == from && candidate.routeInfo.to.name != firstFrom && candidate.routeInfo.from.name != firstTo) { //Check that the route does not take to the original departure point
                Console.WriteLine("Searching for all routes from " + from);
                TravelRouteModel candidateModel = new TravelRouteModel {
                    From = from, To = candidate.routeInfo.to.name, Distance = candidate.routeInfo.distance,
                    Providers = candidate.providers };
                if (possibleRoute == null) {
                    possibleRoute = new List<TravelRouteModel> { candidateModel };
                    possibleRouteCreated = true;
                }
                else {
                    //Among the already found possible routes, are there any with the final "To" field matching candidateModel's "From" field? If yes, add it to the list
                    foreach (var route in AllPossibleRoutes) {
                        if ((route[^1].To) == candidateModel.From) { 
                            possibleRoute = new List<TravelRouteModel>(possibleRoute);
                            possibleRoute.Add(candidateModel);
                            possibleRouteCreated = true;
                        }
                    }
                    //If by now no possibleRoute exists for the current route model, create it now
                    if (possibleRouteCreated == false) {
                        possibleRoute = new List<TravelRouteModel> { candidateModel };
                    }
                }
                AllPossibleRoutes.Add(possibleRoute);
            }
        }
        Console.WriteLine("Possible routes:");
        foreach (var VARIABLE in AllPossibleRoutes) {
            Console.WriteLine("____________________________________");
            foreach (var var in VARIABLE) {
                Console.WriteLine("From " + var.From + " To " + var.To);
            }
        }
        Console.WriteLine("____________________________________");

        //Look through all possible routes and see if any of them match the chosen destination
        foreach (var TravelRoute in AllPossibleRoutes) {
            if (TravelRoute[TravelRoute.Count - 1].To == to) {
                if (!AllGoodRoutes.Contains(TravelRoute)) {
                    Console.WriteLine("Adding valid route. Target destination: " + to);
                    foreach (var var in TravelRoute) {
                        Console.WriteLine("From " + var.From); Console.WriteLine("To " + var.To);
                    }
                    Console.WriteLine("-----------------------------------");
                    AllGoodRoutes.Add(TravelRoute);
                }
            }
        }
        //If there are no suitable candidates, call the function again, but now with the items in the list.
        if (AllGoodRoutes.Count < 2) {
            List<List<TravelRouteModel>> AllPossibleRoutesCopy = new List<List<TravelRouteModel>>(AllPossibleRoutes);
            for (int i = AllPossibleRoutesCopy.Count - 1; i >= 0; i--) {
                List<TravelRouteModel> TravelRoute = AllPossibleRoutesCopy[i];
                Console.WriteLine("Inside recursive call. From " + TravelRoute[^1].From + " To " + to);
                if (Protector > 50) {
                    Console.WriteLine("50 times called. Stopping.");
                    break;
                }
                FindAvailableRoutes2(pricelist, TravelRoute[^1].To, to, AllGoodRoutes, possibleRoute);
            }
            /*foreach (List<TravelRouteModel> TravelRoute in AllPossibleRoutesCopy) {
                Console.WriteLine("Inside recursive call. From " + TravelRoute[^1].To + " To " + to);
                if (Protector > 10) {
                    Console.WriteLine("10 times called. Stopping.");
                    break;
                }
                FindAvailableRoutes2(pricelist, TravelRoute[^1].To, to, AllGoodRoutes, possibleRoute);
            }*/
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
using CosmosOdyssey.Models;

public class FilterRouteResults
{
    public static List<List<TravelRouteModel>> FilteredRouteByCompany(string company, List<List<TravelRouteModel>> FullRouteOptions)
    {
        List<List<TravelRouteModel>> outputList = new List<List<TravelRouteModel>>();

        foreach (var fullRoute in FullRouteOptions)
        {
            List<TravelRouteModel> suitableRoute = new List<TravelRouteModel>();
            foreach (var route in fullRoute)
            {
                foreach (var provider in route.Providers)
                {
                    if (provider.company.name == company)
                    {
                        suitableRoute.Add(route);
                        break; // Exit the inner loop once a matching provider is found
                    }
                }
            }
            outputList.Add(suitableRoute);
        }

        return outputList;
    }
}
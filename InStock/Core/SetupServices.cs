using System.Collections.Generic;
using InStock.Core.Data;
using InStock.Core.Regions;
using InStock.Core.Services;
using InStock.Core.Services.Amazon;

namespace InStock.Core;

public static class SetupServices
{
    private static readonly Dictionary<string, string> UKServices = new()
    {
        // List of asins & links
    };
    public static void SetupAll()
    {
        AmazonService service = new(new ServiceData(120, Region.UK, ServiceType.Amazon));
        foreach (KeyValuePair<string, string> kvp in UKServices)
        {
            service.CreateProduct(kvp.Key, kvp.Value);
        }
    }
}
using System.Collections.Generic;
using InStock.Core.Data;
using InStock.Core.Http;
using InStock.Core.Regions;

namespace InStock.Core.Services.Amazon;

public class AmazonService : IAmazonService
{
    private ServiceData _serviceData;

    private List<IProduct> _products;
    
    public AmazonService(ServiceData data)
    {
        _serviceData = data;
        _products = new();
    }
    
    public ServiceType GetServiceType()
    {
        return _serviceData.ServiceType;
    }

    public Region GetRegion()
    {
        return _serviceData.Region;
    }

    public int GetTimeout()
    {
        return _serviceData.Timeout;
    }
    
    public void CreateProduct(string asin, string link)
    {
        _products.Add(new AmazonProduct(_serviceData, asin, link));
    }

    public AmznServiceDispatcher GetDispatcher()
    {
        return _serviceData.ServiceDispatcher;
    }
    
    
}
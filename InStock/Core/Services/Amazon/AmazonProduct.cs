using System;
using System.Threading;
using System.Timers;
using InStock.Core.Data;
using InStock.Core.Http;
using InStock.Core.Regions;
using Timer = System.Timers.Timer;

namespace InStock.Core.Services.Amazon;

public class AmazonProduct : IProduct
{
    private int _timeout;
    private readonly Region _region;
    private string _asin;

    private AmazonLink _amazonLink;
    private HttpAgent _agent;
    private ProductData _productData;

    private AmazonParser _parser;

    private Logger _logger;

    private string _affiliateLink;
    // create thread
    
    public AmazonProduct(ServiceData serviceData, string asin, string link)
    {
        _timeout = serviceData.Timeout;
        _region = serviceData.Region;
        _asin = asin;
        _affiliateLink = link;
        
        _productData = new()
        {
            Name = "",
            Price = "",
            Stock = "",
        };

        _logger = new Logger(serviceData.ServiceType, serviceData.Region);

        _amazonLink = new AmazonLink(asin, _region);
        
        CreateAgent();

        _parser = new AmazonParser();
        
        new Thread(InitializeDataLoop).Start();
    }
    
    private string FetchHtml()
    {
        return _agent.FetchBody();
    }

    private Timer _dataLoop;
    private void InitializeDataLoop()
    {
        HandleData();
        
        _dataLoop = new(_timeout * 1000)
        {
            Enabled = true,
            AutoReset = true
        };
        
        _dataLoop.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
        {
            HandleData();
        };
    }

    private void CreateAgent()
    {
        _agent = new(_amazonLink.ToString(), _timeout);
    }

    private void HandleData()
    {
        CreateAgent();
        ProductData newData;
        newData = _parser.ParseHtml(FetchHtml());

        if (_productData.IsEmpty())
        {
            _productData = newData;
            _logger.PushProduct(_productData, _affiliateLink);
            return;
        }
        
        ComparePrice(newData);
        CompareStock(newData);

        _productData = newData; // Finally, set the old data to new.
        _agent.Dispose();
    }

    private void ComparePrice(ProductData newData)
    {
        if (_productData.Price == newData.Price) return;
        
        _logger.UpdatePrice(newData.Price, _productData.Price);
    }

    private void CompareStock(ProductData newData)
    {
        if (_productData.Stock == newData.Stock) return;
        
        _logger.UpdateStock(newData.Stock, _productData.Stock);
    }

    public int GetTimeout()
    {
        return _timeout;
    }

    public HttpAgent GetAgent()
    {
        return _agent;
    }

    public string GetPrice()
    {
        return _productData.Price;
    }

    public string GetName()
    {
        return _productData.Name;
    }

    public string GetStock()
    {
        return _productData.Stock;
    }

    public string GetAffiliateLink()
    {
        return _affiliateLink;
    }
}
using InStock.Core.Http;

namespace InStock.Core.Services;

public interface IProduct
{
    int GetTimeout(); // Get data from Service
    HttpAgent GetAgent();
    string GetPrice();
    string GetName();
    string GetStock();
    string GetAffiliateLink();
}
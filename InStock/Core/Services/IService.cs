using InStock.Core.Http;
using InStock.Core.Regions;

namespace InStock.Core.Services
{
    public interface IService
    {
        ServiceType GetServiceType();
        Region GetRegion();
        int GetTimeout();
        void CreateProduct(string asin, string link);
    }
}
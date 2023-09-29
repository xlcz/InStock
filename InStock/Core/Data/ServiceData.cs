using InStock.Core.Regions;
using InStock.Core.Services;
using InStock.Core.Services.Amazon;

namespace InStock.Core.Data;

public struct ServiceData
{
    public int Timeout;
    public Region Region;
    public AmznServiceDispatcher ServiceDispatcher;
    public ServiceType ServiceType;

    public ServiceData(int timeout, Region region, ServiceType serviceType)
    {
        Timeout = timeout;
        Region = region;
        ServiceType = serviceType;
    }
}
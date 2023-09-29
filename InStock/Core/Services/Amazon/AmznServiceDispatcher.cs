using System.Collections.Generic;

namespace InStock.Core.Services.Amazon;

public class AmznServiceDispatcher
{
    private List<IService> Services;

    public AmznServiceDispatcher()
    {
        Services = new();
    }

    public void DispatchService()
    {
        
    }
}
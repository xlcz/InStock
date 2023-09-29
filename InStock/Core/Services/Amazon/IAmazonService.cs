using InStock.Core.Regions;

namespace InStock.Core.Services.Amazon;

public interface IAmazonService : IService
{
    Amazon.AmznServiceDispatcher GetDispatcher();
    
}
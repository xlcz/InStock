using InStock.Core.Data;
using InStock.Core.Discord;
using InStock.Core.Regions;

namespace InStock.Core.Services;

public class Logger
{
    private string _serviceName, _productName = "", _affiliateLink = "";
    private ServiceType _serviceType;
    
    public Logger(ServiceType serviceType, Region region)
    {
        _serviceName = $"[{serviceType.ToString()} | {region.ToString()}]";
    }

    public void PushProduct(ProductData productData, string link)
    {
        _productName = productData.Name;
        _affiliateLink = link;
        
        DiscordClient.Messenger.SendProductEmbed(productData, _affiliateLink);
    }

    public void UpdatePrice(string newPrice, string oldPrice)
    {
        DiscordClient.Messenger.SendEmbed(_productName,
            "Price Change",
            $"The price for this product has changed from {oldPrice} to {newPrice}",
            _affiliateLink);
    }
    
    public void UpdateStock(string newStock, string oldStock)
    {
        DiscordClient.Messenger.SendEmbed(_productName,
            "Stock Change",
            $"The stock for this product has changed from {oldStock} to {newStock}",
            _affiliateLink);
    }
}
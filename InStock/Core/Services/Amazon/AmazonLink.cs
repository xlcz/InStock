using System.Collections.Generic;
using InStock.Core.Regions;

namespace InStock.Core.Services.Amazon;

public struct AmazonLink
{
    private static readonly Dictionary<Region, string> TLDs = new()
    {
        { Region.COM, "com" },
        { Region.UK, "co.uk" },
        { Region.GERMANY, "de" }
    };
    
    public string ASIN, TLD;
    
    public AmazonLink(string asin, Region region)
    {
        ASIN = asin;
        TLD = TLDs[region];
    }

    public override string ToString() => 
        $"https://amazon.{TLD}/-/en/dp/{ASIN}"; // [/-/en/] makes content english
}
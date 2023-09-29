using HtmlAgilityPack;
using InStock.Core.Data;

namespace InStock.Core.Services.Amazon;

public class AmazonParser
{
    //private string _html;
    private HtmlDocument _htmlDoc;    
    private static readonly string NotInStock = "Not in stock";
    
    public AmazonParser()
    {
    }
    
    public ProductData ParseHtml(string html)
    {
        if (html == string.Empty) return ProductData.Empty;
        
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(html);

        _htmlDoc = htmlDoc;
        ProductData productData = new();

        productData.Stock = GetStock();
        productData.Price = GetPrice();
        productData.Name = GetName();

        return productData;
        // Valid XPath to availability : /html[1]/body[1]/div[2]/div[1]/div[7]/div[4]/div[4]/div[21]/div[1]
        // Valid XPath to ^/span : /html[1]/body[1]/div[2]/div[1]/div[7]/div[4]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/form[1]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[5]/div[1]/div[1]/span[1]

    }

    private string _productName;
    private string GetName()
    {
        return _productName ??= _htmlDoc.GetElementbyId("title").ChildNodes[1].InnerHtml.TrimStart().TrimEnd();
    }

    private string GetStock()
    {
        bool inStock = true;

        try
        {
            _htmlDoc.GetElementbyId("add-to-cart-button").InnerHtml.ToCharArray();
        }
        catch
        {
            inStock = false;
        }
        
        if (!inStock) return NotInStock;
        return _htmlDoc.GetElementbyId("availabilityInsideBuyBox_feature_div")
            .Element("div")
            .Element("div")
            .Element("span").InnerHtml
            .TrimStart()
            .TrimEnd(); // Longer but gets true value.
    }

    private string _price = "Unavailable";
    private string GetPrice()
    {
        // /html[1]/body[1]/div[2]/div[1]/div[5]/div[2]/div[1]/div[2]/div[4]/div[3]/div[2]/div[1]/table[1]/tr[1]/td[2]/span[1]/span[1]/#text[1]
        string price = "";
            
        try
        {
            price = _htmlDoc.GetElementbyId("corePrice_feature_div")
                .Element("div")
                .Element("span")
                .Element("span").InnerHtml;
        }
        catch
        {
            try
            {
                price = _htmlDoc.GetElementbyId("corePrice_feature_div")
                    .Element("div")
                    .Element("div")
                    .Element("span")
                    .Element("span").InnerHtml;
            }
            catch
            {
                price = _price ;
            }
        }

        _price = price;
        return price;
    }
}
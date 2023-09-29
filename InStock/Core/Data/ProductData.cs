using System;

namespace InStock.Core.Data;

public struct ProductData
{
    public string Name,
        Price,
        Stock;

    public static ProductData Empty => new ProductData();

    public bool IsEmpty()
    {
        return (Name == string.Empty && Price == String.Empty && Stock == String.Empty);
    }
}
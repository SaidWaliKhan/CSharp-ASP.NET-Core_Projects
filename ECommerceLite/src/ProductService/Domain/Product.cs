namespace ProductService.Domain;

public class Product
{
    public Guid Id {get; init;}
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
    public int StockQuantity { get; set; }

}
namespace Basket.Library
{
    public interface IProduct
    {
        Category Category { get; }
        double Price { get; }
        string Title { get; }

        Product Add(Product product);
    }
}
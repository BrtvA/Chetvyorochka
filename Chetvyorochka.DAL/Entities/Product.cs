namespace Chetvyorochka.DAL.Entities
{
    public class Product: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ProductTypeId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public List<Basket> Baskets { get; set; } = new();
        public ProductType? ProductTypes { get; set; }
    }
}

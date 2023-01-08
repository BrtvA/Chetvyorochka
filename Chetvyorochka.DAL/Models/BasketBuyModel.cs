namespace Chetvyorochka.DAL.Models
{
    internal class BasketBuyModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProductCount { get; set; }
        public int BasketCount { get; set; }
        public string Description { get; set; } = null!;
        public int ProductTypeId { get; set; }
        public decimal Price { get; set; }
    }
}

using ShoesDE.Database;
using System.Windows.Media;

namespace ShoesDE.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Article = product.Article;
            Name = product.Name;

            Price = product.Price;
            Discount = product.Discount;

            AmountInStock = product.AmountInStock;
            Description = product.Description;

            Photo = product.Photo;

            Category = product.Category;
            Producer = product.Producer;
            Provider = product.Provider;
            Unit = product.Unit;

            GetPhoto();
            GetBackground();
            GetPrice();
        }

        public int Id { get; set; }

        public string Article { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal AmountInStock { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public Category Category { get; set; }

        public Producer Producer { get; set; }

        public Provider Provider { get; set; }

        public Unit Unit { get; set; }

        public Brush Background { get; set; }

        private void GetBackground()
        {
            if (AmountInStock == 0)
            {
                Background = Brushes.LightBlue;
            }
            else if (Discount >= 15)
            {
                Background = (Brush)new BrushConverter()
                    .ConvertFromString("#2E8B57");
            }
            else
            {
                Background = Brushes.White;
            }
        }

        private void GetPhoto()
        {
            if (string.IsNullOrWhiteSpace(Photo))
            {
                Photo = "/Res/picture.png";
            }
        }

        private void GetPrice()
        {
            if (Discount <= 0)
                return;

            OldPrice = Price;

            Price = OldPrice * (1 - Discount / 100);
        }
    }
}
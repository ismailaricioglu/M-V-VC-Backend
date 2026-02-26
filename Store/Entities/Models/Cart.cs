namespace Entities.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; }
        public Cart()
        {
            Lines = new List<CartLine>();
        }

        /// <summary>
        /// Sepete yeni bir ürün ekler. Eðer ürün zaten sepette varsa, miktarýný artýrýr.
        /// </summary>
        /// <param name="product">Eklenecek ürün.</param>
        /// <param name="quantity">Eklenecek ürün miktarý.</param>
        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = Lines
                .FirstOrDefault(l => l.Product.ProductId == product.ProductId);

            if (line is null)
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        /// <summary>
        /// Sepetten belirtilen ürünü kaldýrýr.
        /// </summary>
        /// <param name="product">Kaldýrýlacak ürün.</param>
        public virtual void RemoveLine(Product product) =>
            Lines.RemoveAll(l => l.Product.ProductId.Equals(product.ProductId));

        /// <summary>
        /// Sepetteki tüm ürünlerin toplam deðerini hesaplar.
        /// </summary>
        /// <returns>Toplam sepet tutarý (decimal).</returns>
        public decimal ComputeTotalValue() =>
            Lines.Sum(e => e.Product.Price * e.Quantity);

        /// <summary>
        /// Sepeti tamamen temizler.
        /// </summary>
        public virtual void Clear() => Lines.Clear();
    }
}
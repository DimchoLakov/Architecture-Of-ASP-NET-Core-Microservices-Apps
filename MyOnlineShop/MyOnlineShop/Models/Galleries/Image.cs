namespace MyOnlineShop.Models.Products
{
    public class Image
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }

        public bool IsPrimary { get; set; }

        public string MimeType { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

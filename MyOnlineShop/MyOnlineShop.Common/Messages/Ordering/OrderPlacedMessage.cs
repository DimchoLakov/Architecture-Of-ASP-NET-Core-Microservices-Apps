namespace MyOnlineShop.Common.Messages.Ordering
{
    public class OrderPlacedMessage
    {
        public string UserId { get; set; }

        public int Total { get; set; }
    }
}

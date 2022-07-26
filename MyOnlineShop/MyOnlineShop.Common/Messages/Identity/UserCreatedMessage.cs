namespace MyOnlineShop.Common.Messages.Identity
{
    public record UserCreatedMessage
    {
        public string UserId { get; set; }

        public string Email { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
    }
}

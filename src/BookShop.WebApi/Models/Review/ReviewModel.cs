namespace BookShop.WebApi.Models.Review
{
    public record ReviewModel
    {
        public byte Score { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserImagePath { get; set; }

    }
}

namespace Wallet.Models
{
    public class Card
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public long? CardNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? CVV { get; set; }
        public string? Bank { get; set; }
        public string? Brand { get; set; }
        public string? CardBackgroundColor { get; set; }
        public bool ShowBadge { get; set; }
    }
}

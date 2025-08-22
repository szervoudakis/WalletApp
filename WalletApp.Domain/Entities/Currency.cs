namespace WalletApp.Domain.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public required string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        
    }
}
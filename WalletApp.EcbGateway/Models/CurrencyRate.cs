namespace WalletApp.EcbGateway.Models
{
    public class CurrencyRate
    {
        public required string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; } 
    }
}
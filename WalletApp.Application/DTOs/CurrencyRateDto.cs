namespace WalletApp.Application.DTOs
{
    public class CurrencyRateDto
    {
        public required string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}

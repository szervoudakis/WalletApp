namespace WalletApp.Application.DTOs
{
    public class WalletBalanceDto
    {
        public long WalletId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "EUR";
    }
}

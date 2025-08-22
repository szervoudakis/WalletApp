namespace WalletApp.CurrencyApi.Models
{//DTO used for user registration reguests
    public class RegisterRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
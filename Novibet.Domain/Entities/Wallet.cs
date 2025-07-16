namespace Novibet.Domain.Entities
{
    public class Wallet
    {
        public long Id { get; private set; } //unique Id for wallet

        public decimal Balance { get; private set; } //current ballance

        public string Currency { get; private set; }

        public Wallet(long id, string currency)
        {
            Id = id;
            Currency = currency;
            Balance = 0;  //init balance with 0
        }

        //method for adding money to ballance
        public void AddFunds(decimal ammount)
        {
            if (ammount <= 0) throw new ArgumentException("Amount must be positive.");
            Balance += ammount;
        }

        public void SubstractFunds(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            if (Balance < amount) throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
        }
        
        public void ForceSubtractFunds(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            Balance -= amount;
        }
    }
}
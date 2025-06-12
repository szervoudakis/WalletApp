namespace Novibet.Domain.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        
    }
}
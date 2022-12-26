namespace PaymentAPI.Models;

public class Card
{
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public DateOnly ValidThrough { get; set; }
    public int Cvc { get; set; }
}
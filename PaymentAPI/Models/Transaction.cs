namespace PaymentAPI.Models;

public class Transaction
{
    public required int Id { get; set; }
    public required DateTime DateTime { get; set; }
    public required int Sum { get; set; }
    public required Card SentFrom { get; set; }
    public required Card SentTo { get; set; }
}
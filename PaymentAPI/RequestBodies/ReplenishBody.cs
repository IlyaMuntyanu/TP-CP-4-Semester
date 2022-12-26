namespace PaymentAPI.RequestBodies;

public class ReplenishBody
{
    public int CardNumber { get; set; }
    public int Sum { get; set; }
}
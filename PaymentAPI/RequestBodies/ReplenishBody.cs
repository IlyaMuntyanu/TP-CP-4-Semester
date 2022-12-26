namespace PaymentAPI.RequestBodies;

public class ReplenishBody
{
    public long CardNumber { get; set; }
    public int Sum { get; set; }
}
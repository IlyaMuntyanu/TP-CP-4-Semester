namespace PaymentAPI.RequestBodies;

public class CardBody
{
    public long CardNumber { get; set; }
    public DateOnly ValidThrough { get; set; }
    public int Cvc { get; set; }
}
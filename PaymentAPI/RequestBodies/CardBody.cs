namespace PaymentAPI.RequestBodies;

public class CardBody
{
    public long CardNumber { get; set; }
    public int ValidThroughMonth { get; set; }
    public int ValidThroughYear { get; set; }
    public int Cvc { get; set; }
}
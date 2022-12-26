namespace TP_CP_5_Semester.PaymentApiClient.RequestBodies;

public class TransferBody
{
    public long FromCardNumber { get; set; }
    public int FromValidThroughMonth { get; set; }
    public int FromValidThroughYear { get; set; }
    public int FromCvc { get; set; }
    public long ToCardNumber { get; set; }
    public int Sum { get; set; }
}
namespace TP_CP_5_Semester.Configuration;

public class PaymentConfiguration
{
    public long CardNumber { get; set; }
    public int ValidThroughMonth { get; set; }
    public int ValidThroughYear { get; set; }
    public int Cvc { get; set; }
}
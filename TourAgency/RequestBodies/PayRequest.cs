namespace TP_CP_5_Semester.RequestBodies;

public class PayRequest
{
    public int BookingId { get; set; }
    public long CardNumber { get; set; }
    public int ValidThroughMonth { get; set; }
    public int ValidThroughYear { get; set; }
    public int Cvc { get; set; }
    
}
namespace TP_CP_5_Semester.RequestBodies;

public class CancelRequest
{
    public required int BookingId { get; set; }
    public long? CardNumber { get; set; }
}
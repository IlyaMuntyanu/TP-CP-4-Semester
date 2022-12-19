namespace TP_CP_5_Semester.RequestBodies;

public class OrderRequest
{
    public string Email { get; set; }
    public long TourId { get; set; }
    public int Amount { get; set; }
}
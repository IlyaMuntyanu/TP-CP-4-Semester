namespace TP_CP_5_Semester.Models;

public class Tour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Price { get; set; }
    public bool Deleted { get; set; }
    public int Leftover { get; set; }
}
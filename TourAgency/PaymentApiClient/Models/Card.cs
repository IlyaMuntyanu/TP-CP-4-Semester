using System.Text.Json.Serialization;

namespace TP_CP_5_Semester.PaymentApiClient.Models;

public class Card
{
    [JsonIgnore]
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public int ValidThroughMonth { get; set; }
    public int ValidThroughYear { get; set; }
    public int Cvc { get; set; }
    public CardStatus CardStatus { get; set; }
    
    public int Balance { get; set; }
}
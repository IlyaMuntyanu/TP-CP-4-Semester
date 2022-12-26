using System.Text.Json.Serialization;

namespace TP_CP_5_Semester.PaymentApiClient.Models;

public class CardStatus
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
}
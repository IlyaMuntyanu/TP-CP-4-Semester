using System.Text.Json.Serialization;

namespace PaymentAPI.Models;

public class CardStatus
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
}
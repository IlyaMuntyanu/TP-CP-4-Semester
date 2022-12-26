using System.Text.Json.Serialization;

namespace PaymentAPI.Models;

public class Card
{
    [JsonIgnore]
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public DateOnly ValidThrough { get; set; }
    public int Cvc { get; set; }
    public CardStatus CardStatus { get; set; }
    
    public int Balance { get; set; }
}
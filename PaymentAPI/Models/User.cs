using System.Text.Json.Serialization;

namespace PaymentAPI.Models;

public class User
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required List<Card> Cards { get; set; }
    
    [JsonIgnore]
    public byte[]? PasswordHash { get; set; }

    [JsonIgnore]
    public byte[]? PasswordSalt { get; set; }
}
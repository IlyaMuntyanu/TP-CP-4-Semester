using System.Text.Json.Serialization;

namespace PaymentAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public List<Card> Cards { get; set; }
    
    [JsonIgnore]
    public byte[]? PasswordHash { get; set; }

    [JsonIgnore]
    public byte[]? PasswordSalt { get; set; }
}
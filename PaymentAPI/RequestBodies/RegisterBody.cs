namespace PaymentAPI.RequestBodies;

public class RegisterBody
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}
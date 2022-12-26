namespace TP_CP_5_Semester.PaymentApiClient;

public class Client
{
    private readonly Uri _baseAddress = new("https://localhost:7264");

    public async void TransferCard(
        long senderCardNumber,
        int senderValidThroughMonth,
        int senderValidThroughYear,
        int senderCvc,
        long reciverCardNumber,
        int sum
    )
    {
        using var client = new HttpClient();
        client.BaseAddress = _baseAddress;
        var body = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("fromCardNumber", senderCardNumber.ToString()),
            new KeyValuePair<string, string>("fromValidThroughMonth", senderValidThroughMonth.ToString()),
            new KeyValuePair<string, string>("fromValidThroughYear", senderValidThroughYear.ToString()),
            new KeyValuePair<string, string>("fromCvc", senderCvc.ToString()),
            new KeyValuePair<string, string>("toCardNumber", reciverCardNumber.ToString()),
            new KeyValuePair<string, string>("sum", sum.ToString())
        });
        var result = await client.PostAsync("/Card/Replenish", body);
        var resultContent = await result.Content.ReadAsStringAsync();
    }
}
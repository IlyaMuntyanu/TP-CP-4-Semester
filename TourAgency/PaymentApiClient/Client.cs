using System.Net;
using System.Text;
using System.Text.Json;
using TP_CP_5_Semester.PaymentApiClient.RequestBodies;

namespace TP_CP_5_Semester.PaymentApiClient;

public class Client
{
    private readonly Uri _baseAddress = new("https://localhost:7264");

    public async Task<HttpStatusCode> TransferCard(TransferBody body)
    {
        using var client = new HttpClient();
        client.BaseAddress = _baseAddress;
        
        
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var result = await client.PostAsync("/Card/Transfer", content);

        return result.StatusCode;
    }
}
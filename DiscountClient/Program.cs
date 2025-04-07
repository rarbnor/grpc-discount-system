using DiscountShared;
using Grpc.Net.Client;

Console.WriteLine("Starting gRPC client...");

// Allow HTTP/2 unencrypted support for localhost
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new DiscountService.DiscountServiceClient(channel);

// 1. Generate some codes
var generateResponse = await client.GenerateCodesAsync(new GenerateRequest
{
    Count = 5,
    Length = 8
});

Console.WriteLine($"Generated: {generateResponse.Result}");

if (!generateResponse.Result)
{
    Console.WriteLine("Failed to generate codes. Exiting...");
    return;
}

// 2. Ask user for a code to use
Console.Write("Enter a code to use: ");
var inputCode = Console.ReadLine() ?? "";

var useResponse = await client.UseCodeAsync(new UseCodeRequest
{
    Code = inputCode
});

Console.WriteLine($"UseCode result: {useResponse.Result} (0=OK, 1=NotFound, 2=Used)");

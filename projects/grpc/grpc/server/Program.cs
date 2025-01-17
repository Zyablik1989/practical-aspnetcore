using Grpc.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder();
builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(k =>
{
    k.ConfigureEndpointDefaults(options => options.Protocols = HttpProtocols.Http2);
    k.ListenLocalhost(5500, o => o.UseHttps());
});

var app = builder.Build();

app.MapGrpcService<BillboardService>();
//app.MapGrpcService<StatusCheck>();
//app.MapGrpcService<CheckStatusService>();
app.MapGet("/", () => "This server contains a gRPC service");

app.Run();

public class StatusCheckService : StatusCheck.Status.StatusBase
{

}

public class BillboardService : Billboard.Board.BoardBase
{
    public override Task<Billboard.MessageReply> ShowMessage(Billboard.MessageRequest request, ServerCallContext context)
    {
        var now = DateTime.UtcNow;


        return Task.FromResult(new Billboard.MessageReply
        {
            DisplayTime = long.MaxValue,
            ReceiveFrom = request.Sender
        });
    }
}
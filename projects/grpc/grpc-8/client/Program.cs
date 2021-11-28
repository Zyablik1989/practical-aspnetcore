using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using Grpc.Net.Client;
using System.Net.Http;

namespace GrpcServer
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            //Make sure that the grpc-server is run 
            app.Run(async context =>
            {
                //We need this switch because we are connecting to an unsecure server. If the server runs on SSL, there's no need for this switch.
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

                var channel = GrpcChannel.ForAddress("http://localhost:5500"); //check the values at /server project
                var client = new Billboard.Board.BoardClient(channel);
                var request = new Billboard.MessageRequest();
                request.WageValue = 10_000;

                var reply = await client.ShowMessageAsync(request);

                var displayDate = new DateTime(reply.ReceivedTime);
                await context.Response.WriteAsync($"We sent a message to a gRPC server and  received  the following reply \n'\n{reply.Message}' \non {displayDate} ");
            });
        }
    }

}
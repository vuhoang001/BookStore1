using BuildingBlocks.Chassis.ApiDocument;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();


app.MapScalarGateway(builder.Configuration,
                     versions: ["v1", "v2"],
                     scalarPath: "/scalar"
);


app.MapReverseProxy();

app.Run();
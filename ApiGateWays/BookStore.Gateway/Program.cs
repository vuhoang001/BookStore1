using BuildingBlocks.Chassis.ApiDocument;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();


app.MapScalarFromYarp(builder.Configuration);


app.MapReverseProxy();

app.Run();
using BookStore.Basket.Extensions;
using BuildingBlocks.Chassis.ApiDocument;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;

var builder = WebApplication.CreateBuilder(args);


builder.AddApplicationService();

builder.Services.AddSwaggerWithForwardedPrefix("/basket");


var app = builder.Build();


app.UseSwaggerWithForwardedPrefix();
app.UseHttpsRedirection();


var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(Versions.V1)
    .ReportApiVersions()
    .Build();

app.MapEndpoints(apiVersionSet);


app.Run();
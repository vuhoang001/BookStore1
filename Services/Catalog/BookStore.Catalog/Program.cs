using BookStore.Catalog.Extensions;
using BuildingBlocks.Chassis.ApiDocument;
using BuildingBlocks.Chassis.EndPoints;
using BuildingBlocks.Constants.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationService();

builder.Services.AddApiDocument("Catalog", configure: o => o
                                    .WithVersions("v1", "v2")
                                    .WithPrefix("/Catalog")
                                    .WithDescription("Catalog Service - Shopping cart and item management")
                                    .WithJwtAuth()
);
var app = builder.Build();

app.UseExceptionHandler();
app.UseApiDocument();
app.UseHttpsRedirection();


var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(Versions.V1)
    .HasApiVersion(Versions.V2)
    .ReportApiVersions()
    .Build();

app.MapEndpoints(apiVersionSet);


app.Run();
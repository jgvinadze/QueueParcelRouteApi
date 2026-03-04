using Microsoft.AspNetCore.Mvc;
using QueueParcelRouteApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IOracleSqlText, OracleSqlText>();
builder.Services.AddSingleton<IMariaDbSqlText, MariaDbSqlText>();
builder.Services.AddTransient<DapperDbConnectionFactory>();
builder.Services.AddScoped<RoutesEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Logger.LogInformation("service QueueParcelRouteApi started");

app.UseHttpsRedirection();

app.MapGet("/ParcelRoutes/InsParcelRoutesInMariaDbAsync", async ([FromServices] RoutesEndpoints endPoints) => {

    var result = await endPoints.InsParcelRoutesInMariaDbAsync();

    if (result == null)
        return Results.BadRequest();

    return Results.Ok(result);
});

app.MapPost("/ParcelRoutes/DeleteProcessedParcelsAndRoutesAsync", async ([FromServices] RoutesEndpoints endPoints) =>
{

    var result = await endPoints.DeleteProcessedParcelsAndRoutesAsync();

    if (result == false)
        return Results.BadRequest();

    return Results.Ok(result);
});

app.Run();


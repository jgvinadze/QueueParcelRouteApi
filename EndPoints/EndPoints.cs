using Microsoft.AspNetCore.Mvc;
using QueueParcelRouteApi.Infrastructure;
using System.Runtime.CompilerServices;

namespace EndPoints
{
    public static class EndPoints
    {
        public static void AddEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/ParcelRoutes/InsParcelRoutesInMariaDbAsync", async ([FromServices] RoutesEndpoints endPoints, CancellationToken ct) => {

                var result = await endPoints.InsParcelRoutesInMariaDbAsync(ct);

                if (result == null)
                    return Results.BadRequest();

                return Results.Ok(result);
            });

            app.MapPost("/ParcelRoutes/DeleteProcessedParcelsAndRoutesAsync", async ([FromServices] RoutesEndpoints endPoints, CancellationToken ct) =>
            {

                var result = await endPoints.DeleteProcessedParcelsAndRoutesAsync(ct);

                if (result == false)
                    return Results.BadRequest();

                return Results.Ok(result);
            });
        }
    }
}

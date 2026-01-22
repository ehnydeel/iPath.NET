using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace iPath.API;

public static class FhirEndpoints
{
    public static IEndpointRouteBuilder MapFhirApi(this IEndpointRouteBuilder route)
    {
        var fhir = route.MapGroup("fhir")
                .WithTags("Fhir");

        fhir.MapGet("{resource}/{id}", async (string resource, string id, IOptions<iPathConfig> opts) =>
        {
            var dir = opts.Value.FhirResourceFilePath;
            if (System.IO.Directory.Exists(dir))
            {
                var filename = System.IO.Path.Combine(dir, resource);
                filename = System.IO.Path.Combine(filename, id) + ".json";
                if (System.IO.File.Exists(filename))
                { 
                    return Results.File(filename, contentType: "text/json");
                }
            }

            return Results.NotFound();
        });

        return route;
    }
}


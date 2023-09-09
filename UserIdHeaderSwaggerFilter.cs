using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sharding_DI_Sample;

public class UserIdHeaderSwaggerFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();
        
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-UserId",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema{ Type = "string" }
        });
    }
}
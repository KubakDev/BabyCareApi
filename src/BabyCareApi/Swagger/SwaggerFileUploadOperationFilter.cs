using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BabyCareApi.Swagger;

public class SwaggerFileUploadOperationFilter : IOperationFilter
{
  private const string MULTIPART_FORMDATA = "multipart/form-data";

  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    if (context.ApiDescription?.HttpMethod != HttpMethods.Post)
      return;

    var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();

    if (fileParameters.Count == 0)
      return;

    // Remove all but multipart/form-data
    if (!operation.RequestBody.Content.TryGetValue(MULTIPART_FORMDATA, out var multiPartMediaType))
      return;

    operation.RequestBody.Content.Clear();
    operation.RequestBody.Content.Add(MULTIPART_FORMDATA, multiPartMediaType);

    var properties = multiPartMediaType.Schema.Properties;
    var fileSchema = new OpenApiSchema()
    {
      Type = "string",
      Format = "binary"
    };

    var ignoredProperties = typeof(IFormFile).GetProperties().Select(doc => doc.Name);
    foreach (var ignoredProperty in ignoredProperties)
      properties.Remove(ignoredProperty);

    foreach (var fileParameter in fileParameters)
    {
      properties.Remove(fileParameter.Name);
      properties.Add(fileParameter.Name, fileSchema);
    }
  }
}
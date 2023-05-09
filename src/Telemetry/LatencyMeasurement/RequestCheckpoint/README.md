# Request Checkpoint

This sample shows you how to use the _Request checkpoint middleware_ to measure and log the time spent running individual steps while processing incoming requests within the ASP.NET Core pipeline.

Within the sample, the middleware is configured and registered in Startup.cs by calling AddRequestCheckpoint(IServiceCollection) followed by UseRequestCheckpoint(IApplicationBuilder).

After the middleware is registered, the captured time samples are automatically logged. Additionally, the samples are added to the Server-Timing response header.

### Request checkpoint middleware

Request checkpoint middleware exposes many ASP.NET Core middlewares that compute and log the time spent processing incoming requests within the ASP.NET Core pipeline.

Here are the properties associated with request checkpoint middleware:

| Property              | Description                                                                 |
| --------------------- | --------------------------------------------------------------------- |
| `elthdr`             | Time spent until response headers are sent to the client |
| `eltltf`             | Time spent until the response is sent to the client |
| `eltrspproc`             | Time spent inside middleware pipeline to process the request and the response |
| `eltexm` and `eltenm`             | Time elapsed until the request exits the middleware pipeline and the response enters the middleware pipeline |

> :information_source: NOTE<br/>
> The time spent is calculated starting from the moment the stopwatch is created, that is, at the beginning of the middleware pipeline.

This example shows how to use the request checkpoint middleware:

```csharp
new HostBuilder()
    .ConfigureWebHost(webBuilder =>
    {
        webBuilder
            .ConfigureServices(services =>
            {
                services.AddRequestCheckpoint();    // Add all middlewares to the service collection
            })
            .Configure(app =>
            {
                ...
                app.UseRequestCheckpoint();         // Can be put anywhere inside pipeline
                ...
                app.UseEndpoints(...);
            });
    });
```

### Package(s)

Add package reference(s) to your project files in order to use the above features:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Telemetry.Middleware" Version="*" />
</ItemGroup>
```

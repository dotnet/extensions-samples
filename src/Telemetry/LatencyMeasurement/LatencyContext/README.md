# Latency context sample

This sample shows you how to use Latency context to measure the latency of an operation.

### Latency context initialization

To initialize latency context, run the following within `Startup.cs`:

```cs
public void Configure(IApplicationBuilder app)
{
    app.ApplicationServices.RegisterTagNames(ServiceTagNames.AllTags);
    app.ApplicationServices.RegisterCheckpointNames(ServiceCheckpointNames.AllCheckpoints);
    app.ApplicationServices.RegisterMeasureNames(ServiceMeasureNames.AllMeasures);
    app.ApplicationServices.AddLatencyContext();
}
```

### Latency context usage

`ExampleService.cs`

The following shows usage within a web middleware as an example. However, this implementation isn't limited to only web middleware and can be use outside it as well.

```cs
// Business logic middleware
public Task InvokeAsync(HttpContext context, ILatencyContext latencyContext)
{
    latencyContext.AddCheckpoint(ServiceCheckpointNames.StartServiceCall);
    latencyContextControl.Context.SetTag(ServiceTagNames.Operation, context.GetOperation());

    for(int i = 0; i < 5; i++ )
    {
        var ret = MakeServiceCall();
        latencyContext.AccumulateMeasure(ServiceMeasureNames.ServiceCalls, 1);
        latencyContext.AccumulateMeasure(ServiceMeasureNames.NumItems, ret.NumItems);
        latencyContext.SetMeasure(ServiceMeasureNames.LastNumItems, ret.NumItems);
    }

    latencyContext.AddCheckpoint(ServiceCheckpointNames.StopServiceCall);
}
```

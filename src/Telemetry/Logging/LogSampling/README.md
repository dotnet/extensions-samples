# Log Sampling

This sample shows how to use
[log sampling](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.Telemetry/README.md).
Log Sampling allows logs to be sampled, e.g. only some share of all log messages will be emitted.

The sample uses a typical `HostApplicationBuilder` pattern to configure a small console application with
logging. Log sampling is enabled by calling `.AddRandomProbabilisticSampler()` on the logging builder
and
providing a configuration via `appsettings.json`.

The configuration in `appsettings.json` is flexible - you can configure specific sampling rates per
any combination of
- log level
- category name
- event id

And, importantly, the configuration supports dynamic runtime updates via the `IOptionsMonitor<T>` pattern.
So you can change the `appsettings.json` (in the `/artifacts/bin/LogSampling/Debug` folder) at runtime
and the changes will be picked up by the Log sampling.

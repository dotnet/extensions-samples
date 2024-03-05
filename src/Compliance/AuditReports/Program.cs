// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

const string ReportLocation = "./";
const string ComplianceReportFileName = "ComplianceReport.json";

Console.WriteLine("Starting AuditReports with arguments: " + string.Join(";", args));

CheckReport(ComplianceReportFileName);

static void CheckReport(string fileName)
{
    var reportsLocation = Path.GetFullPath(Path.Combine(ReportLocation, fileName));
    if (File.Exists(reportsLocation))
    {
        Console.WriteLine("[Success] Report generated in: " + reportsLocation);
    }
    else
    {
        Console.Error.WriteLine("[Error] No report generated in: " + reportsLocation);
    }
}

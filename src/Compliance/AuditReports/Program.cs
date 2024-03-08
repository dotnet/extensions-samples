// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

// If you changed the "<ComplianceReportOutputPath>" property in .csproj file,
// you also need to update the value below:
const string ReportLocation = "./";
const string ComplianceReportFileName = "ComplianceReport.json";

Console.WriteLine("Starting AuditReports with arguments: " + string.Join(";", args));

CheckReport(ComplianceReportFileName);

static void CheckReport(string fileName)
{
    var reportsLocation = Path.GetFullPath(Path.Combine(ReportLocation, fileName));
    if (File.Exists(reportsLocation))
    {
        Console.WriteLine("[Success] Compliance report is generated in: " + reportsLocation);
        Console.WriteLine("Its content is listed below:");
        Console.WriteLine("{0}", File.ReadAllText(reportsLocation));
    }
    else
    {
        Console.Error.WriteLine("[Error] No report generated in: " + reportsLocation);
    }
}

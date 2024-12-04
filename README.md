![Nuget](https://img.shields.io/nuget/v/Unicorn.Reporting.ReportPortal?style=plastic) ![Nuget](https://img.shields.io/nuget/dt/Unicorn.Reporting.ReportPortal?style=plastic)

# Unicorn.Reporting.ReportPortal

Unicorn has ability to generate powerful test results report using [Report Portal](https://reportportal.io)

Just deploy ReportPortal instance, add tests project dependency to [Unicorn.Reporting.ReportPortal](https://www.nuget.org/packages/Unicorn.Reporting.ReportPortal) package and initialize reporter during tests assembly initialization.
***
Place **ReportPortal.config.json** configuration file to directory with test assemblies. Sample content is presented below:
```json
{
    "enabled": true,
    "server": {
        "url": "https://report-portal-uri/api/v1/",
        "project": "Some project",
        "authentication": {
        "uuid": "your_uuid"
        },
    },
    "launch": {
        "name": "Unicorn tests run",
        "description": "Unit tests of Unicorn Framework",
        "debugMode": false,
        "tags": [ "Windows 10", "UnicornFramework" ]
    }
}  
```

then add code with reporting initialization to `[TestsAssembly]`
```csharp
using Unicorn.Taf.Api;
using Unicorn.Taf.Core.Testing.Tests.Attributes;
using Unicorn.Reporting.ReportPortal;

namespace Tests
{
    [TestsAssembly]
    public static class TestsAssembly
    {
        private static ITestReporter reporter;

        [RunInitialize]
        public static void InitRun()
        {
            reporter = new ReportPortalReporter(); // Start new launch in Report Portal.
            
            /* in case you want to report into already started existing launch use
             * reporter = new ReportPortalReporter(existing_launch_id); */
            
        }

        [RunFinalize]
        public static void FinalizeRun()
        {
            reporter.Dispose(); // Finish launch in Report portal if it was not externally started.
            reporter = null;
        }
    }
}
```

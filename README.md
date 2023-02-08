![Nuget](https://img.shields.io/nuget/v/Unicorn.ReportPortalAgent?style=plastic)
![Nuget](https://img.shields.io/nuget/dt/Unicorn.ReportPortalAgent?style=plastic)

# ReportPortal agent

Unicorn has ability to generate powerful test results report using [EPAM Report Portal](https://reportportal.io)

Just deploy EPAM ReportPortal instance, add tests project dependency to [Unicorn.ReportPortalAgent](https://www.nuget.org/packages/Unicorn.ReportPortalAgent) package and initialize reporter during tests assembly initialization.
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
using Unicorn.Core.Testing.Tests.Attributes;
using Unicorn.ReportPortalAgent;

namespace Tests
{
    [TestsAssembly]
    public static class TestsAssembly
    {
        private static ReportPortalReporterInstance rpInstance;

        [RunInitialize]
        public static void InitRun()
        {
            rpInstance = new ReportPortalReporterInstance(); // Start new launch in Report Portal.
            
            /* in case you want to report into already started existing launch use
             * rpInstance = new ReportPortalReporterInstance(existing_launch_id); */
            
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

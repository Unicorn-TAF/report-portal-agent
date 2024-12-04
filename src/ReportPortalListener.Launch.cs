using System;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Reporter;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Reporting.ReportPortal
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private ILaunchReporter launchReporter;
        
        internal void StartRun()
        {
            try
            {
                LaunchMode launchMode = Config.GetValue(ConfigurationPath.LaunchDebugMode, false) ?
                    LaunchMode.Debug :
                    LaunchMode.Default;

                var attributes = Config
                    .GetKeyValues("Launch:Attributes", new List<KeyValuePair<string, string>>())
                    .Select(a => new ItemAttribute { Key = a.Key, Value = a.Value });

                var startLaunchRequest = new StartLaunchRequest
                {
                    Name = Config.GetValue(ConfigurationPath.LaunchName, "Unicorn tests launch"),
                    Description = Config.GetValue(ConfigurationPath.LaunchDescription, string.Empty),
                    StartTime = DateTime.UtcNow,
                    Mode = launchMode,
                    Attributes = attributes.ToList()
                };

                launchReporter = new LaunchReporter(_rpService, Config, null, _extensionManager);
                launchReporter.Start(startLaunchRequest);
            }
            catch (Exception exception)
            {
                ULog.Warn(Prefix + BaseMessage + Environment.NewLine + exception);
            }
        }

        internal void FinishRun()
        {
            try
            {
                var finishLaunchRequest = new FinishLaunchRequest
                {
                    EndTime = DateTime.UtcNow,
                };

                launchReporter.Finish(finishLaunchRequest);
                launchReporter.Sync();
            }
            catch (Exception exception)
            {
                ULog.Warn(Prefix + BaseMessage + Environment.NewLine + exception);
            }
        }
    }
}

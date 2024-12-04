using ReportPortal.Client.Abstractions.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Testing;
using ULogging = Unicorn.Taf.Core.Logging;

namespace Unicorn.Reporting.ReportPortal
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private readonly Dictionary<ULogging.LogLevel, LogLevel> _logLevels =
            new Dictionary<ULogging.LogLevel, LogLevel>
        {
            { ULogging.LogLevel.Error, LogLevel.Error },
            { ULogging.LogLevel.Warning, LogLevel.Warning },
            { ULogging.LogLevel.Info, LogLevel.Info },
            { ULogging.LogLevel.Debug, LogLevel.Debug },
            { ULogging.LogLevel.Trace, LogLevel.Trace },
        };

        private readonly ConcurrentDictionary<Guid, SuiteMethod> _currentTests = 
            new ConcurrentDictionary<Guid, SuiteMethod>();

        internal void ReportTestMessage(ULogging.LogLevel level, string info)
        {
            var stackTrace = new StackTrace();
            var currentTest = _currentTests.Values.First(t => stackTrace.GetFrames()
                .Any(sf => sf.GetMethod().Name.Contains(t.TestMethod.Name)));

            if (currentTest != null)
            {
                AddLog(currentTest.Outcome.Id, _logLevels[level], info);
            }
        }

        /// <summary>
        /// Reports logs to current executing suite method.
        /// </summary>
        /// <param name="method">method itself</param>
        /// <param name="arguments">method arguments</param>
        internal void ReportStepInfo(MethodBase method, object[] arguments) =>
            ReportTestMessage(
                ULogging.LogLevel.Info,
                StepsUtilities.GetStepInfo(method, arguments));
    }
}

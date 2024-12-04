using Unicorn.Taf.Api;
using Unicorn.Taf.Core;

namespace Unicorn.Reporting.ReportPortal
{
    /// <summary>
    /// Report portal reporter instance. Contains subscriptions to corresponding Unicorn events.
    /// </summary>
    public sealed class ReportPortalReporter : ITestReporter
    {
        private readonly ReportPortalListener _listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPortalReporter"/> class.<br/>
        /// New RP launch is started automatically with automatic subscription to all test events.
        /// </summary>
        public ReportPortalReporter() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPortalReporter"/> class 
        /// based on existing launch ID and with automatic subscription to all test events.<br/>
        /// If ID is null, then starts new launch on RP.
        /// </summary>
        /// <param name="existingLaunchId">existing launch ID</param>
        public ReportPortalReporter(string existingLaunchId)
        {
            _listener = new ReportPortalListener();

            if (!string.IsNullOrEmpty(existingLaunchId))
            {
                _listener.ExistingLaunchId = existingLaunchId;
            }

            _listener.StartRun();

            TafEvents.OnTestStart += _listener.StartSuiteMethod;
            TafEvents.OnTestFinish += _listener.FinishSuiteMethod;
            TafEvents.OnTestSkip += _listener.SkipSuiteMethod;

            TafEvents.OnSuiteMethodStart += _listener.StartSuiteMethod;
            TafEvents.OnSuiteMethodFinish += _listener.FinishSuiteMethod;

            TafEvents.OnSuiteStart += _listener.StartSuite;
            TafEvents.OnSuiteFinish += _listener.FinishSuite;

            TafEvents.OnStepStart += _listener.ReportStepInfo;
        }

        /// <summary>
        /// Sets defect type to set for skipped tests in report portal.
        /// </summary>
        /// <param name="defectType">report portal defect type ID</param>
        public void SetSkippedTestsDefectType(string defectType) =>
            _listener.SkippedTestDefectType = defectType;

        /// <summary>
        /// Unsubscribes from events and finishes launch if it is not external.
        /// </summary>
        public void Dispose()
        {
            if (_listener != null)
            {
                // Need to finish it even if the launch is external!
                _listener.FinishRun();

                TafEvents.OnTestStart -= _listener.StartSuiteMethod;
                TafEvents.OnTestFinish -= _listener.FinishSuiteMethod;
                TafEvents.OnTestSkip -= _listener.SkipSuiteMethod;

                TafEvents.OnSuiteMethodStart -= _listener.StartSuiteMethod;
                TafEvents.OnSuiteMethodFinish -= _listener.FinishSuiteMethod;

                TafEvents.OnSuiteStart -= _listener.StartSuite;
                TafEvents.OnSuiteFinish -= _listener.FinishSuite;

                TafEvents.OnStepStart -= _listener.ReportStepInfo;
            }
        }


    }
}

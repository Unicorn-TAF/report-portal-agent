using System;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;

namespace Unicorn.Reporting.ReportPortal
{
    /// <summary>
    /// Report portal listener, which handles reporting stuff for all test items.
    /// </summary>
    public partial class ReportPortalListener
    {
        private void AddAttachment(Guid id, LogLevel level, string text, string attachmentName, string mime, byte[] content)
        {
            try
            {
                var request = new CreateLogItemRequest
                {
                    Level = level,
                    Time = DateTime.UtcNow,
                    Text = text,

                    Attach = new LogItemAttach(mime, content)
                    {
                        Name = attachmentName
                    }
                };

                _testFlowIds[id].Log(request);
            }
            catch (Exception exception)
            {
                Console.WriteLine(Prefix + BaseMessage + Environment.NewLine + exception);
            }
        }

        private void AddLog(Guid id, LogLevel level, string text)
        {
            try
            {
                var request = new CreateLogItemRequest
                {
                    Level = level,
                    Time = DateTime.UtcNow,
                    Text = text,
                };

                _testFlowIds[id].Log(request);
            }
            catch (Exception exception)
            {
                Console.WriteLine(Prefix + BaseMessage + Environment.NewLine + exception);
            }
        }
    }
}

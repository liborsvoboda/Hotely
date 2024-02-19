using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;

namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// HeathCheck Handler For Sending Information About Fails
    /// </summary>
    public class DelegateHealthCheckPublisher : IHealthCheckPublisher {
        private HealthReport? _prevStatus = null;

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken) {
            if ((report.Status == HealthStatus.Degraded || report.Status == HealthStatus.Unhealthy) &&
            ServerConfigSettings.ServiceCoreCheckerEmailSenderActive &&
            ((ServerConfigSettings.ModuleHealthServiceMessageOnChangeOnly && _prevStatus?.Status == HealthStatus.Healthy) || !ServerConfigSettings.ModuleHealthServiceMessageOnChangeOnly)) {
                string message = "";
                report.Entries.ToList().ForEach(monit => {
                    if (monit.Value.Status == HealthStatus.Degraded || monit.Value.Status == HealthStatus.Unhealthy) {
                        message += monit.Key + " status: " + monit.Value.Status.ToString() + Environment.NewLine + monit.Value.Description + Environment.NewLine;
                    }
                });
                CoreOperations.SendEmail(new MailRequest() { Content = message });
            }
            _prevStatus = report;
            return Task.CompletedTask;
        }
    }
}
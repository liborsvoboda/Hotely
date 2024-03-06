using Quartz;
using Quartz.Spi;
using System.Linq;
using System.Threading;

namespace UbytkacBackend.ServerCoreStructure {


    public class AutoSchedulerService : IHostedService {
        private readonly ILogger _logger;

        public AutoSchedulerService(ILogger<AutoSchedulerService> logger) {
            _logger = logger;
            //ServerRuntimeData.ServerAutoSchedulerProvider = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            await ServerRuntimeData.ServerAutoSchedulerProvider.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken) {
            await ServerRuntimeData.ServerAutoSchedulerProvider.Shutdown(cancellationToken);
        }

        public async Task PauseAll(CancellationToken cancellationToken) {
            await ServerRuntimeData.ServerAutoSchedulerProvider.PauseAll(cancellationToken);
        }

        public async Task ResumeAll(CancellationToken cancellationToken) {
            await ServerRuntimeData.ServerAutoSchedulerProvider.ResumeAll(cancellationToken);
        }
        public async Task<bool> IsPaused(CancellationToken cancellationToken) {
           return await ServerRuntimeData.ServerAutoSchedulerProvider.IsTriggerGroupPaused("AutoScheduler",cancellationToken);
        }
        public bool IsShutdown() { return ServerRuntimeData.ServerAutoSchedulerProvider.IsShutdown; }
    }


    public class JobFactory : IJobFactory {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
            var job = _serviceProvider.GetService(bundle.JobDetail.JobType) as AutoScheduledJob;
            return job;
        }

        public void ReturnJob(IJob job) {
        }
    }


    /// <summary>
    /// Server AutoSchedule Planner
    /// </summary>
    public static class ServerCoreAutoScheduler {

        internal async static Task<bool> AutoSchedulerPlanner(int? rescheduleTaskId = null, bool deleteOnly = false) {
            IScheduler? scheduler = ServerRuntimeData.ServerAutoSchedulerProvider; List<SolutionSchedulerList>? data = null;

            if (rescheduleTaskId == null) { data = new hotelsContext().SolutionSchedulerLists.Where(a => a.Active).Include(a => a.User).ThenInclude(a => a.Role).ToList(); }
            else if (rescheduleTaskId != null && deleteOnly) { JobKey remoteScheduledTask = new JobKey(rescheduleTaskId.ToString(), "AutoScheduler"); await scheduler.DeleteJob(remoteScheduledTask); }
            else {
                data = new hotelsContext().SolutionSchedulerLists.Where(a => a.Active && a.Id == rescheduleTaskId).Include(a => a.User).ThenInclude(a => a.Role).ToList();
                JobKey remoteScheduledTask = new JobKey(rescheduleTaskId.ToString(), "AutoScheduler"); await scheduler.DeleteJob(remoteScheduledTask);
            }
            if (data != null && !deleteOnly) {
                data.ForEach(async scheduledTask => {

                    JobDataMap data = new() { { "id", scheduledTask.Id }, { "name", scheduledTask.Name }, { "type", scheduledTask.InheritedGroupName },{ "email", scheduledTask.Email }, { "data", scheduledTask.Data },
                        { "runonetime", scheduledTask.StartNowOnly },{ "startat", scheduledTask.StartAt },{ "finishat", scheduledTask.FinishAt },{ "interval", scheduledTask.Interval },{ "intervaltype", scheduledTask.InheritedIntervalType },
                        { "sequence", scheduledTask.Sequence },{ "userrole", scheduledTask.User.Role.SystemName },{ "username", scheduledTask.User.Name + " " + scheduledTask.User.SurName } };

                    IJobDetail job = JobBuilder.Create<AutoScheduledJob>().WithIdentity(scheduledTask.Id.ToString(), "AutoScheduler").UsingJobData(data).StoreDurably(true).RequestRecovery().Build();

                    ITrigger trigger = null;
                    if (scheduledTask.StartNowOnly) {
                        trigger = TriggerBuilder.Create().ForJob(job).WithIdentity("TR_" + scheduledTask.Name, "AutoScheduler").WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithRepeatCount(0)).StartNow().WithPriority(scheduledTask.Sequence).Build();
                    }
                    else {
                        if (scheduledTask.InheritedIntervalType == "second") {
                            trigger = TriggerBuilder.Create().ForJob(job).WithIdentity("TR_" + scheduledTask.Name, "AutoScheduler").WithSimpleSchedule(x => x.WithIntervalInSeconds(scheduledTask.Interval).RepeatForever()).StartAt(scheduledTask.StartAt.Value).WithPriority(scheduledTask.Sequence).Build();
                        }
                        else if (scheduledTask.InheritedIntervalType == "minute") {
                            trigger = TriggerBuilder.Create().ForJob(job).WithIdentity("TR_" + scheduledTask.Name, "AutoScheduler").WithSimpleSchedule(x => x.WithIntervalInMinutes(scheduledTask.Interval).RepeatForever()).StartAt(scheduledTask.StartAt.Value).WithPriority(scheduledTask.Sequence).Build();
                        }
                        else if (scheduledTask.InheritedIntervalType == "hour") {
                            trigger = TriggerBuilder.Create().ForJob(job).WithIdentity("TR_" + scheduledTask.Name, "AutoScheduler").WithSimpleSchedule(x => x.WithIntervalInHours(scheduledTask.Interval).RepeatForever()).StartAt(scheduledTask.StartAt.Value).WithPriority(scheduledTask.Sequence).Build();
                        }
                        else if (scheduledTask.InheritedIntervalType == "day") {
                            trigger = TriggerBuilder.Create().ForJob(job).WithIdentity("TR_" + scheduledTask.Name, "AutoScheduler").WithSimpleSchedule(x => x.WithIntervalInHours(24 * scheduledTask.Interval).RepeatForever()).StartAt(scheduledTask.StartAt.Value).WithPriority(scheduledTask.Sequence).Build();
                        }
                    }

                    await scheduler.ScheduleJob(job, trigger, default);
                });
            }
            return true;
        }
    }

    /// <summary>
    /// Autoscheduler Process Scheduled Task 
    /// definitions 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public class AutoScheduledJob : IJob {

        public Task Execute(IJobExecutionContext context) {
            SolutionSchedulerProcessList taskResult = new SolutionSchedulerProcessList();

            try {
                var jobData = context.JobDetail.JobDataMap;
                string? data = (!string.IsNullOrWhiteSpace(jobData?.First(a => a.Key.ToLower() == "data").Value?.ToString())) ? jobData.First(a => a.Key.ToLower() == "data").Value.ToString() : null;
                if (!string.IsNullOrWhiteSpace(data)) {

                    string? jobType = jobData?.First(a => a.Key.ToLower() == "type").Value?.ToString()?.ToLower();
                    taskResult.ScheduledTaskId = int.Parse(jobData.First(a => a.Key.ToLower() == "id").Value.ToString());
                    taskResult.ProcessData = JsonSerializer.Serialize(jobData); taskResult.ProcessCrashed = false;
                    bool runOneTime = bool.Parse(jobData.First(a => a.Key.ToLower() == "runonetime").Value.ToString());
                    DateTime? finistAt = runOneTime ? null: DateTime.Parse(jobData.First(a => a.Key.ToLower() == "finishat").Value.ToString());

                    if (jobType == "email") {
                        try {
                            IDictionary<string, string> maildata = JsonSerializer.Deserialize<IDictionary<string, string>>(data);
                            CoreOperations.SendEmail(new MailRequest() {
                                Sender = ServerConfigSettings.ConfigManagerEmailAddress,
                                Recipients = jobData.First(a => a.Key.ToLower() == "email").Value?.ToString()?.Split(";").ToList(),
                                Subject = maildata?.First(a => a.Key.ToLower() == "subject").Value.ToString(),
                                Content = maildata?.First(a => a.Key.ToLower() == "content").Value.ToString()
                            },true);
                        } catch (Exception ex) { taskResult.ProcessCrashed = true; taskResult.ProcessLog = DataOperations.GetSystemErrMessage(ex); CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }
                    }
                    else if (jobType == "sqlemail") {
                        try {
                            if (jobData.First(a => a.Key.ToLower() == "userrole").Value?.ToString() == "admin") {
                                IDictionary<string, string> sqlmaildata = JsonSerializer.Deserialize<IDictionary<string, string>>(data);
                                CoreOperations.SendEmail(new MailRequest() {
                                    Sender = ServerConfigSettings.ConfigManagerEmailAddress,
                                    Recipients = jobData.First(a => a.Key.ToLower() == "email").Value?.ToString()?.Split(";").ToList(),
                                    Subject = sqlmaildata?.First(a => a.Key.ToLower() == "subject").Value.ToString(),
                                    Content = sqlmaildata?.First(a => a.Key.ToLower() == "content").Value.ToString()
                                }, true);
                            }
                        } catch (Exception ex) { taskResult.ProcessCrashed = true; taskResult.ProcessLog = DataOperations.GetSystemErrMessage(ex); CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }

                    }
                    else if (jobType == "command") {
                        try {
                            if (jobData.First(a => a.Key.ToLower() == "userrole").Value?.ToString() == "admin") {
                                ProcessClass? process = new ProcessClass() { Command = System.IO.Path.GetFullPath(data), Arguments = "", WorkingDirectory = System.IO.Path.GetFullPath(data.Replace(System.IO.Path.GetFileName(data), "")) };
                                CoreOperations.RunSystemProcess(process);
                            }
                        } catch (Exception ex) { taskResult.ProcessCrashed = true; taskResult.ProcessLog = DataOperations.GetSystemErrMessage(ex); CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }

                    }
                    else if (jobType == "sqlquery") {
                        try {
                            if (jobData.First(a => a.Key.ToLower() == "userrole").Value?.ToString() == "admin") {
                                _ = new hotelsContext().UbytkacBackendCollectionFromSql<CustomString>($"EXEC {data};");
                            }

                        } catch (Exception ex) { taskResult.ProcessCrashed = true; taskResult.ProcessLog = DataOperations.GetSystemErrMessage(ex); CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }

                    }
                    else if (jobType == "websocketnotify") {
                        try {
                            ServerCoreWebHelpers.SendMessageAndUpdateWebSocketsInSpecificPath(ServerConfigSettings.WebSocketGlobalNotifyPath, data);

                        } catch (Exception ex) { taskResult.ProcessCrashed = true; taskResult.ProcessLog = DataOperations.GetSystemErrMessage(ex); CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }
                    }
                    else if (jobType == "sqlwebsocketnotify") {
                        try {
                            if (jobData.First(a => a.Key.ToLower() == "userrole").Value?.ToString() == "admin") {
                                string? result = new hotelsContext().UbytkacBackendCollectionFromSql<CustomString>($"EXEC {data};").ToString();
                                ServerCoreWebHelpers.SendMessageAndUpdateWebSocketsInSpecificPath(ServerConfigSettings.WebSocketGlobalNotifyPath, result);
                            }

                        } catch (Exception ex) { taskResult.ProcessCrashed = true; taskResult.ProcessLog = DataOperations.GetSystemErrMessage(ex); CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(ex) }); }
                    } else {  CoreOperations.SendEmail(new MailRequest() { Content = "ThisScheduledTaskNotImplemented " + string.Join(",",jobData.KeySet()) }); }

                    //Log Process Result
                    taskResult.ProcessCompleted = true; taskResult.TimeStamp = DateTimeOffset.Now.DateTime;
                    EntityEntry<SolutionSchedulerProcessList>? dbdata = new hotelsContext().SolutionSchedulerProcessLists.Add(taskResult); dbdata.Context.SaveChanges();

                    //Deactivate Task in Database
                    if (runOneTime || (finistAt != null && finistAt < DateTimeOffset.Now)) {
                        var recUpdate = new hotelsContext().SolutionSchedulerLists.Where(a => a.Id == taskResult.ScheduledTaskId).FirstOrDefault();
                        if (recUpdate != null) { recUpdate.Active = false; var dbChange = new hotelsContext().SolutionSchedulerLists.Update(recUpdate); dbChange.Context.SaveChanges(); }

                        IScheduler? scheduler = ServerRuntimeData.ServerAutoSchedulerProvider;
                        JobKey remoteScheduledTask = new JobKey(taskResult.ScheduledTaskId.ToString(), "AutoScheduler"); scheduler.DeleteJob(remoteScheduledTask).GetAwaiter().GetResult();
                    }

                }
                else {return Task.FromResult("DataNotFound"); }
            } catch (Exception Ex) {
                CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) });
                return Task.FromException(Ex);
            }
            return Task.FromResult(true);
        }
    }
    
}
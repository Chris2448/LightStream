using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");

            try
            {
                RunAsync(cancellationTokenSource.Token).Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            var result = base.OnStart();

            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var dbSettings = new DbSettings
            {
                connectionstring = CloudConfigurationManager.GetSetting("dbconnectionstring"),
                database = CloudConfigurationManager.GetSetting("dbname"),
                collections = new collections
                {
                    People = "People",
                    Configurations = "Configurations",
                    Counters = "Counters",
                    Programs = "Programs",
                    Suggestions = "Suggestions"
                }
            };

            var filler = new TableFiller(dbSettings);

            while (!cancellationToken.IsCancellationRequested)
            {
                // Do the work you wanna do

                Trace.TraceInformation("People: [Started Filling Table]");
                filler.FillPeople();
                Trace.TraceInformation("People: [Finished Filling Table]");

                Trace.TraceInformation("Suggestions: [Started Filling Table]");
                filler.FillSuggestions();
                Trace.TraceInformation("Suggestions: [Finished Filling Table]");

                await Task.Delay(10000);
            }
        }
    }
}

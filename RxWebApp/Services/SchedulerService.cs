using System.Reactive.Concurrency;

namespace RxWebApp.Services
{
    internal class SchedulerService : ISchedulerService
    {
        public IScheduler CurrentThread
        {
            get { return Scheduler.CurrentThread; }
        }

        public IScheduler Immediate
        {
            get { return Scheduler.Immediate; }
        }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler Pool
        {
            get { return Scheduler.Default; }
        }
    }
}
using System.Reactive.Concurrency;

namespace RxWebApp.Services
{
    public interface ISchedulerService
    {
        IScheduler CurrentThread { get; }
        IScheduler Immediate { get; }
        IScheduler NewThread { get; }
        IScheduler Pool { get; }
    }
}
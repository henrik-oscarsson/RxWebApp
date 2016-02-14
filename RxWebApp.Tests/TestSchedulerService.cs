using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using RxWebApp.Services;

namespace RxWebApp.Tests
{
    public class TestSchedulerService : ISchedulerService
    {
        private readonly TestScheduler _currentThread = new TestScheduler();
        private readonly TestScheduler _immediate = new TestScheduler();
        private readonly TestScheduler _newThread = new TestScheduler();
        private readonly TestScheduler _pool = new TestScheduler();

        IScheduler ISchedulerService.CurrentThread { get { return _currentThread; } }
        IScheduler ISchedulerService.Immediate { get { return _immediate; } }
        IScheduler ISchedulerService.NewThread { get { return _newThread; } }
        IScheduler ISchedulerService.Pool { get { return _pool; } }

        public TestScheduler CurrentThread
        {
            get { return _currentThread; }
        }

        public TestScheduler Immediate
        {
            get { return _immediate; }
        }

        public TestScheduler NewThread
        {
            get { return _newThread; }
        }

        public TestScheduler Pool
        {
            get { return _pool; }
        }
    }
}
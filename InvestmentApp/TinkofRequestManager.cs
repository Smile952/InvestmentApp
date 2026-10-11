namespace InvestmentApp
{
    public class TinkofRequestManager
    {
        delegate void TaskAppend(Func<Task> task);
        public int MaxTaskCount { get; set; }
        public int Delay { get; set; }
        public Queue<Func<Task>> requests { get; set; } = new Queue<Func<Task>>();
        public TinkofRequestManager(int maxTaskCount, int delay)
        {
            MaxTaskCount = maxTaskCount;
            Delay = delay;
        }
        public async Task RunRequest()
        {
            await requests.Dequeue().Invoke();
            await Task.Delay((int)(Delay/MaxTaskCount));
        }
        public async Task AppendRequestAsync(Func<Task> request)
        {
            requests.Enqueue(request);
            await RunRequest();
        }
    }
}

using System.Collections.Concurrent;
using PuppeteerSharp;
using WebScrappingDemo.Domain.Enums;
using WebScrappingDemo.Domain.PuppeteerModels.OutageModels;

namespace WebScrappingDemo.Services;

public class BrowserSessionStorage : IDisposable
{
    //private UnhandledExceptionEventHandler UnhandledExceptionEvent { get; }

    private EventHandler ProcessExitHandler { get; }

    private static ConcurrentDictionary<string, BrowserSession> _sessions = new ConcurrentDictionary<string, BrowserSession>();

    public BrowserSessionStorage()
    {
        WeakReference<IDisposable> weakReferenceToSelf =
         new(this);
        ProcessExitHandler = (_, __) =>
        {
            Console.WriteLine("Starting...", "ProcessExitHandler");
            if (weakReferenceToSelf.TryGetTarget(
                out IDisposable? self))
            {
                self.Dispose();
            }
            Console.WriteLine("Exiting...", "ProcessExitHandler");
        };
        AppDomain.CurrentDomain.ProcessExit
            += ProcessExitHandler;
    }

    public ConcurrentDictionary<string, BrowserSession> Sessions { get { return _sessions; } }

    public void Dispose()
    {
        FlushBrowserSessions();
        AppDomain.CurrentDomain.ProcessExit -=
            ProcessExitHandler;
        GC.Collect();
    }

    private static void FlushBrowserSessions()
    {
        var sessions = _sessions;

        foreach (var record in sessions)
        {
            sessions.TryRemove(record.Key, out var session);
            session.Dispose();
        }
    }
}

public class BrowserSession : IDisposable, IAsyncDisposable
{
    public string SessionId { get; set; }

    public OutageInputStep CurrentInputStep { get; set; } = OutageInputStep.Step_0;

    public IBrowser Browser { get; set; }

    public IPage Page { get; set; }

    public List<DropdownOption> DropdownOptions { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public void ExtendSessionTime()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void Dispose()
    {
        if (Browser is not null)
        {
            Browser.CloseAsync().Wait();
            Browser.Dispose();
        }

        GC.Collect();
    }

    public async ValueTask DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.CloseAsync();
            await Browser.DisposeAsync();
        }

        GC.Collect();
    }
}

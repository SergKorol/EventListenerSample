using System.Diagnostics.Tracing;

namespace EventListenerSample;

public static class Program
{
    public static async Task Main()
    {
        using var eventListener = new HttpEventListener();
        var client = new HttpClient();
        await client.GetStringAsync("https://catfact.ninja/fact");
    }
}

sealed class HttpEventListener : EventListener
{
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        switch (eventSource.Name)
        {
            case "System.Net.Http":
                EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All);
                break;
            case "System.Threading.Tasks.TplEventSource":
                const EventKeywords tasksFlowActivityIds = (EventKeywords)0x80;
                EnableEvents(eventSource, EventLevel.LogAlways, tasksFlowActivityIds);
                break;
            case "Microsoft-Windows-DotNETRuntime":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Runtime":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "Private.InternalDiagnostics.System.Net.Http":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Buffers.ArrayPoolEventSource":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Diagnostics.Eventing.FrameworkEventSource":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "Microsoft-Diagnostics-DiagnosticSource":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "Private.InternalDiagnostics.System.Net.Sockets":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Net.Sockets":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Net.NameResolution":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "Private.InternalDiagnostics.System.Net.Security":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Net.Security":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "System.Collections.Concurrent.ConcurrentCollectionsEventSource":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
            case "Private.InternalDiagnostics.System.Net.Primitives":
                EnableEvents(eventSource, EventLevel.Informational, EventKeywords.All);
                break;
        }

        base.OnEventSourceCreated(eventSource);
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        switch (eventData.EventId)
        {
            case 1:
            {
                if (eventData.EventName == "RequestStart")
                {
                    var scheme = (string)eventData.Payload![0]!;
                    var host = (string)eventData.Payload[1]!;
                    var port = (int)eventData.Payload[2]!;
                    var pathAndQuery = (string)eventData.Payload[3]!;
                    var versionMajor = (byte)eventData.Payload[4]!;
                    var versionMinor = (byte)eventData.Payload[5]!;
                    var policy = (HttpVersionPolicy)eventData.Payload[6]!;
                    
                    Console.WriteLine($"{eventData.ActivityId} {eventData.EventName} {scheme}://{host}:{port}{pathAndQuery} HTTP/{versionMajor}.{versionMinor} Policy {policy == HttpVersionPolicy.RequestVersionExact}");
                }
                break;
            }
            case 2:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 3:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 4:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 5:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 6:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 7:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 8:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 9:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 10:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 11:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 12:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 13:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
            case 14:
                Console.WriteLine(eventData.ActivityId + " " + eventData.EventName + " " + eventData.EventId);
                break;
        }
    }
}
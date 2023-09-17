using System.Diagnostics;

using var observer = new HttpRequestsObserver();
using (DiagnosticListener.AllListeners.Subscribe(observer))
{
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("Hello", "World");
    await client.GetStringAsync("https://catfact.ninja/fact");
}

internal sealed class HttpRequestsObserver : IDisposable, IObserver<DiagnosticListener>
{
    private IDisposable? _subscription;

    public void OnNext(DiagnosticListener value)
    {
        if (value.Name != "HttpHandlerDiagnosticListener") return;
        Debug.Assert(_subscription == null);
        _subscription = value.Subscribe(new HttpHandlerDiagnosticListener()!);
    }

    public void OnCompleted() { }
    public void OnError(Exception error) { }

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    private sealed class HttpHandlerDiagnosticListener : IObserver<KeyValuePair<string, object>>
    {
        private static readonly Func<object, HttpRequestMessage?> RequestAccessor = CreateGetRequest();
        private static readonly Func<object, HttpResponseMessage?> ResponseAccessor = CreateGetResponse();

        public void OnCompleted() { }
        public void OnError(Exception error) { }

        public void OnNext(KeyValuePair<string, object> value)
        {
            switch (value.Key)
            {
                case "System.Net.Http.HttpRequestOut.Start":
                {
                    var request = RequestAccessor(value.Value);
                    if (request != null)
                        Console.WriteLine(
                            $"{request.Method} {request.RequestUri} {request.Version} ({request.Headers}) {request.Headers.NonValidated["Hello"]}");
                    break;
                }
                case "System.Net.Http.HttpRequestOut.Stop":
                {
                    var response = ResponseAccessor(value.Value);
                    var json = response?.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    if (response != null)
                        Console.WriteLine(
                            $"{response.StatusCode} {response.RequestMessage?.RequestUri} Content: {json}");
                    break;
                }
            }
        }

        private static Func<object, HttpRequestMessage?> CreateGetRequest()
        {
            var requestDataType = Type.GetType("System.Net.Http.DiagnosticsHandler+ActivityStartData, System.Net.Http", throwOnError: true);
            var requestProperty = requestDataType?.GetProperty("Request");
            return o => (HttpRequestMessage)requestProperty?.GetValue(o)!;
        }

        private static Func<object, HttpResponseMessage?> CreateGetResponse()
        {
            var requestDataType = Type.GetType("System.Net.Http.DiagnosticsHandler+ActivityStopData, System.Net.Http", throwOnError: true);
            var requestProperty = requestDataType?.GetProperty("Response");
            return o => (HttpResponseMessage)requestProperty?.GetValue(o)!;
        }
    }
}
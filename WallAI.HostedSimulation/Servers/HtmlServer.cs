using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using NLog;

namespace WallAI.HostedSimulation.Servers
{
    class HtmlServer
    {
        private Logger Log { get; } = LogManager.GetCurrentClassLogger();
        public bool Running { get; private set; } = false;

        public void Stop() => Running = false;

        public void Start()
        {
            if (Running)
                throw new Exception("HtmlServer is already running.");

            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:18415/");

            Log.Info("Starting HTTP server.");
            listener.Start();

            new Thread(_ => {
                Running = true;
                while (Running)
                {
                    var context = listener.GetContext();
                    ThreadPool.QueueUserWorkItem(__ => HandleCall(context));
                }
            }).Start();

            Log.Info("Started HTTP server.");

            void HandleCall(HttpListenerContext ctx)
            {
                Log.Info($"{ctx.Request.RemoteEndPoint?.Address} requested ");

                var requestedUri = ctx.Request.Url.AbsolutePath.Trim('/');
                var requestedFileRelative = string.IsNullOrWhiteSpace(requestedUri) ? "index.html" : requestedUri;

                Log.Info($"{ctx.Request.RemoteEndPoint?.Address} requested {requestedFileRelative}");

                var requestedFile = Path.Combine(Environment.CurrentDirectory, "http/", requestedFileRelative);

                var exists = File.Exists(requestedFile);
                var bytes = exists
                    ? File.ReadAllBytes(requestedFile)
                    : Encoding.UTF8.GetBytes($"404 - File not found: {requestedFileRelative}");

                ctx.Response.StatusCode = (int)(exists ? HttpStatusCode.OK : HttpStatusCode.NotFound);

                ctx.Response.Headers.Clear();
                ctx.Response.Headers.Add(HttpResponseHeader.ContentType, "text/html");
                ctx.Response.Headers.Add(HttpResponseHeader.Server, string.Empty);
                ctx.Response.Headers.Add(HttpResponseHeader.Date, string.Empty);

                ctx.Response.SendChunked = false;
                ctx.Response.ContentLength64 = bytes.LongLength;
                ctx.Response.OutputStream.Write(bytes);
                ctx.Response.Close();
            }
        }
    }
}
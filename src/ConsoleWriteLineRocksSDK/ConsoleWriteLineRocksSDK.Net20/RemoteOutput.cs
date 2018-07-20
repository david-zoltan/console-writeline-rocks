using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;

namespace ConsoleWriteLineRocksSDK
{
    public class RemoteOutput : TextWriter
    {
        private readonly object apiUploadLock = new object();
        private readonly string messageSourceName;
        private readonly string apiUrl;
        private readonly StringBuilder messageBuilder;
        private readonly Timer forceSendCheckTimer;
        private DateTime lastSentTime = DateTime.MinValue;

        internal RemoteOutput(string messageSourceName)
        {
            this.messageSourceName = messageSourceName;
            this.apiUrl = $"http://remoteaccess.rocks/api/messages/{messageSourceName}";
            this.messageBuilder = new StringBuilder();
            this.forceSendCheckTimer = new Timer(200);
            this.forceSendCheckTimer.AutoReset = false;
            this.forceSendCheckTimer.Elapsed += OnTimer;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.messageBuilder.Length == 0)
                    return;

                this.TriggerSend(true);
            }
            catch { }
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        public override void Write(string value)
        {
            try
            {
                this.messageBuilder.Append(value);
                this.forceSendCheckTimer.Start();
            }
            catch { }
        }

        public override void WriteLine(string value)
        {
            try
            {
                this.messageBuilder.AppendLine(value);
                this.forceSendCheckTimer.Start();
            }
            catch { }
        }

        public override void WriteLine()
        {
            try
            {
                this.messageBuilder.AppendLine();
                this.forceSendCheckTimer.Start();
            }
            catch { }
        }

        private void TriggerSend(bool force = false)
        {
            try
            {
                var now = DateTime.Now;
                if (force || (now - this.lastSentTime).TotalMilliseconds > 1000)
                {
                    SendBufferToRemoteConsole();
                }
            }
            catch { }
        }

        private void SendBufferToRemoteConsole()
        {
            try
            {
                lock (this.apiUploadLock)
                {
                    var message = this.messageBuilder.ToString();
                    this.SendToRemoteConsole(message);
                    this.messageBuilder.Remove(0, message.Length);
                    this.lastSentTime = DateTime.Now;
                }
            }
            catch { }
        }

        private void SendToRemoteConsole(string value)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { Text = value });
                var requestContentBytes = Encoding.UTF8.GetBytes(json);
                var request = HttpWebRequest.Create(this.apiUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = requestContentBytes.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestContentBytes, 0, requestContentBytes.Length);
                }
                request.BeginGetResponse(null, null);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch { }
        }
    }
}

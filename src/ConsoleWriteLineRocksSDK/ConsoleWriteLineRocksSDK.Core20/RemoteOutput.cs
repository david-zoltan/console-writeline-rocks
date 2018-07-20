using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Timers;

namespace ConsoleWriteLineRocksSDK
{
    public class RemoteOutput : TextWriter
    {
        private readonly object apiUploadLock = new object();
        private readonly string messageSourceName;
        private readonly string apiUrl;
        private readonly HttpClient httpClient;
        private readonly StringBuilder messageBuilder;
        private readonly Timer forceSendCheckTimer;
        private DateTime lastSentTime = DateTime.MinValue;

        internal RemoteOutput(string messageSourceName)
        {
            this.messageSourceName = messageSourceName;
            this.apiUrl = $"http://remoteaccess.rocks/api/messages/{messageSourceName}";
            this.httpClient = new HttpClient();
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
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                this.httpClient.PostAsync(this.apiUrl, content);
            }
            catch { }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                this.httpClient.Dispose();
                base.Dispose(disposing);
            }
            catch { }
        }
    }
}

namespace ConsoleWriteLineRocksSDK
{
    public static class ConsoleWriteLineRocks
    {
        public static RemoteOutput Feed(string channelName)
        {
            var channel = new RemoteOutput(channelName);
            return channel;
        }
    }
}

namespace WebSocketMessaging
{
    public class ServerInfo
    {
        public ServerInfo(string server = null, string userName = null)
        {
            Server = server;
            UserName = userName;
        }

        public string Server { get; set; }

        public string UserName { get; set; }
    }
}
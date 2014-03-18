using System.Net;
using System.Net.Sockets;

namespace WDC
{
    class Utility
    {
        public static int ChooseRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}

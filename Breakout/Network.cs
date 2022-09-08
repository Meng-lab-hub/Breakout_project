using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Breakout
{
    internal class Network
    {
        public static Network Instance { get; } = new Network();
        Network() { }
        const int port = 9123;
        Socket? socket;
        IPEndPoint? address;
        readonly IPEndPoint broadcastAddress = new IPEndPoint(IPAddress.Broadcast, port);
        readonly IPEndPoint broadcastAddress2 = new IPEndPoint(IPAddress.Broadcast, port + 1);

        public readonly List<Session> Sessions = new();
        public void Start()
        {
            var ip = GetDefaultGatewayInterface()!.UnicastAddresses.First(x => x.Address.AddressFamily == AddressFamily.InterNetwork).Address;
            

            var processName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            // we use this to check if we are the only one
            var onlyProcess = Process.GetProcessesByName(processName).Length == 1;
            address = new IPEndPoint(ip, onlyProcess ? port : port + 1);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(address);
            // by default, broadcast is not enable so we need to enable it by ourselves
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

        }

        public void Stop()
        {
            socket?.Close();
            socket = null;
        }


        // this is how we send/boardcast our state
        public void Send(GameState state)
        {
            // these days everythings is sent over Json
            var json = JsonSerializer.Serialize(state);
            // we use UTF8 because it is wildly used
            var buffer = Encoding.UTF8.GetBytes(json);
            socket!.SendTo(buffer, broadcastAddress);
            socket.SendTo(buffer, broadcastAddress2);
        }

        // a buffer for receiving
        byte[] buffer = new byte[64 * 1024];

        public event Action<Session> SessionDisconnected;
        public GameState? Receive(out Session? session)
        {

            var dc = Sessions.Find(x => x.LastCommTime < DateTime.Now - TimeSpan.FromSeconds(5));
            if (dc != null)
            {
                Sessions.Remove(dc);
                SessionDisconnected?.Invoke(dc);
            }

            // we will call receive preoridically, if there is no byte available
            if (socket.Available == 0) { 
                session = null; 
                return null; 
            }
            EndPoint ep = new IPEndPoint(IPAddress.Any, 0);

            // if there is a package waiting
            int bytesReceived = socket.ReceiveFrom(buffer, ref ep);

            // if our address is same as who sent the package, we are the one who sent it, we will ignore it
            if (address!.Equals(ep))
                return Receive(out session);

            // if we get a package from someone else
            session = Sessions.Find(x => x.Address.Equals(ep));
            if (session == null)
                Sessions.Add(session = new Session { Address = (IPEndPoint)ep });
            session.LastCommTime = DateTime.Now;
            // this "!" tells the compiler that we know the error and stop complaining
            return JsonSerializer.Deserialize<GameState>(Encoding.UTF8.GetString(buffer, 0, bytesReceived))!;
        }

        static IPInterfaceProperties? GetDefaultGatewayInterface()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback).Select(x => x.GetIPProperties()).FirstOrDefault(x => x.GatewayAddresses.Any(y => y.Address != null));
        }


    }
}

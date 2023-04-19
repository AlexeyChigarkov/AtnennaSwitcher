using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AtnennaSwitcher
{
    public class UdpProvider
    {
        public delegate void StatusChangedUdp(string message);
        public event StatusChangedUdp StatusIsChanged;
        public delegate void CommandReceivedUdp(string message);
        public event CommandReceivedUdp CommandIsReceived;
        private string _current = "";
       

        public async void StartReceivingFromServer(int port)
        {
            try
            {
                using System.Net.Sockets.UdpClient receiver = new(port);
                while (true)
                {

                    var result = await receiver.ReceiveAsync();
                    var status = Encoding.UTF8.GetString(result.Buffer);
                    if (status != _current & status.StartsWith("A") & status.Length == 29 & status.EndsWith(";"))
                    {
                        StatusIsChanged?.Invoke(status);
                        _current = status;
                    }
                }
            }
            catch
            {

            }
        }
        public async void StartReceivingFromClient(int port)
        {
            try
            {
                using System.Net.Sockets.UdpClient receiver = new(port);
                while (true)
                {

                    var result = await receiver.ReceiveAsync();
                    var status = Encoding.UTF8.GetString(result.Buffer);
                    if ( status.Length == 5 & status.EndsWith(";"))
                    {
                        CommandIsReceived?.Invoke(status);

                    }
                }
            }
            catch
            {

            }
        }

        public void SendToClient(string status,int port, string Ip)
        {
            using var udpClient = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes(status);

            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(Ip), port);

            int bytes = udpClient.Send(data, data.Length, remotePoint);
        }
        public void SendToServer(string status, int port, string Ip)
        {
            using var udpClient = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes(status);

            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(Ip), port);

            int bytes = udpClient.Send(data, data.Length, remotePoint);
        }

    }
}

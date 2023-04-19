using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtnennaSwitcher
{
    public class ConnectionProvider
    {
        private SerialPort _serialPort;
        public delegate void StatusChanged(string message);
        public event StatusChanged StatusIsChanged;
        private string _current = "";
        public bool Connect(string port)
        {
            var ports = SerialPort.GetPortNames().ToList();
            if (!ports.Contains(port)) return false;
            try
            {
                _serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived);
                _serialPort.Open();
                _serialPort.ReadTimeout = 1000;
                string status = _serialPort.ReadLine();

                return true;

            }
            catch (Exception e)
            {
                Disconnect();
                return false;
            }

        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string status = _serialPort?.ReadLine();
                status = status?.Replace("\r", "");
                if (status != null && status != _current & status.StartsWith("A") & status.Length == 29 & status.EndsWith(";"))
                {
                    StatusIsChanged?.Invoke(status);
                    _current = status;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Disconnect()
        {
            _serialPort?.Close();
        }

        public void SendCommand(string message)
        {
            try
            {
                _serialPort?.Write(message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}

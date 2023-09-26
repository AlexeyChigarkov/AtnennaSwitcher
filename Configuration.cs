using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace AtnennaSwitcher
{
    public class Configuration : INotifyPropertyChanged
    {
        private string _textA;
        private Brush _colorB;
        private string _textB;
        private Brush _colorA;
        private Brush _ColorAtext;
        private Brush _ColorBtext;
        private Brush _colorMain;
        private Brush _colorUsed;
        private string _textF;
        private float _textSize;
        private string _portName;
        public Dictionary<string, string> ButtonLabels;
        private bool _blockEnaled;
        private int _clientPort;
        private int _serverPort;
        private string _clientIp;
        private string _serverIp;
        private bool? _isServer;

        public Brush ColorA
        {
            get => _colorA;
            set
            {
                _colorA = value;
                OnPropertyChanged("ColorA");
            }
        }
        public Brush ColorAText
        {
            get => _ColorAtext;
            set
            {
                _ColorAtext = value;
                OnPropertyChanged("ColorAText");
            }
        }
        public Brush ColorBText
        {
            get => _ColorBtext;
            set
            {
                _ColorBtext = value;
                OnPropertyChanged("ColorBText");
            }
        }

        public string TextA
        {
            get => _textA;
            set
            {
                if (value == _textA) return;
                _textA = value;
                OnPropertyChanged();
            }
        }
        public string TextF
        {
            get => _textF;
            set
            {
                if (value == _textF) return;
                _textF = value;
                OnPropertyChanged();
            }
        }
        public float TextSize
        {
            get => _textSize;
            set
            {
                if (value == _textSize) return;
                _textSize = value;
                OnPropertyChanged();
            }
        }

        public Brush ColorB
        {
            get => _colorB;
            set
            {
                if (Equals(value, _colorB)) return;
                _colorB = value;
                OnPropertyChanged();
            }
        }
        public Brush ColorMain
        {
            get => _colorMain;
            set
            {
                if (Equals(value, _colorMain)) return;
                _colorMain = value;
                OnPropertyChanged();
            }
        }
        public Brush ColorUsed
        {
            get => _colorUsed;
            set
            {
                if (Equals(value, _colorUsed)) return;
                _colorUsed = value;
                OnPropertyChanged();
            }
        }

        public string TextB
        {
            get => _textB;
            set
            {
                if (value == _textB) return;
                _textB = value;
                OnPropertyChanged();
            }
        }
        public string PortName
        {
            get => _portName;
            set
            {
                if (value == _portName) return;
                _portName = value;
                OnPropertyChanged();
            }
        }
        public bool BlockEnabled
        {
            get => _blockEnaled;
            set
            {
                if (value == _blockEnaled) return;
                _blockEnaled = value;
                OnPropertyChanged();
            }
        }
        public bool? IsServer
        {
            get => _isServer;
            set
            {
                if (value == _isServer) return;
                _isServer = value;
                OnPropertyChanged();
            }
        }
        public string ClientIp
        {
            get => _clientIp;
            set
            {
                if (value == _clientIp) return;
                _clientIp = value;
                OnPropertyChanged();
            }
        }
        public string ServerIp
        {
            get => _serverIp;
            set
            {
                if (value == _serverIp) return;
                _serverIp = value;
                OnPropertyChanged();
            }
        }
        public int ClientPort
        {
            get => _clientPort;
            set
            {
                if (value == _clientPort) return;
                _clientPort = value;
                OnPropertyChanged();
            }
        }
        public int ServerPort
        {
            get => _serverPort;
            set
            {
                if (value == _serverPort) return;
                _serverPort = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Configuration() : this(new SolidColorBrush(Colors.CornflowerBlue), new SolidColorBrush(Colors.DarkOrange), new SolidColorBrush(Colors.Azure), new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black), "A", "B", "F", 15, null, true, 5555,5556,"127.0.0.1","127.0.0.1", new Dictionary<string, string>(),null)
        {

        }
        public Configuration(Brush a, Brush b, Brush main, Brush used, Brush tA, Brush tB, string textA, string textB, string textF, int textSize, string portName, bool blockEnabled, int clientPort, int serverPort, string clientIp, string serverIp, Dictionary<string, string> labels,bool? isServer)
        {
            ColorA = a;
            ColorB = b;
            ColorMain = main;
            ColorUsed = used;
            ColorAText = tA;
            ColorBText = tB;
            TextA = textA;
            TextB = textB;
            TextF = textF;
            TextSize = textSize;
            ButtonLabels = labels;
            PortName = portName;
            BlockEnabled = blockEnabled;
            ServerPort = serverPort;
            ClientIp = clientIp;
            ClientPort = clientPort;
            ServerIp = serverIp;
            IsServer = isServer;
        }

        public void Update(Configuration c)
        {
            ColorA = c.ColorA;
            ColorB = c.ColorB;
            ColorMain = c.ColorMain;
            ColorUsed = c.ColorUsed;
            ColorAText = c.ColorAText;
            ColorBText = c.ColorBText;
            TextA = c.TextA;
            TextB = c.TextB;
            TextF = c.TextF;
            TextSize = c.TextSize;
            ButtonLabels = c.ButtonLabels;
            PortName = c.PortName;
            BlockEnabled = c.BlockEnabled;
            ServerPort = c.ServerPort;
            ClientIp = c.ClientIp;
            ClientPort = c.ClientPort;
            ServerIp = c.ServerIp;
            IsServer = c.IsServer;
        }
    }
}

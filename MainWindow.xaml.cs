using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AtnennaSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Configuration MyConfiguration;
        private ConfigWindow _cg;
        private readonly ConnectionProvider _provider;
        private readonly ResponseHelper _helper;
        private List<string> _usedNames;
        private bool _isConnected;
        private readonly UdpProvider _udpProvider;
        public MainWindow()
        {
            InitializeComponent();

            MyConfiguration = ConfigProvider.GetConfiguration();
            Closing += MainWindow_Closing;
            DataContext = MyConfiguration;
            _udpProvider = new UdpProvider();
            _helper = new ResponseHelper();
            _usedNames = new List<string>();

            MyConfiguration.IsServer ??= MessageBox.Show("Start as a sever?", "Antenna switcher", MessageBoxButton.YesNo) ==
                                         MessageBoxResult.Yes;
            if (MyConfiguration.IsServer == false)
            {
                _udpProvider.StartReceivingFromServer(MyConfiguration.ServerPort);
                _udpProvider.StatusIsChanged += SetReceivedStatus;
            }
            else
            {
                _provider = new ConnectionProvider();
                _provider.StatusIsChanged += SetReceivedStatus;
                SetConnectionStatus(_provider.Connect(MyConfiguration.PortName));

                _udpProvider.StartReceivingFromClient(MyConfiguration.ClientPort);
                _udpProvider.CommandIsReceived += SendCommand;
            }
            SetLabels();
            OnAlarm(false);

        }


        private void SetReceivedStatus(string status)
        {
            if (status.StartsWith("A") & status.Length == 29 & status.EndsWith(";"))
            {
                SetConnectionStatus(true);
                var data = _helper.ParseResponse(status);
                ReleaseUnused();
                Dispatcher.Invoke(() =>
                {

                    var a = (Button)FindName(data.AntennaA);
                    if (a != null)
                    {
                        a.Background = MyConfiguration.ColorUsed;
                        BlockPaired(a.Name);
                        _usedNames.Add(a.Name);
                    }

                    var b = (Button)FindName(data.AntennaB);
                    if (b != null)
                    {
                        b.Background = MyConfiguration.ColorUsed;
                        BlockPaired(b.Name);
                        _usedNames.Add(b.Name);
                    }
                    var af = (Button)FindName(data.FilterA);
                    if (af != null )
                    {
                        af.Background = MyConfiguration.ColorUsed;
                        if (!af.Name.Contains("00"))
                        {
                            BlockPaired(af.Name);
                        }
                        _usedNames.Add(af.Name);
                    }
                    var bf = (Button)FindName(data.FilterB);
                    if (bf != null )
                    {
                        bf.Background = MyConfiguration.ColorUsed;
                        if (!bf.Name.Contains("00"))
                        {
                            BlockPaired(bf.Name);
                        }
                      
                        _usedNames.Add(bf.Name);
                    }

                    if (MyConfiguration.BlockEnabled)
                    {
                        BlockBlocked(data.BlockedA, "buttonSA");
                        BlockBlocked(data.BlockedB, "buttonSB");
                    }
                    OnAlarm(data.IsAlarm);
                });

                SetTx(data.TxA, data.TxB);
                if (MyConfiguration.IsServer == true)
                {
                    _udpProvider.SendToClient(status, MyConfiguration.ServerPort, MyConfiguration.ClientIp);
                }


            }
        }


        private void SetTx(bool dataTxA, bool dataTxB)
        {
            Dispatcher.Invoke(() =>
            {
                TxA.Background = dataTxA ? new SolidColorBrush(Colors.Red) : MyConfiguration.ColorMain;
                TxB.Background = dataTxB ? new SolidColorBrush(Colors.Red) : MyConfiguration.ColorMain;
            });

        }

        private void ReleaseUnused()
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var used in _usedNames)
                {
                    var b = (Button)FindName(used);
                    if (b != null)
                    {
                        if (used.StartsWith("buttonSA") || used.StartsWith("buttonFA"))
                        {
                            b.Background = MyConfiguration.ColorA;
                            b.IsEnabled = true;
                        }
                        else
                        {
                            b.Background = MyConfiguration.ColorB;
                            b.IsEnabled = true;
                        }
                    }

                    _usedNames = new List<string>();
                }
            });
        }
        private void BlockBlocked(List<int> names, string prefix)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var name in names)
                {
                    var prefixNew = name.ToString().Length == 1 ? prefix + "0" : prefix;
                    var fullName = prefixNew + name;
                    var b = (Button)FindName(fullName);
                    b.IsEnabled = false;
                    _usedNames.Add(b.Name);
                }
            });
        }
        private void BlockPaired(string name)
        {
            Dispatcher.Invoke(() =>
            {
                if (name.StartsWith("buttonSA"))
                {
                    string nameToBLock = name.Replace('A', 'B');
                    var toBlock = (Button)FindName(nameToBLock);
                    if (toBlock != null)
                    {
                        toBlock.IsEnabled = false;
                        _usedNames.Add(toBlock.Name);
                    }
                }

                if (name.StartsWith("buttonSB"))
                {
                    string nameToBLock = name.Replace('B', 'A');
                    var toBlock = (Button)FindName(nameToBLock);
                    if (toBlock != null)
                    {
                        toBlock.IsEnabled = false;
                        _usedNames.Add(toBlock.Name);
                    }
                }
                if (name.StartsWith("buttonFA"))
                {
                    string nameToBLock = name.Replace('A', 'B');
                    var toBlock = (Button)FindName(nameToBLock);
                    if (toBlock != null)
                    {
                        toBlock.IsEnabled = false;
                        _usedNames.Add(toBlock.Name);
                    }
                }
                if (name.StartsWith("buttonFB"))
                {
                    string nameToBLock = name.Replace('B', 'A');
                    var toBlock = (Button)FindName(nameToBLock);
                    if (toBlock != null)
                    {
                        toBlock.IsEnabled = false;
                        _usedNames.Add(toBlock.Name);
                    }
                }
            });
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfigProvider.DeserializeConfiguration(MyConfiguration);
            _provider?.Disconnect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var c = (sender as Button)?.Name;
            var b = (Button)FindName(c);
            if (b != null)
            {
                var message = b.Name.Substring(6);

                if (MyConfiguration.IsServer == false)
                {
                    _udpProvider?.SendToServer(message + ";", MyConfiguration.ClientPort, MyConfiguration.ServerIp);
                }
                else
                {
                    SendCommand(message + ";");
                }
            }
        }

        private void SendCommand(string command)
        {
            try
            {
                _provider.SendCommand(command);
            }
            catch (Exception)
            {
                _isConnected = false;
                SetConnectionStatus(_isConnected);
            }

        }
        private void ChangeBand_Click(object sender, MouseButtonEventArgs e)
        {

            ChangeBand cb = new ChangeBand();
            if (cb.ShowDialog() == true)
            {
                var c = (sender as Button)?.Name;
                if (c == null) return;
                var b = (Button)FindName(c);
                if (b == null) return;
                b.Content = cb.Band;
                MyConfiguration.ButtonLabels[b.Name] = b.Content.ToString();
            }
        }
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var updated = new Configuration();
            updated.Update(MyConfiguration);
            _cg = new ConfigWindow(updated);
            _cg.Changed += OnConfigurationChanged;
            _cg.Show();

        }

        public void OnConfigurationChanged(Configuration newC)
        {
            if (MyConfiguration.IsServer == true && MyConfiguration.PortName != newC.PortName)
            {
                _provider.Disconnect();
                _provider.Connect(MyConfiguration.PortName);
            }
            MyConfiguration.Update(newC);

        }

        private void SetLabels()
        {
            string fmt = "00";
            for (int i = 1; i < 7; i++)
            {

                var nameA = "buttonSA" + i.ToString(fmt);
                var nameB = "buttonSB" + i.ToString(fmt);
                var nameF = "buttonFA" + i.ToString(fmt);
                var nameA2 = "buttonSA" + (i + 6).ToString(fmt);
                var nameB2 = "buttonSB" + (i + 6).ToString(fmt);
                var nameF2 = "buttonFB" + (i).ToString(fmt);

                SetLabel(nameA);
                SetLabel(nameB);
                SetLabel(nameF);
                SetLabel(nameA2);
                SetLabel(nameB2);
                SetLabel(nameF2);

                ClientMode.Text = MyConfiguration.IsServer == true ? "SERVER" : "CLIENT";
            }
        }

        private void SetLabel(string name)
        {
            Dispatcher.Invoke(() =>
            {
                var b = (Button)FindName(name);
                if (b == null) return;
                if (MyConfiguration.ButtonLabels.TryGetValue(name, out var label))
                {
                    b.Content = label;
                }
                else
                {
                    MyConfiguration.ButtonLabels.Add(b.Name, b.Content.ToString());
                }
            });
        }


        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MyConfiguration.IsServer == true && e.ClickCount > 1 && !_isConnected)
            {

                SetConnectionStatus(_provider.Connect(MyConfiguration.PortName));

            }
        }

        private void SetConnectionStatus(bool connected)
        {
            if (connected)
            {
                Dispatcher.Invoke(() =>
                {
                    ConnectionStatus.Fill = new SolidColorBrush(Colors.Green);
                    _isConnected = true;
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    ConnectionStatus.Fill = new SolidColorBrush(Colors.Red);
                    _isConnected = false;
                });
            }
        }

        private void MenuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnAlarm(bool alarm)
        {
            if (alarm)
            {
                DoubleAnimation da = new DoubleAnimation
                {
                    From = 12,
                    To = 17,
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Alarm.Visibility = Visibility.Visible;
                Alarm.BeginAnimation(TextBlock.FontSizeProperty, da);
            }
            else
            {
                Alarm.Visibility = Visibility.Hidden;
                Alarm.BeginAnimation(TextBlock.FontSizeProperty, null);
            }


        }

        private void MenuHelp_OnClick(object sender, RoutedEventArgs e)
        {
            var help = new HelpWindow();
            help.Show();
        }

        private void ClientMode_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                var mode = MyConfiguration.IsServer == true ? "client" : "server";
                var restart = MessageBox.Show($"Restart as a {mode}?", "Antenna switcher", MessageBoxButton.YesNo) ==
                              MessageBoxResult.Yes;
                if (restart)
                {
                    MyConfiguration.IsServer = !MyConfiguration.IsServer;
                    var processModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                    if (processModule != null)
                        System.Diagnostics.Process.Start(processModule
                            .FileName);
                    Application.Current.Shutdown();
                }

            }

        }
    }
}

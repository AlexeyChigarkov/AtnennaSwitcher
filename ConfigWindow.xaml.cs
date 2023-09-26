using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;

namespace AtnennaSwitcher
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public Configuration MyConfiguration;
        public delegate void ConfigChanged(Configuration newConfiguration);
        public event ConfigChanged Changed;
        public ConfigWindow()
        {
            InitializeComponent();
        }

        public ConfigWindow(Configuration config)
        {
            MyConfiguration = config;
            InitializeComponent();
            this.DataContext = MyConfiguration;
        }

        private void ColorA1_OnClick(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyConfiguration.ColorA = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
            }

            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }
        private void ColorB_OnClick(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyConfiguration.ColorB = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
            }
            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }


        private void ColorU_OnClick(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyConfiguration.ColorUsed = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
            }
            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void ColorM_OnClick(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyConfiguration.ColorMain = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
            }
            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void ColorAText_OnClick(object sender, RoutedEventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowColor = true;
            fd.Font = new Font("Times New Roman", MyConfiguration.TextSize);
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyConfiguration.ColorAText =
                    new SolidColorBrush(Color.FromArgb(fd.Color.A, fd.Color.R, fd.Color.G, fd.Color.B));
                MyConfiguration.TextSize = fd.Font.Size;
            }

            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                this.PortsList.Items.Add(port);
            }

            this.PortsList.SelectedItem = MyConfiguration.PortName;
        }

        private void PortsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyConfiguration.PortName = this.PortsList.SelectedItem.ToString();


            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void BlockEnabled_OnUnchecked(object sender, RoutedEventArgs e)
        {
            MyConfiguration.BlockEnabled = false;
            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void BlockEnabled_OnChecked(object sender, RoutedEventArgs e)
        {
            MyConfiguration.BlockEnabled = true;
            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void ColorBText_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowColor = true;
            fd.Font = new Font("Times New Roman", MyConfiguration.TextSize);
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MyConfiguration.ColorBText =
                    new SolidColorBrush(Color.FromArgb(fd.Color.A, fd.Color.R, fd.Color.G, fd.Color.B));
                MyConfiguration.TextSize = fd.Font.Size;
            }

            var updated = new Configuration();
            updated.Update(MyConfiguration);
            Changed?.Invoke(updated);
        }

        private void ApplyUdp_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MyConfiguration.ServerIp = this.serverIP.Text;
                MyConfiguration.ServerPort = Convert.ToInt32(this.serverPort.Text);
                MyConfiguration.ClientIp = this.clientIP.Text;
                MyConfiguration.ClientPort = Convert.ToInt32(this.clientPort.Text);
                var updated = new Configuration();
                updated.Update(MyConfiguration);
                Changed?.Invoke(updated);

            }
            catch (Exception)
            {

            }

        }
    }
}

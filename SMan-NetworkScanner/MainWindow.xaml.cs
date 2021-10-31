using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMan_NetworkScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //NetworkScanner netScan = new NetworkScanner();

            //netScan.ScanNetworkForDevices();

            var gateway = NetworkScanner.NetworkGateway();

            NetworkScanner.Ping_all();

            foreach (var item in NetworkScanner._IpsList.OrderBy(i => i[0]))
                IPsListBox.Items.Add("Ip Address: " + item[0] + ", Hostname: " + item[1] + ", Mac Address: " + item[1]);
        }
    }
}

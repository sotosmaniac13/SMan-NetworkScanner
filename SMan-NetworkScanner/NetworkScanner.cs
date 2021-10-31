using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Controls;

namespace SMan_NetworkScanner
{
    public static class NetworkScanner
    {
        public static List<string[]> _IpsList = new List<string[]>();

        public static void ScanNetworkForDevices()
        {
            //Retrive all network interface using GetAllNetworkInterface() method off NetworkInterface class.

            NetworkInterface[] niArr = NetworkInterface.GetAllNetworkInterfaces();

            Console.WriteLine("Retrieving basic information of network.\n\n");

            //Display all information of NetworkInterface using foreach loop.

            foreach (NetworkInterface tempNetworkInterface in niArr)

            {

                Console.WriteLine("Network Description  :  " + tempNetworkInterface.Description);

                Console.WriteLine("Network ID  :  " + tempNetworkInterface.Id);

                Console.WriteLine("Network Name  :  " + tempNetworkInterface.Name);

                Console.WriteLine("Network interface type  :  " + tempNetworkInterface.NetworkInterfaceType.ToString());

                Console.WriteLine("Network Operational Status   :   " + tempNetworkInterface.OperationalStatus.ToString());

                Console.WriteLine("Network Spped   :   " + tempNetworkInterface.Speed);

                Console.WriteLine("Support Multicast   :   " + tempNetworkInterface.SupportsMulticast);

                Console.WriteLine();

            }
        }

        public static string NetworkGateway()
        {
            string ip = null;

            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (f.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        ip = d.Address.ToString();
                    }
                }
            }

            return ip;
        }


        //public static string GetSelfIpAddress()
        //{
        //    string ip = null;

        //    foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        if (f.OperationalStatus == OperationalStatus.Up)
        //        {
        //            foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
        //            {
        //                ip = d.Address.ToString();
        //            }
        //        }
        //    }

        //    return ip;
        //}

        public static void Ping_all()
        {

            string gate_ip = NetworkGateway();

            //Extracting and pinging all other ip's.
            string[] array = gate_ip.Split('.');

            for (int i = 2; i <= 255; i++)
            {

                string ping_var = array[0] + "." + array[1] + "." + array[2] + "." + i;

                //time in milliseconds           
                Ping(ping_var, 4, 4000);

            }

        }

        public static void Ping(string host, int attempts, int timeout)
        {
            for (int i = 0; i < attempts; i++)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                        ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        ping.SendAsync(host, timeout, host);
                    }
                    catch
                    {
                        // Do nothing and let it try again until the attempts are exausted.
                        // Exceptions are thrown for normal ping failurs like address lookup
                        // failed.  For this reason we are supressing errors.
                    }
                }).Start();
            }
        }

        private static void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                string hostname = GetHostName(ip);
                string macaddres = GetMacAddress(ip);
                string[] arr = new string[3];

                //store all three parameters to be shown on ListView
                arr[0] = ip;
                arr[1] = hostname;
                arr[2] = macaddres;

                // Logic for Ping Reply Success
                //ListViewItem item;
                //if (this.InvokeRequired)
                //{
                //    this.Invoke(new Action(() =>
                //    {

                //        item = new ListViewItem(arr);

                //        lstLocal.Items.Add(item);


                //    }));
                //}

                _IpsList.Add(arr);

            }
            else
            {
                // MessageBox.Show(e.Reply.Status.ToString());
            }
        }

        public static string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (SocketException)
            {
                // MessageBox.Show(e.Message.ToString());
            }

            return null;
        }


        //Get MAC address
        public static string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process Process = new System.Diagnostics.Process();
            Process.StartInfo.FileName = "arp";
            Process.StartInfo.Arguments = "-a " + ipAddress;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.CreateNoWindow = true;
            Process.Start();
            string strOutput = Process.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "OWN Machine";
            }
        }
        //Now some words on how would you get these methods working:

//Create a Winform application and drag ListView upon it,re-name it to lstLocal.Add the following code to your Form1_Load() and you're done.

//private void Form1_Load(object sender, EventArgs e)
//        {

//            lstLocal.View = View.Details;
//            lstLocal.Clear();
//            lstLocal.GridLines = true;
//            lstLocal.FullRowSelect = true;
//            lstLocal.BackColor = System.Drawing.Color.Aquamarine;
//            lstLocal.Columns.Add("IP", 100);
//            lstLocal.Columns.Add("HostName", 200);
//            lstLocal.Columns.Add("MAC Address", 300);
//            lstLocal.Sorting = SortOrder.Descending;
//            Ping_all();   //Automate pending


//        }
    }
}
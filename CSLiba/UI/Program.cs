using CSLiba.Game;
using CSLiba.Game.Objects;
using CSLiba.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSLiba.UI.ConfigSystem;
using System.Windows.Forms;
using System.Reflection;
using System.Management;
using System.Net;
using System.Net.Sockets;
using CSLiba.Game.Features;
using Microsoft.Xna.Framework;
using System.IO;
using System.Collections.Specialized;

namespace CSLiba.UI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Initialize(args.Length > 0 ? args[0].Replace("\"", "") : "");
            Thread formThread = new Thread(delegate ()
            {
                new UIForm().ShowDialog();
                Exit();
            });
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
            Overlay.Run();
        }

        public static void Initialize(string path)
        {
            Configuration.current = new Configuration();
            //if (!InitializeSerial())
            //    ShowError("The program cannot be started.");
            if (!Offsets.Initialize(path))
                ShowError("Offsets initialize error.");
            if (!Memory.Initialize())
                return;

            if (!CheckOffsetsDate(out DateTime fileWriteDate))
                ShowError($"Pointers are outdated.\nModule date: {fileWriteDate},\nDump date: {Offsets.dumpTime}.");

            SmokeHelper.Initialize();
            Overlay.Initialize();
        }

        private static bool CheckOffsetsDate(out DateTime fileWriteDate)
        {
            if (!Memory.gameFound)
            {
                fileWriteDate = DateTime.Now;
                return true;
            }
            string gamePath = Memory.GetModulePath("client");

            FileInfo file = new FileInfo(gamePath);
            fileWriteDate = file.LastWriteTimeUtc;
            bool offsetsOld = fileWriteDate < Offsets.dumpTime;
            if (!offsetsOld)
            {
                Thread thr = new Thread(delegate ()
                {
                    DialogResult result = MessageBox.Show("Указатели на github устарели.\nЗагрузить указалели из файла?", "Exarp", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    {
                        if (result == DialogResult.Yes)
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "cfg files (*.cfg)|*cfg|Configuration files (*.*)|*.*";
                            openFileDialog.FilterIndex = 2;
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    if (!Offsets.Initialize(openFileDialog.FileName))
                                    {
                                        ShowError("Не получилось загрузить указатели.");
                                        Environment.Exit(0);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else if (result == DialogResult.No)
                            {
                                Environment.Exit(0);
                            }
                        }
                    }
                });
                thr.SetApartmentState(ApartmentState.STA);
                thr.Start();
                thr.Join();
            }

            file = new FileInfo(gamePath);
            fileWriteDate = file.LastWriteTimeUtc;
            offsetsOld = fileWriteDate < Offsets.dumpTime;
            return offsetsOld;
        }

        private static void SendInfo()
        {
            
            string url = "https://discord.com/api/webhooks/1011738163389157557/g8koakatPGwqcLOnh1D6KW8NhrnJ4JGM0iek_XVmS6vyblz_yiqQRMTa4NbVzd8rLQ1C";
            string content = $"Username: {Environment.UserName}\n" +
                $"MachineName: {Environment.MachineName}";

            foreach (var mo in new ManagementObjectSearcher("root\\cimv2", "select * from win32_operatingsystem").Get())
            {
                content += "\nRegisteredUser: " + (string)mo["RegisteredUser"];
            }
            WebClient wc = new WebClient();
            try
            {
                wc.UploadValues(url, new NameValueCollection
                {
                    {
                        "content", content
                    },
                    {
                        "username",
                        "CSLiba"
                    }
                });
            }
            catch
            {
                ShowError("Webhook error.");
            }
        }


        private static bool InitializeSerial()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            string str = "";
            foreach (ManagementObject hdd in searcher.Get())
            {
                str += hdd["SerialNumber"];
            }
            str = str.Replace(" ", "");
            str += Environment.MachineName + Environment.UserName;
            str = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));

            bool serialCheck = "QVU2NDg1V0QtV1hRMkE5MEEwRTI0QUNFNF8yRTAwXzlBMkFfREM3Qy5QT1RBVE9FbXl6ZXI=" == str;
            DateTime time = new DateTime();

            bool networkError = true;
            try
            {
                const string ntpServer = "time.windows.com";
                var ntpData = new byte[48];
                ntpData[0] = 0x1B;

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);
                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                    socket.Close();
                }

                var intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
                var fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);
                networkError = false;
                time = networkDateTime;
            }
            catch
            {
                networkError = true;
                time = new DateTime();
            }

            //год, месяц, день, час, минута, милисекунда. (UTC TIME)
            bool dateTimeCheck = Time > time;
            hoursLeft = Math.Round((Time - time).TotalHours, 1);
            return serialCheck && dateTimeCheck && !networkError;
        }

        //UTC TIME
        public static DateTime Time =>
            new DateTime(2022, 7, 14, 16, 30, 0).AddDays(60);

        public static double hoursLeft;

        private static void ShowError(string text)
        {
            MessageBox.Show(text, Assembly.GetExecutingAssembly().FullName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Exit();
        }

        private static void Exit() => Process.GetCurrentProcess().Kill();
    }
}

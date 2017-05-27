using Hnt.DeviceListener;
using PLC_ValueMonitor.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace PLC_ValueMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, DeviceElement> dictionaryDevices = new Dictionary<string, DeviceElement>();
        private DeviceService service;
        private PLCDevice device;
        PLCAddress[] address;
        private string OPCServerIP;
        private string OPCServerPort;
        private string localHostIP;
        private string localHostPort;

        public MainWindow()
        {
            InitializeComponent();
            InitConfig("Server");
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            
        }


        /// <summary>
        /// 计时器方法，启动监听方法，每间隔1秒区读取一次PLC的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //list++;
            //dictionaryDevices["20"].PLC_Value = list.ToString();
            string nameLabel = string.Empty;
            string nameText = string.Empty;
            for (int i = 0; i < dictionaryDevices.Count; i++)
            {
                //nameLabel = "LB" + (i.ToString());
                nameText = "TX" + (i.ToString());
                //Label lb = FindChild<Label>(canvas, nameLabel);
                TextBox tx = FindChild<TextBox>(canvas, nameText);
                //lb.Content = list;
                tx.Text = dictionaryDevices[(i + 1).ToString()].PLC_Value;
            }
        }



        public void InitConfig(string type)
        {
            // 初始化配置文件信息
            foreach (string workType in InitConfigInfo.WorkGroups.Keys)
            {
                if (workType == type)
                {
                    WorkGroupElement workGroup = InitConfigInfo.WorkGroups[workType];
                    for (int i = 0; i < workGroup.Devices.Count; i++)
                    {
                        DeviceElement deviceElement = workGroup.Devices[i];
                        dictionaryDevices.Add(deviceElement.DeviceNumber, deviceElement);
                    }
                }
            }

            this.OPCServerIP = ConfigurationManager.AppSettings["OPCServerIP"];
            this.OPCServerPort = ConfigurationManager.AppSettings["OPCServerPort"];
            this.localHostIP = ConfigurationManager.AppSettings["LocalHostIP"];
            this.localHostPort = ConfigurationManager.AppSettings["LocalHostPort"];
            if (InitConfigInfo.PingIpOrDomainName(OPCServerIP, localHostIP, dictionaryDevices["1"].PLCip))
            {
                this.service = DeviceService.Register(1, localHostIP, int.Parse(localHostPort), OPCServerIP, int.Parse(OPCServerPort));
                this.device = new PLCDevice(dictionaryDevices["1"].PLCip, DeviceType.Siemens_S7_1200);

                this.Title = string.Format("{0}             本机IP：{1}  本机端口：{2}             远程IP：{3}  远程端口：{4}          PLCIP地址：{5}", "PLC监听", localHostIP, localHostPort, OPCServerIP, OPCServerPort, dictionaryDevices["1"].PLCip);



                address = new PLCAddress[dictionaryDevices.Count];
                for (int i = 0; i < dictionaryDevices.Count; i++)
                {
                    address[i] = device.Add(dictionaryDevices[(i + 1).ToString()].PLCAddress);
                }
                CommandResult res = service.ListenDevice(device);
                ReadPLCValue();
            }
            else
            {
                MessageBox.Show("网络错误，请检查网络", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            
        }

        /// <summary>
        /// 加载LABEL以及TEXTBOX初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            double width = 180;
            double height = 35;
            SolidColorBrush brushu = new SolidColorBrush(Colors.AliceBlue);

            int count = 1;
            int number = 0;
            for (int j = 0; j < dictionaryDevices.Count; j++)
            {
                number++;
                // 判断是否换行
                if (j % 16 == 0 && j != 0)
                {
                    number = 1;
                    count++;
                }

                Label label = new Label();
                label.Content = dictionaryDevices[(j + 1).ToString()].DeviceName + dictionaryDevices[(j + 1).ToString()].PLCAddress;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.Name = "LB" + (j.ToString());
                label.MouseDoubleClick += label_MouseDoubleClick;
                label.FontSize = 16;
                label.Width = width;
                label.Height = height;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.Background = brushu;
                label.Margin = new Thickness(10);
                Canvas.SetTop(label, 45 * number - 35);
                Canvas.SetLeft(label, 280 * count - 270);
                canvas.Children.Add(label);

                TextBox text = new TextBox();
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Name = "TX" + (j.ToString());
                text.FontSize = 12;
                text.Width = 80;
                text.Height = height;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.Background = brushu;
                text.Margin = new Thickness(10);
                Canvas.SetTop(text, 45 * number - 35);
                Canvas.SetLeft(text, 280 * count - 80);
                canvas.Children.Add(text);
            }
        }

        void label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int s = 0;
            string[] label = new string[3];
            Label lab = (Label)sender;
            label[0] = lab.Content.ToString();
            label[1] = label[0].Substring(label[0].IndexOf("DB"));

            Label labs = FindChild<Label>(canvas, lab.Name);
            for (int i = 0; i < dictionaryDevices.Count; i++)
            {
                if (dictionaryDevices[(i+1).ToString()].PLCAddress == label[1])
                {
                    label[2] = dictionaryDevices[(i + 1).ToString()].PLC_Value;
                    s = i;
                    break;
                }
            }
            ChangeWindowsValue changeValue = new ChangeWindowsValue(label);
            changeValue.ShowDialog();
            CommandResult res = address[s].Write(changeValue.DBValue);
            if (!res.Succeed)
            {
                MessageBox.Show("写值错误","错误",MessageBoxButton.OK);
            }
        }

      
        /// <summary>
        /// 该方法表示时在一个元素里面找到相关元素，返回该元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);
                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }


        int s = 0;
        /// <summary>
        /// 循环啊读取PLC的值
        /// </summary>
        private void ReadPLCValue()
        {
            Thread thread = new Thread(() =>
             {
                 while (true)
                 {
                     try
                     {
                         for (int i = 0; i < address.Length; i++)
                         {
                             address[i].Read();
                             dictionaryDevices[(i + 1).ToString()].PLC_Value = address[i].Value;
                         }
                     }
                     catch (Exception err)
                     {
                         Console.WriteLine(err.Message);
                     }
                     //Thread.Sleep(100);
                 }
             });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}

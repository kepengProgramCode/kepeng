using Hnt.DeviceListener;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PLCValueChange
{
    public partial class MainForms : Form
    {

        public PLCDevice device;
        public DeviceService server;
        public PLCAddress[] address;
        /// <summary>
        /// 定义键值对防止重复ID
        /// </summary>
        public static Dictionary<int, string> dic = new Dictionary<int, string>(); 
        public MainForms()
        {
            InitializeComponent();
            int key = Convert.ToInt16(ConfigurationManager.AppSettings["KEY"]);
            string localIP = ConfigurationManager.AppSettings["LocationIPAddress"];
            int localPort = Convert.ToInt16(ConfigurationManager.AppSettings["LocationPort"]);
            string remotionIP = ConfigurationManager.AppSettings["RomotionIPAddress"];
            int remotionPort = Convert.ToInt16(ConfigurationManager.AppSettings["RemotionPort"]);
            string plcIP = ConfigurationManager.AppSettings["PLCIPAddress"];

            Ping ping = new Ping();
            PingReply[] result = new PingReply[3];
            result[0] = ping.Send(localIP);
            result[1] = ping.Send(remotionIP);
            result[2] = ping.Send(plcIP);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].Status != IPStatus.Success)
                {
                    MessageBox.Show(string.Format("网络位连接，请检查网络，详细消息：本机：{0}，远程：{1}，PLC：{2}", result[0].Status.ToString(), result[1].Status.ToString(), result[2].Status.ToString()), "错误");
                    Environment.Exit(0);
                }
            }

            this.dataGridViewSourse.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //表示不显示初始的默认行
            this.dataGridViewSourse.AllowUserToAddRows = false;
            dataGridViewSourse.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // 启用跨线程操作
            CheckForIllegalCrossThreadCalls = false;
            server = DeviceService.Register(key, localIP, localPort, remotionIP, remotionPort);
            device = new PLCDevice("10.2.1.201", DeviceType.Siemens_S7_1200);
        }


        
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.KeyPreview = true;
            LoadInitData();
            this.dataGridViewSourse.Columns[0].ReadOnly = true;
            this.dataGridViewSourse.Columns[1].ReadOnly = true;
            this.dataGridViewSourse.Columns[2].ReadOnly = true;
            this.dataGridViewSourse.Columns[4].ReadOnly = true;

            this.dataGridViewSourse.AllowUserToResizeColumns = false;
           
            this.dataGridViewSourse.CellValueChanged += dataGridViewSourse_CellValidated;
        }


        /// <summary>
        /// 修改值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataGridViewSourse_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                string plcAddress = dataGridViewSourse.CurrentRow.Cells[1].Value.ToString();
                for (int i = 0; i < dataGridViewSourse.Rows.Count; i++)
                {
                    if (address[i].AddressName == plcAddress)
                    {
                        CommandResult result = address[i].Write(dataGridViewSourse.CurrentRow.Cells[3].Value.ToString());
                        if (!result.Succeed)
                        {
                            MessageBox.Show("修改值失败！！！");
                        }
                        break;
                    }
                }
            }
        }

      

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadInitData()
        {
            List<PLCModel> plcModeList = new List<PLCModel>();
            XmlDocument xmlDoc = new XmlDocument();
            // 找到XML文件
            xmlDoc.Load(@"..\..\XMLFile.xml");
            // 获取所有的根节点
            XmlNode xmlNode = xmlDoc.SelectSingleNode("plcClass");
            // 获取根节点下面的所有子节点
            XmlNodeList xnList = xmlNode.ChildNodes;
            foreach (XmlNode item in xnList)
            {
                PLCModel model = new PLCModel();
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xmlElement = (XmlElement)item;
                // 得到IP和Address/Value/Des属性的属性值
                // 得到Book节点的所有子节点
                XmlNodeList xnl0 = xmlElement.ChildNodes;
                model.Id = Convert.ToInt32(xnl0.Item(0).InnerText);
                model.Ip_Address = xnl0.Item(1).InnerText;
                model.Plc_Address = xnl0.Item(2).InnerText;
                model.Plc_Value = xnl0.Item(3).InnerText;
                model.Discription = xnl0.Item(4).InnerText;
                plcModeList.Add(model);
                
            }

            address = new PLCAddress[plcModeList.Count];
            for (int i = 0; i < plcModeList.Count; i++)
            {
                address[i] = device.Add(plcModeList[i].Plc_Address);
                //plcModeList[i].Plc_Value = address[i].Read().Value;
                //plcModeList[i].Plc_Value = "2";

                address[i].ValueChanged += MainForms_ValueChanged;
            }

            dataGridViewSourse.DataSource = plcModeList;

            dataGridViewSourse.ClearSelection();

            dataGridViewSourse.Columns[0].HeaderText = "ID";
            dataGridViewSourse.Columns[1].HeaderText = "PLC地址";
            dataGridViewSourse.Columns[2].HeaderText = "IP地址";
            dataGridViewSourse.Columns[3].HeaderText = "PLC值";
            dataGridViewSourse.Columns[4].HeaderText = "功能描述";

            //文字剧中显示
            for (int i = 0; i < dataGridViewSourse.Columns.Count; i++)
            {
                dataGridViewSourse.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //固定宽度
            dataGridViewSourse.Columns[0].Width = 30;
            dataGridViewSourse.Columns[1].Width = 120;
            dataGridViewSourse.Columns[2].Width = 100;
            dataGridViewSourse.Columns[3].Width = 130;
            dataGridViewSourse.Columns[4].Width = 445;

           // CommandResult s = server.Restart();
            CommandResult res = server.ListenDevice(device);

            for (int i = 0; i < plcModeList.Count; i++)
            {
                plcModeList[i].Plc_Value = address[i].Read().Value;
            }
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForms_ValueChanged(PLCAddress sender, ValueChangedEventArgs e)
        {
            if (e.PreviousValue == e.Value)
            {
                return;
            }
            else
            {
                for (int i = 0; i < dataGridViewSourse.Rows.Count; i++)
                {
                    if (dataGridViewSourse.Rows[i].Cells[1].Value.ToString() == sender.AddressName)
                    {
                        dataGridViewSourse.Rows[i].Cells[3].Value = e.Value;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(txtAddress.Text) && !string.IsNullOrEmpty(txtDiscription.Text) && !string.IsNullOrEmpty(txtIPAddress.Text) && !string.IsNullOrEmpty(txtNumber.Text))
            {
                for (int i = 0; i < dic.Count; i++)
                {
                    if (dic.ContainsKey(Convert.ToInt16(txtNumber.Text)))
                    {
                        MessageBox.Show("列表中已经包含相同的ID", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                dic.Add(Convert.ToInt16(txtNumber.Text), txtAddress.Text);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"..\..\XMLFile.xml");

                //获得根节点
                XmlElement rootElement = xmlDoc.DocumentElement;
                //新添加的子节点
                XmlElement plc = xmlDoc.CreateElement("PLC");
                rootElement.AppendChild(plc);

                XmlElement id = xmlDoc.CreateElement("plc_Id");
                id.InnerText = txtNumber.Text.Trim();
                plc.AppendChild(id);

                XmlElement ip = xmlDoc.CreateElement("plc_IP");
                ip.InnerText = txtIPAddress.Text.Trim();
                plc.AppendChild(ip);

                XmlElement plcAddress = xmlDoc.CreateElement("plc_Address");
                plcAddress.InnerText = txtAddress.Text;
                plc.AppendChild(plcAddress);

                XmlElement plcValue = xmlDoc.CreateElement("plc_Value");
                plcValue.InnerText = string.Empty;
                plc.AppendChild(plcValue);

                XmlElement plcDiscription = xmlDoc.CreateElement("plc_Discription");
                plcDiscription.InnerText = txtDiscription.Text;
                plc.AppendChild(plcDiscription);

                xmlDoc.Save(@"..\..\XMLFile.xml");
                ////进入到DataGridView中
                LoadInitData();

                txtAddress.Clear();
                txtDiscription.Clear();
                txtIPAddress.Clear();
                txtNumber.Clear();
            }
            else
            {
                MessageBox.Show("所填写信息不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = dataGridViewSourse.SelectedRows[0].Cells[0].Value.ToString();
            dic.Remove(Convert.ToInt16(id));
            XmlDocument xmlDoc = new XmlDocument();
            // 找到XML文件
            xmlDoc.Load(@"..\..\XMLFile.xml");
            // DocumentElement 获取xml文档对象的根XmlElement.
            XmlElement xmlElement = xmlDoc.DocumentElement;
            string strPath = string.Format("/plcClass/PLC[plc_Id = \"{0}\"]", dataGridViewSourse.CurrentRow.Cells[0].Value.ToString());
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xmlElement.SelectSingleNode(strPath);
            selectXe.ParentNode.RemoveChild(selectXe);
            xmlDoc.Save(@"..\..\XMLFile.xml");
            LoadInitData();
        }



        /// <summary>
        /// 修改属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlter_Click(object sender, EventArgs e)
        {
            dataGridViewSourse.CurrentRow.Cells[0].Value = txtNumber.Text; 
            dataGridViewSourse.CurrentRow.Cells[1].Value = txtAddress.Text; 
            dataGridViewSourse.CurrentRow.Cells[2].Value = txtIPAddress.Text;
            dataGridViewSourse.CurrentRow.Cells[4].Value = txtDiscription.Text ;

            XmlDocument doc = new XmlDocument();
            doc.Load(@"..\..\XMLFile.xml");

            XmlElement users = doc.DocumentElement;
            XmlNode xn = users.SelectSingleNode(string.Format("/plcClass/PLC[plc_Id = \"{0}\"]", txtNumber.Text));

            xn["plc_Id"].InnerText = txtNumber.Text;
            xn["plc_IP"].InnerText = txtIPAddress.Text;
            xn["plc_Address"].InnerText = txtAddress.Text;
            xn["plc_Discription"].InnerText = txtDiscription.Text;
            doc.Save(@"..\..\XMLFile.xml");
            LoadInitData();

            txtAddress.Clear();
            txtDiscription.Clear();
            txtIPAddress.Clear();
            txtNumber.Clear();
        }

        /// <summary>
        /// 选择修改行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNumber.Text = dataGridViewSourse.CurrentRow.Cells[0].Value.ToString();
            txtAddress.Text = dataGridViewSourse.CurrentRow.Cells[1].Value.ToString();
            txtIPAddress.Text = dataGridViewSourse.CurrentRow.Cells[2].Value.ToString();
            txtDiscription.Text = dataGridViewSourse.CurrentRow.Cells[4].Value.ToString();
        }
    }
}

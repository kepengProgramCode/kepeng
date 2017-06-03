using Hnt.DeviceListener;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        
        public MainForms()
        {
            InitializeComponent();

            this.dataGridViewSourse.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //表示不显示初始的默认行
            this.dataGridViewSourse.AllowUserToAddRows = false;
            dataGridViewSourse.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // 启用跨线程操作
            CheckForIllegalCrossThreadCalls = false;
            server = DeviceService.Register(80, "192.168.1.118", 9010, "10.2.1.221", 9001);
            device = new PLCDevice("10.2.1.201", DeviceType.Siemens_S7_1200);
        }


        
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadInitData();
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
            dataGridViewSourse.Columns[3].Width = 240;
            dataGridViewSourse.Columns[4].Width = 85;

            for (int i = 0; i < plcModeList.Count; i++)
            {
                address[i] = device.Add(plcModeList[i].Plc_Address);
                address[i].ValueChanged += MainForms_ValueChanged;
            }
            server.ListenDevice(device);
        }

        void MainForms_ValueChanged(PLCAddress sender, ValueChangedEventArgs e)
        {
            
        }


        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = dataGridViewSourse.SelectedRows[0].Cells[0].Value.ToString();

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
    }
}

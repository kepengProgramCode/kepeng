using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLC_ValueMonitor
{
    /// <summary>
    /// ChangeWindowsValue.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeWindowsValue : Window
    {

        public ChangeWindowsValue(string[]Attributes)
        {
            InitializeComponent();
            txtDBAddress.Text = Attributes[1];
            txtAddressValue.Text = Attributes[2];
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsol_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // 判断这个是整形的值
            if (txtDBAddress.Text.Split(',')[1].StartsWith("I"))
            {
                Regex rge = new Regex(@"^[0-9]*$");
                if (rge.IsMatch(txtChangeValue.Text))
                {
                    dBValue = txtChangeValue.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入正确的格式", "警告！", MessageBoxButton.OK);
                }
            }
            // 其他类型的值
            else
            {

            }
        }
    
    


        private string dBValue;
        public string DBValue
        {
            get { return dBValue; }
            set { dBValue = value; }
        }
    }
}

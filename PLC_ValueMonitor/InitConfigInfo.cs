using PLC_ValueMonitor.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PLC_ValueMonitor
{
   public class InitConfigInfo
    {

        private static Dictionary<string, WorkGroupElement> workGroups;
        private static Dictionary<string, TaskParameterElement> taskParameters;
        public static Dictionary<string, WorkGroupElement> WorkGroups
        {
            get
            {
                if (workGroups == null)
                {
                    Init("groupSection");
                }
                return workGroups;
            }
        }
        public static Dictionary<string, TaskParameterElement> TaskParameters
        {
            get
            {
                if (taskParameters == null)
                {
                    Init("taskSection");
                }
                return taskParameters;
            }
        }


        public static void Init(string sectionType)
        {
            if (sectionType == "groupSection")
            {
                WorkSection groupSection = (WorkSection)ConfigurationManager.GetSection("sntConfig/workGroups");
                
                workGroups = new Dictionary<string, WorkGroupElement>();
                for (int i = 0; i < groupSection.WorkGroups.Count; i++)
                {
                    WorkGroupElement element = groupSection.WorkGroups[i];
                    workGroups.Add(element.Type, element);
                }
            }
            if (sectionType == "taskSection")
            {
            TaskSection taskSection = (TaskSection)ConfigurationManager.GetSection("sntConfig/taskParameters");
            taskParameters = new Dictionary<string, TaskParameterElement>();
            for (int i = 0; i < taskSection.TaskParameters.Count; i++)
            {
                TaskParameterElement element = taskSection.TaskParameters[i];
                taskParameters.Add(element.Type, element);
            }
            }
        }

         /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败 
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        public static bool PingIpOrDomainName(string stockerIp, string inLineIP, string outLineIp)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objStockerPinReply = objPingSender.Send(stockerIp, intTimeout, buffer, objPinOptions);
                PingReply objInLinePinReply = objPingSender.Send(inLineIP, intTimeout, buffer, objPinOptions);
                PingReply objOutLinePinReply = objPingSender.Send(outLineIp, intTimeout, buffer, objPinOptions);
                string pingStockerResult = objStockerPinReply.Status.ToString();
                string pingIilineResult = objInLinePinReply.Status.ToString();
                string pingOutLineResult = objOutLinePinReply.Status.ToString();
                if ((pingStockerResult == "Success")&& (pingIilineResult == "Success") && (pingOutLineResult == "Success"))
                {
                    return true;
                }
                   
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

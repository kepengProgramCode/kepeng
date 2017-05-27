using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PLC_ValueMonitor.Configuration
{
    public class DeviceElement : ConfigurationElement
    {
        [ConfigurationProperty("deviceName", IsRequired = false)]
        public string DeviceName
        {
            get
            {
                return (string)base["deviceName"];
            }
            set
            {
                base["deviceName"] = value;
            }
        }

        [ConfigurationProperty("Name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)base["Name"];
            }
            set
            {
                base["Name"] = value;
            }
        }

       
        [ConfigurationProperty("deviceNumber", IsRequired = false)]
        public string DeviceNumber
        {
            get
            {
                return (string)base["deviceNumber"];
            }
            set
            {
                base["deviceNumber"] = value;
            }
        }

       

        [ConfigurationProperty("ip", IsRequired = false)]
        public string Ip
        {
            get
            {
                return (string)base["ip"];
            }
            set
            {
                base["ip"] = value;
            }
        }
        [ConfigurationProperty("port", IsRequired = false)]
        public string Port
        {
            get
            {
                return (string)base["port"];
            }
            set
            {
                base["port"] = value;
            }
        }
        [ConfigurationProperty("PLCip", IsRequired = false)]
        public string PLCip
        {
            get
            {
                return (string)base["PLCip"];
            }
            set
            {
                base["PLCip"] = value;
            }
        }
        [ConfigurationProperty("PLCAddress", IsRequired = false)]
        public string PLCAddress
        {
            get
            {
                return (string)base["PLCAddress"];
            }
            set
            {
                base["PLCAddress"] = value;
            }
        }



        [ConfigurationProperty("socketClientIP", IsRequired = false)]
        public string SocketClientIP
        {
            get
            {
                return (string)base["socketClientIP"];
            }
            set
            {
                base["socketClientIP"] = value;
            }
        }
        [ConfigurationProperty("socketClientPort", IsRequired = false)]
        public string SocketClientPort
        {
            get
            {
                return (string)base["socketClientPort"];
            }
            set
            {
                base["socketClientPort"] = value;
            }
        }
        private string deviceStatus;
        public string DeviceStatus
        {
            get { return deviceStatus; }
            set { deviceStatus = value; }
        }


        private string PLCValue;

        public string PLC_Value
        {
            get { return PLCValue; }
            set { PLCValue = value; }
        }
    }
}

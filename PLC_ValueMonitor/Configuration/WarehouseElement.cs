using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PLC_ValueMonitor.Configuration
{
    public class WarehouseElement : ConfigurationElement
    {
        [ConfigurationProperty("address", IsRequired = false)]
        public string Address
        {
            get
            {
                return (string)base["address"];
            }
            set
            {
                base["address"] = value;
            }
        }
        [ConfigurationProperty("lineIP", IsRequired = false)]
        public string LineIP
        {
            get
            {
                return (string)base["lineIP"];
            }
            set
            {
                base["lineIP"] = value;
            }
        }
        [ConfigurationProperty("number", IsRequired = true)]
        public string Number
        {
            get
            {
                return (string)base["number"];
            }
            set
            {
                base["number"] = value;
            }
        }
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public DeviceElementCollection Devices
        {
            get
            {
                return (DeviceElementCollection)base[""];
            }
        }
    }
}

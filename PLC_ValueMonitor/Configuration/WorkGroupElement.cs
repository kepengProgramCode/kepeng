using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PLC_ValueMonitor.Configuration
{
    public class WorkGroupElement : ConfigurationElement
    {
        [ConfigurationProperty("store", IsRequired = false)]
        public string Store
        {
            get
            {
                return (string)base["store"];
            }
            set
            {
                base["store"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }
        [ConfigurationProperty("", IsDefaultCollection = true)]
        //public WarehouseElementCollection Warehouses
        //{
        //    get
        //    {
        //        return (WarehouseElementCollection)base[""];
        //    }
        //}
        public DeviceElementCollection Devices
        {
            get
            {
                return (DeviceElementCollection)base[""];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_ValueMonitor.Configuration
{
    public class TaskParameterElement : ConfigurationElement
    {
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
        public LocationAddrElementCollection LocationAddrs
        {
            get
            {
                return (LocationAddrElementCollection)base[""];
            }
        }
    }
}

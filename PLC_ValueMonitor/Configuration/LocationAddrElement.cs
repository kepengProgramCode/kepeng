using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_ValueMonitor.Configuration
{
   public  class LocationAddrElement :ConfigurationElement
    {
         [ConfigurationProperty("type", IsRequired = false)]
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
       /// <summary>
       /// 排
       /// </summary>
        [ConfigurationProperty("row", IsRequired = true)]
         public string Row
        {
            get
            {
                return (string)base["row"];
            }
            set
            {
                base["row"] = value;
            }
        }
       /// <summary>
       /// 列
       /// </summary>
        [ConfigurationProperty("column", IsRequired = true)]
        public string Column
        {
            get
            {
                return (string)base["column"];
            }
            set
            {
                base["column"] = value;
            }
        }
        /// <summary>
        /// 层
        /// </summary>
        [ConfigurationProperty("layer", IsRequired = true)]
        public string Layer
        {
            get
            {
                return (string)base["layer"];
            }
            set
            {
                base["layer"] = value;
            }
        }
        [ConfigurationProperty("numbers", IsRequired = true)]
        public string Numbers
        {
            get
            {
                return (string)base["numbers"];
            }
            set
            {
                base["numbers"] = value;
            }
        }
        [ConfigurationProperty("locationTypeAddr", IsRequired = false)]
        public string LocationTypeAddr
        {
            get
            {
                return (string)base["locationTypeAddr"];
            }
            set
            {
                base["locationTypeAddr"] = value;
            }
        }
        [ConfigurationProperty("remark", IsRequired = true)]
        public string Remark
        {
            get
            {
                return (string)base["remark"];
            }
            set
            {
                base["remark"] = value;
            }
        }

    }
}

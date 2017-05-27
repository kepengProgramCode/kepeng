using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_ValueMonitor.Configuration
{
    public class LocationAddrElementCollection : ConfigurationElementCollection
    {
         protected override ConfigurationElement CreateNewElement()
        {
            return new LocationAddrElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LocationAddrElement)element).Type;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override string ElementName
        {
            get
            {
                return "locationAddr";
            }
        }

        public LocationAddrElement this[int index]
        {
            get
            {
                return (LocationAddrElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
   


    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PLC_ValueMonitor.Configuration
{
    public class WorkGroupElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkGroupElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkGroupElement)element).Type;
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
                return "workGroup";
            }
        }

        public WorkGroupElement this[int index]
        {
            get
            {
                return (WorkGroupElement)BaseGet(index);
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_ValueMonitor.Configuration
{
    public class TaskParameterElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TaskParameterElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TaskParameterElement)element).Type;
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
                return "taskParameter";
            }
        }

        public TaskParameterElement this[int index]
        {
            get
            {
                return (TaskParameterElement)BaseGet(index);
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

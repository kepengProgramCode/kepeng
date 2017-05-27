using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PLC_ValueMonitor.Configuration
{
    public class WorkSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public WorkGroupElementCollection WorkGroups
        {
            get
            {
                return (WorkGroupElementCollection)base[""];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_ValueMonitor.Configuration
{
    public class TaskSection : ConfigurationSection
    {

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public TaskParameterElementCollection TaskParameters
        {
            get
            {
                return (TaskParameterElementCollection)base[""];
            }
        }
    }
}

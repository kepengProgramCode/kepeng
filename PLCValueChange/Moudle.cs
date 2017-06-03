using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCValueChange
{
    public class PLCModel
    {
        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string plc_Address;

        public string Plc_Address
        {
            get { return plc_Address; }
            set { plc_Address = value; }
        }
        string ip_Address;

        public string Ip_Address
        {
            get { return ip_Address; }
            set { ip_Address = value; }
        }
        string plc_Value;

        public string Plc_Value
        {
            get { return plc_Value; }
            set { plc_Value = value; }
        }

        string discription;

        public string Discription
        {
            get { return discription; }
            set { discription = value; }
        }
    }
}

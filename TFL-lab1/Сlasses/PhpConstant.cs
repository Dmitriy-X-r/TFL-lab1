using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TFL_lab1.Сlasses
{
    internal class PhpConstant
    {
        string _name;
        public string name { get => _name; }

        Type _type;
        public Type type { get => _type; }

        object _value;
        public object value { get=> _value;}

        public PhpConstant(string name, Type type, object value)
        {
            _name = name; 
            _type = type;
            _value = value; 
        }
    }
}

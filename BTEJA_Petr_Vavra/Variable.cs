using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_Petr_Vavra
{
    public class Variable
    {

        //ident
        public string Name { get; set; }
        public object? Value { get; set; }
        public DataType DataType { get; set; }

        public Variable()
        {

        }

    }
}

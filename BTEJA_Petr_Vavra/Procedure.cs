using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_Petr_Vavra
{
    public class Procedure
    {

        public string Name { get; set; }
        public List<VarFormal> VarFormals { get; set; }
        public DataType ReturnType { get; set; }
        public List<object?> Body { get; set; }

        public object? ReturnExpression { get; set; }

    }
}

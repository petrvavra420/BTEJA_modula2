using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_Petr_Vavra
{
    public class Variable :ICloneable
    {

        //ident
        public string Name { get; set; }
        public object? Value { get; set; }
        public DataType DataType { get; set; }

        //array
        public int? ArraySize { get; set; }
        public List<object?> ArrayValues { get; set; }
        public DataType ArrayType { get; set; }

        public Variable()
        {
            ArraySize = null;
            ArrayValues = new List<object?>();
        }

        public Variable DeepCopy()
        {
            Variable copy = new Variable();
            copy.Name = this.Name;
            copy.Value = DeepCopyValue(this.Value); // Metoda pro hlubokou kopii hodnoty
            copy.DataType = this.DataType;
            copy.ArraySize = this.ArraySize;

            if (this.ArrayValues != null)
            {
                copy.ArrayValues = new List<object?>(this.ArrayValues.Select(DeepCopyValue));
            }

            copy.ArrayType = this.ArrayType;

            return copy;
        }

        private object? DeepCopyValue(object? value)
        {
            if (value == null)
            {
                return null;
            }

            // Pokud je hodnota typu string nebo je to hodnota value type, nepotřebujeme hlubokou kopii
            if (value is string || value.GetType().IsValueType)
            {
                return value;
            }

            // Pokud je hodnota reference type, provést hlubokou kopii
            if (value is ICloneable cloneable)
            {
                return cloneable.Clone();
            }

            if (value is object)
            {
                return value;
            }

            throw new ArgumentException("Nelze provést hlubokou kopii hodnoty neznámého typu.");
        }

        public object Clone()
        {
            Variable variable = new Variable();
            variable.Name = this.Name;
            variable.Value = this.Value;
            variable.DataType = this.DataType;
            variable.ArraySize = this.ArraySize;
            variable.ArrayValues = this.ArrayValues;
            variable.ArrayType = this.ArrayType;
            return variable;
        }
    }
}

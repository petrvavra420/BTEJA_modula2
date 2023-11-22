using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BTEJA_Petr_Vavra
{
    public class ModulaVisitor : Modula2BaseVisitor<object?>
    {

        private List<Variable> Variables { get; } = new List<Variable>();


        //TODO POLE
        //POLE CHECK TYPŮ?
        public override object VisitVarStatement([NotNull] Modula2Parser.VarStatementContext context)
        {
            string substrVAR = context.GetText().Substring(0, 3);

            var identifier = context.ident().GetText();
            DataType dataType = (DataType)VisitType(context.type());
            if (dataType != DataType.ARRAY)
            {
                Variable varNew = new Variable();
                varNew.Name = identifier;
                varNew.DataType = dataType;
                //pokud obsahuje i inicializaci tak načteme expr
                if (context.GetText().Contains(":="))
                {
                    //kontrola přiřazení správné hodnoty dle datového typu
                    varNew.Value = VisitExpression(context.expression());
                    Type valueType = varNew.Value.GetType();
                    if (
                        !((valueType == typeof(int) && varNew.DataType == DataType.INTEGER)
                        || (valueType == typeof(String) && varNew.DataType == DataType.CHAR)
                        || (valueType == typeof(float) && varNew.DataType == DataType.REAL)
                        || (valueType == typeof(int) && varNew.DataType == DataType.REAL)
                        )
                        )
                    {
                        throw new Exception("Invalid value assigned.");
                    }
                    Console.WriteLine("TYYYYYYYYP: " + varNew.Value.GetType());
                }
                Variables.Add(varNew);
            }
            return null;
        }
        public override object VisitAssignment([NotNull] Modula2Parser.AssignmentContext context)
        {
            var varName = context.ident().GetText();
            var value = VisitExpression(context.expression(0));
            Console.WriteLine(varName + " s hodnotou: " + value);

            //doufejme že nikdy nebudu potřebovat tagat variable objekt místo hodnoty..
            //Variable varFind = (Variable)VisitIdent(context.ident());
            Variable varFind = Variables.Find(variable => variable.Name == varName);

            //kontrola datových typů
            Type valueType = value.GetType();
            if (
                        !((valueType == typeof(int) && varFind.DataType == DataType.INTEGER)
                        || (valueType == typeof(String) && varFind.DataType == DataType.CHAR)
                        || (valueType == typeof(float) && varFind.DataType == DataType.REAL)
                        || (valueType == typeof(int) && varFind.DataType == DataType.REAL)
                        )
                        )
            {
                throw new Exception("Invalid value assigned.");
            }

            varFind.Value = value;

            return null;
        }

        public override object VisitType([NotNull] Modula2Parser.TypeContext context)
        {
            string dataTypeText = context.GetText();
            switch (dataTypeText)
            {
                case "INTEGER":
                    return DataType.INTEGER;
                    break;
                case "REAL":
                    return DataType.REAL;
                    break;
                case "CHAR":
                    return DataType.CHAR;
                    break;
                case "ARRAY":
                    return DataType.ARRAY;
                    break;
                default:
                    break;
            }
            return base.VisitType(context);
        }

        public override object VisitExpression([NotNull] Modula2Parser.ExpressionContext context)
        {
            object result = VisitTerm(context.term(0)); // Zpracuje první term

            for (int i = 0; i < context.addOp().Length; i++)
            {
                string op = context.addOp(i).GetText(); // Získá operátor plus nebo minus

                if (op == "+")
                {
                    result = Add(result, VisitTerm(context.term(i + 1))); // Přida další term
                }
                else if (op == "-")
                {
                    result = Subtract(result, VisitTerm(context.term(i + 1))); // Odečte další term
                }
            }
            return result;
        }

        public override object VisitTerm([NotNull] Modula2Parser.TermContext context)
        {
            object result = VisitFactor(context.factor(0)); // Zpracuje první factor

            for (int i = 0; i < context.multOp().Length; i++)
            {
                string op = context.multOp(i).GetText(); // Získá operátor násobení nebo dělení

                if (op == "*")
                {
                    result = Multiply(result, VisitFactor(context.factor(i + 1))); // vynásobí s  dalším factorem
                }
                else if (op == "/")
                {
                    result = Divide(result, VisitFactor(context.factor(i + 1))); // vydělí s dalším factorem
                }
            }
            return result;

            return base.VisitTerm(context);
        }

        private object? Divide(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l / r;
            }
            if (left is float lf && right is float rf)
            {
                return lf / rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt / rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat / rInt;
            }
            throw new Exception("unable to subtract " + left + " and " + right);
        }

        private object? Multiply(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l * r;
            }
            if (left is float lf && right is float rf)
            {
                return lf * rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt * rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat * rInt;
            }
            throw new Exception("unable to subtract " + left + " and " + right);
            throw new NotImplementedException();
        }

        private object? Negate(object? result)
        {
            if (result is int rInt)
            {
                return rInt * -1;
            }
            if (result is float rFloat)
            {
                return rFloat * -1;
            }

            throw new NotImplementedException();
        }

        private object? Subtract(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l - r;
            }
            if (left is float lf && right is float rf)
            {
                return lf - rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt - rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat - rInt;
            }
            throw new Exception("unable to subtract " + left + " and " + right);
        }

        private object? Add(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l + r;
            }
            if (left is float lf && right is float rf)
            {
                return lf + rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt + rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat + rInt;
            }

            Console.WriteLine("typ: " + left.GetType());
            throw new Exception("unable to add " + left + " and " + right);
        }

        public override object VisitIdent([NotNull] Modula2Parser.IdentContext context)
        {
            var varName = context.GetText();
            //return varName;
            Console.WriteLine("identifikator: " + varName);
            Variable variableFind = Variables.Find(variable => variable.Name == varName);


            if (variableFind == null)
            {
                throw new Exception("Variable " + varName + " is not defined.");
            }
            return variableFind.Value;

            return base.VisitIdent(context);
        }

        //public object CheckIdentExistence([NotNull] Modula2Parser.IdentContext context)
        //{
        //    var varName = context.GetText();
        //    if (!Variables.ContainsKey(varName))
        //    {
        //        throw new Exception("Variable " + varName + " is not defined.");
        //    }
        //    return Variables[varName];

        //}

        public override object VisitNumber([NotNull] Modula2Parser.NumberContext context)
        {
            return int.Parse(context.GetText());
        }


        public override object VisitCharacter([NotNull] Modula2Parser.CharacterContext context)
        {
            //tato metoda odstraní uvozovky a vrátí znak
            string text = context.GetText();
            int startIndex = text.IndexOf('"') + 1; // Najdi pozici první uvozovky a přidej 1
            int endIndex = text.LastIndexOf('"'); // Najdi pozici poslední uvozovky
            string extractedText = text.Substring(startIndex, endIndex - startIndex);
            return extractedText;
        }
        public override object VisitAddOp([NotNull] Modula2Parser.AddOpContext context)
        {
            return context.GetText();
        }

        public override object VisitRealNumber([NotNull] Modula2Parser.RealNumberContext context)
        {
            //nastavení kultury pro tečku
            CultureInfo culture = CultureInfo.InvariantCulture;
            float number = float.Parse(context.GetText(), NumberStyles.Float, culture);
            return number;
        }



    }
}

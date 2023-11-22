using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Microsoft.VisualBasic;
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
            //pokud není pole
            if (dataType != DataType.ARRAY)
            {
                Variable varNew = new Variable();
                varNew.Name = identifier;
                varNew.DataType = dataType;
                //pokud obsahuje i inicializaci tak načteme expr
                if (context.GetText().Contains(":="))
                {
                    //načtení hodnoty
                    varNew.Value = VisitExpression(context.expression());
                    //kontrola přiřazení správné hodnoty dle datového typu
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
            throw new Exception("unable to divide " + left + " and " + right);
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
            throw new Exception("unable to multiply " + left + " and " + right);
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

            throw new Exception("unable to negate " + result);
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

            //Console.WriteLine("typ: " + left.GetType());
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


        //forStatement: 'FOR' ident ':=' expression 'TO' expression ('BY' expression)? 'DO' (statement ';')+ 'END';
        public override object VisitForStatement([NotNull] Modula2Parser.ForStatementContext context)
        {
            //zjistíme název první proměnné
            object ident = context.ident().GetText();
            //zjistíme jestli existuje
            Variable variable = tryFindVariableByIdent(ident);
            //pokud neexistuje, vytvoříme a přidáme do seznamu
            if (variable == null)
            {
                variable = new Variable();
                variable.Name = (string)ident;
                variable.DataType = DataType.INTEGER;
                Variables.Add(variable);
            }
            //kontrola pokud už existující proměnná je typu INTEGER
            if (variable.DataType != DataType.INTEGER)
            {
                throw new Exception("Existing variable '" + variable.Name + "' is not type INTEGER");
            }
            //přiřadíme hodnotu
            variable.Value = VisitExpression(context.expression(0));

            int rangeFrom = (int)variable.Value;
            //uložíme si pravou krajní mez FOR cyklu
            int rangeTo = (int)VisitExpression(context.expression(1));
            //výchozí hodnota inkrementace bude jedna, není li specifikování "BY" jinak
            int rangeBy = 1;
            //pokud existuje třetí výraz v definici, budeme předpokládat že se jedná o "BY"
            if (context.expression(2) != null)
            {
                rangeBy = (int)VisitExpression(context.expression(2));
            }

            Console.WriteLine("---DEBUG: " + ident + " a hodnota: " + variable.Value + ", TO: " + rangeTo + ", BY: " + rangeBy);

            for (int i = rangeFrom; i < rangeTo; i += rangeBy)
            {
                foreach (var statement in context.statement())
                {
                    VisitStatement(statement);
                }
            }
            return null;
        }


        //ifStatement: 'IF' condition 'THEN' (statement ';')+ ('ELSIF' condition 'THEN' (statement ';')+)* ('ELSE' (statement ';')+)? 'END';
        public override object VisitIfStatement([NotNull] Modula2Parser.IfStatementContext context)
        {

            bool condition = (bool)VisitCondition(context.condition(0));

            //zkontrolujeme první IF, pokud je TRUE, provedeme všechny příkazy
            if (condition)
            {
                Console.WriteLine("---DEBUG: Condition is TRUE");
                Visit(context.ifBlock());
                return null;
            }
            else if (!condition)
            {
                Console.WriteLine("---DEBUG: MAIN Condition is FALSE");
                //pokud je FALSE, zkontrolujeme zda existuje ELSEIF
                //POKUD ANO, zkontrolujeme zda je TRUE, pokud ano, provedeme příkazy
                //POKUD NE, zkontrolujeme zda existuje ELSE, pokud ano, provedeme příkazy
                //int conditionCount = 0;
                //foreach (var conditionElse in context.condition())
                //{
                //    bool conditionElseResult = (bool)VisitCondition(conditionElse);
                //    if (conditionElseResult)
                //    {
                //        Console.WriteLine("---DEBUG: ELSEIF Condition is TRUE");
                //        //foreach (var statement in context.ifBlock())
                //        //{
                //        //    VisitIfBlock(statement);
                //        //}
                //        //Visit(else);
                        
                //        Visit(context.elseIfBlock(conditionCount - 1));

                //        return null;
                //    }
                //    else
                //    {
                //        Console.WriteLine("---DEBUG: ELSEIF Condition is FALSE");
                //        Console.WriteLine("---DEBUG: Moving on...");
                //    }

                //    conditionCount++;
                //}
                for (int i = 1; i < context.condition().Length; i++)
                {
                    bool conditionElseResult = (bool)VisitCondition(context.condition(i));
                    if (conditionElseResult)
                    {
                        Console.WriteLine("---DEBUG: ELSEIF Condition is TRUE");
                        // minus jedna protože první je IF
                        Visit(context.elseIfBlock(i - 1));
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("---DEBUG: ELSEIF Condition is FALSE");
                        Console.WriteLine("---DEBUG: Moving on...");
                    }

                }


                //ELSE
                //foreach (var statement in context.ifBlock())
                //{
                //    VisitIfBlock(statement);
                //}
                Visit(context.elseBlock());


            }


            return null;
        }





        //condition: expression ('>=' | '<=' | '>' | '<' | '=' | '#') expression | ident | ('1' | '0');
        public override object VisitCondition([NotNull] Modula2Parser.ConditionContext context)
        {

            string text = context.GetText();


            //true nebo false check ('1' | '0')
            if (text == "1")
            {
                return true;
            }
            else if (text == "0")
            {
                return false;
            }

            //check ('>=' | '<=' | '>' | '<' | '=' | '#')
            if (text.Contains(">="))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = GreaterThanOrEqual(left, right);
                return result;
            }

            if (text.Contains("<="))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = LessThanOrEqual(left, right);
                return result;
            }

            if (text.Contains(">"))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = GreaterThan(left, right);
                return result;
            }

            if (text.Contains("<"))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = LessThan(left, right);
                return result;
            }

            if (text.Contains("="))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = IsEqual(left, right);
                return result;
            }
            if (text.Contains("#"))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = IsNotEqual(left, right);
                return result;
            }

            //check ident
            if (context.ident() != null)
            {
                object ident = context.ident().GetText();
                Variable variable = tryFindVariableByIdent(ident);
                if (variable == null)
                {
                    throw new Exception("Variable " + ident + " is not defined.");
                }

                if (variable.Value == "1")
                {
                    return true;
                }
                if (variable.Value == "0")
                {
                    return false;
                }
                if (variable.DataType == DataType.INTEGER)
                {
                    if ((int)variable.Value == 1)
                    {
                        return true;
                    }
                    if ((int)variable.Value == 0)
                    {
                        return false;
                    }
                }
                if (variable.DataType == DataType.REAL)
                {
                    if ((float)variable.Value == 1)
                    {
                        return true;
                    }
                    if ((float)variable.Value == 0)
                    {
                        return false;
                    }
                }
            }
            return null;
        }


        private bool GreaterThan(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l > r;
            }
            if (left is float lf && right is float rf)
            {
                return lf > rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt > rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat > rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool GreaterThanOrEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l >= r;
            }
            if (left is float lf && right is float rf)
            {
                return lf >= rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt >= rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat >= rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool LessThanOrEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l <= r;
            }
            if (left is float lf && right is float rf)
            {
                return lf <= rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt <= rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat <= rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool LessThan(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l < r;
            }
            if (left is float lf && right is float rf)
            {
                return lf < rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt < rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat < rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool IsEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l == r;
            }
            if (left is float lf && right is float rf)
            {
                return lf == rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt == rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat == rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool IsNotEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l != r;
            }
            if (left is float lf && right is float rf)
            {
                return lf != rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt != rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat != rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }

        //pomocná metoda na hledání proměnných v seznamu dle identifikátoru
        private Variable tryFindVariableByIdent(object ident)
        {
            try
            {
                string identString = ident as string;

                if (identString != null)
                {
                    Variable variableFind = Variables.FirstOrDefault(variable => variable.Name == identString);

                    //nalezen prvek
                    if (variableFind != null)
                    {
                        return variableFind;
                    }
                    //nenalazen, vracíme NULL
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("Ident is empty.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


    }
}
